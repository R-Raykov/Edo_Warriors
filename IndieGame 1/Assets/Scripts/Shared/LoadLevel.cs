using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class LoadLevel : MonoBehaviour
{
    private static string _levelToLoad;
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Text _loadingText;
    [SerializeField] private GameObject _progressIndicator;
    [SerializeField] private MusicManager _musicManager;
    [SerializeField] private PostProcessingProfile[] ppProfiles;

    [SerializeField] private CharacterStats _p1;
    [SerializeField] private CharacterStats _p2;

    private bool _loading = false;

    private void Start()
    {
        GameManager.Instance.LevelLoader = this;
        //GameManager.Instance.PlayerRespawnController.CheckpointSpawnPoint = FindObjectOfType<Checkpoint>().transform;
    }

    // Do all the dirty work between levels 
    private void OnLevelWasLoaded(int level)
    {
        switch(level)
        {
            case 0:
                _musicManager.PlayMenu();
                GameManager.Instance.MainCamera.GetComponent<PostProcessingBehaviour>().profile = ppProfiles[0];
                break;
            case 1:
                _musicManager.PlayMenu();
                GameManager.Instance.MainCamera.GetComponent<PostProcessingBehaviour>().profile = ppProfiles[1];
                break;
            case 2:
                _musicManager.PlayMain();
                GameManager.Instance.MainCamera.GetComponent<PostProcessingBehaviour>().profile = ppProfiles[2];
                break;
            case 3:
                _musicManager.PlayMain();
                GameManager.Instance.MainCamera.GetComponent<PostProcessingBehaviour>().profile = ppProfiles[0];
                break;
            default:
                _musicManager.PlayMain();
                GameManager.Instance.MainCamera.GetComponent<PostProcessingBehaviour>().profile = ppProfiles[2];
                break;
        }

        transform.parent.GetComponent<CleanupHealthBars>().CleanupUI();

        ActivePlayers players = GetComponent<ActivePlayers>();

        players.CanJoin = level == 1 ? true : false;
        _p1.MoveToSpawn();
        _p2.MoveToSpawn();

        GameManager.Instance.PlayerRespawnController.CheckpointSpawnPoint = FindObjectOfType<Checkpoint>().transform;

        GameManager.Instance.MainCamera.GetComponent<CoopCamera>().ReplaceExistingCamera();
    }

    public void LoadScene(string level)
    {
        _loadingScreen.SetActive(true);
        //_loadingText.gameObject.SetActive(true);
        //_ProgressIndicator.SetActive(true);
        _loadingText.text = "Loading...";

        if (!_loading)
        {
            _loading = true;
            StartCoroutine(loadSceneAsync(level));
        }
    }

    private IEnumerator loadSceneAsync(string level)
    {
        yield return new WaitForSeconds(1f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            _progressIndicator.transform.Rotate(new Vector3(0, 0, -10));

            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            _loadingText.text = "Loading..." + progress * 100f + "%";

            if(operation.progress == 0.9f)
            {
                _loadingText.text = "Press A to Continue";
                if(Input.GetButtonDown("AJ" + 1) || Input.GetButtonDown("AJ" + 2) || Input.GetKeyDown(KeyCode.A))
                {
                    operation.allowSceneActivation = true;
                    _loadingScreen.SetActive(false);
                    _loading = false;
                }
            }
            yield return null;
        }
    }
}
