using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IActivatable
{
    [Tooltip("The icon image of the skill")]
    [SerializeField] private Transform _icon;

	// Use this for initialization
	void Start ()
    {
        _icon = transform.GetChild(0);
	}

    public void Activate()
    {
        _icon.GetComponent<IActivatable>().Activate();
    }
}
