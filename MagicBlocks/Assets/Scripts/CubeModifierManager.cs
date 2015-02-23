using UnityEngine;
using System.Collections;

//this class will provide the interface that determines which CubeModifier the user intends to attach
public class CubeModifierManager : MonoBehaviour {

    public static CubeModifierManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void OnTap()
    {
        if (SelectionManager.Instance.GetSelection().tag != "CubeModifier")
            return;

        RaycastHit hit;
        Ray ray;

        if (Utils.TouchCast(out hit, out ray))
        {
            if (hit.transform.tag == "Cube")
            {
                GameObject newModifier = (GameObject)Instantiate(SelectionManager.Instance.GetSelection());
                Cube cube = hit.transform.GetComponent<Cube>();
                Transform anchor = hit.transform.GetComponent<Cube>().getClosestAnchor(hit);
                
                if (cube.AnchorContainsModifier(anchor, newModifier.name))
                {
                    cube.RemoveModifierFromAnchor(anchor, newModifier.name);
                    DestroyImmediate(newModifier);
                    return;//the modifier already exists at this anchor, remove it
                }
                
                if (newModifier.GetComponent<Modifier>().CanHaveMultiplePerCube == false) //if this modifier type cannot have multiple instances on a cube
                    cube.RemoveAllModifiersOfType(newModifier.name);

                AddModifier(newModifier, hit);
                newModifier.GetComponent<Modifier>().retrieveCube();
                //Debug.Log(""+newModifier.name+" instantiated");
            }
        }

    }


    private void AddModifier(GameObject modifier, RaycastHit hit)//anchors the transform, assuming that it is a modifier anchor
    {
        hit.transform.gameObject.GetComponent<Cube>().attachToClosestAnchor(modifier, hit);        
    }

}
