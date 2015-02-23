using UnityEngine;
using System.Collections;

public class Modifier : MonoBehaviour {

    public bool CanHaveMultiplePerCube = true;

    public bool CanParentCubes = false;

    protected Transform _cube = null;

    public bool IsTriggered = false;

    public void retrieveCube()
    {
        if (_cube == null && transform.parent != null && transform.parent.parent != null) { _cube = transform.parent.parent; }
    }
}
