using UnityEngine;

public class CleanupHealthBars : MonoBehaviour
{

    /// <summary>
    /// Cleans up the healthbars of the enemies from the hirearchy when changing levels
    /// </summary>
    public void CleanupUI()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (transform.GetChild(i).GetComponent<EnemyHealthBar>() != null)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
