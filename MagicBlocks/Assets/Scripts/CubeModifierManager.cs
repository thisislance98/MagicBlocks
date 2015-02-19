using UnityEngine;
using System.Collections;

//this class will provide the interface that determines which CubeModifier the user intends to attach
public class CubeModifierManager : MonoBehaviour {

    public static CubeModifierManager Instance;

    private static Transform _modifierType;


    void Awake()
    {
        Instance = this;
    }



    public static void SetModifier(Transform modifier)//sets the current modifier type
    {
        if (null != modifier && modifier.tag == "CubeModifier")
        { 
            _modifierType = modifier;
        }
    }

    public static void AnchorModifierToTap()//safe anchor
    {
        if (null != _modifierType)
        {
            AnchorToTap(_modifierType);
        }
    }

    private static void AnchorToTap(Transform transformToAttach)//anchors the transform, assuming that it is a modifier anchor
    {
        Ray ray;
        RaycastHit hit;

        if (Utils.TouchCast(out hit, out ray))
        {
            if (hit.transform.tag == "Cube")
            {
                Cube theCube = (GameObject.Find(hit.transform.name)).GetComponent<Cube>();//access the hit cube's script
                if (null != theCube)
                    theCube.attachToClosestAnchor(transformToAttach, hit);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
