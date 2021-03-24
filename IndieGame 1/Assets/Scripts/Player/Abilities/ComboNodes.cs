using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System.Linq;

public class ComboNodes : MonoBehaviour {

    [HideInInspector]
    public string attackType;
    [HideInInspector]
    public List<ComboNodes> nextMove;
    [HideInInspector]
    public string damage;
    //can also do animations here

    public void DoMove()
    {
        print("doing move");
    }
}
