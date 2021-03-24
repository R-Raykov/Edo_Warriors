using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System.Linq;

public class ComboController : MonoBehaviour {

    private List<ComboNodes> baseMoves;
    private List<ComboNodes> currentNodes;

    private XDocument doc;

    private bool meleeAttackCombo, rangeAttackCombo, healingCombo;

	private void Start ()
    {
        doc = XDocument.Load("Assets/Radi test stuff/Scripts/Combos.xml");
        currentNodes = baseMoves = InitilizeTree(doc.Descendants("nextMoves").Elements("move"));
    }

    private void Update()
    {
        print("Current Node " + currentNodes.Count());
        BasicAttacks();
        MeleeCombos();
    }

    private List<ComboNodes> InitilizeTree(IEnumerable<XElement> node)
    {
        return node.Select(x => new ComboNodes()
        {
            attackType = x.Attribute("key").Value,
            nextMove = InitilizeTree(x.Elements("move"))
        }).ToList(); ;
    }

    private void BasicAttacks()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            print("basic key pressed");
            meleeAttackCombo = true;
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {

        }
        else if (Input.GetKeyDown(KeyCode.P))
        {

        }
    }

    private void MeleeCombos()
    {
        if (meleeAttackCombo)
        {
            foreach (ComboNodes n in currentNodes)
            {
                if (n.attackType == "I")
                {
                    //print("gettingThere");
                    // print(n.nextMove.Count);
                    currentNodes = n.nextMove;
                    //print(currentNodes[0]);
                    if (n.attackType == "O")
                    {
                        n.DoMove();
                        break;
                    }
                }
            }
        }
    }
}
