using UnityEngine;
using UnityEngine.UI;

public class PlayerUISet : MonoBehaviour
{
    public PlayerClass classID;
    public UIFillBar healthBar;
    public UIFillBar manaBar;
    public GameObject skillSetHolder;
    public Text points;

    private SkillSlot[] _skills;

    private void Start()
    {
        if(classID != PlayerClass.Defaut) _skills = skillSetHolder.GetComponentsInChildren<SkillSlot>();
    }

    public PlayerClass GetClassID()
    {
        return classID;
    }
}
