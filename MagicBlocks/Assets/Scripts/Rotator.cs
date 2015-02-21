using UnityEngine;
using System.Collections;

public class Rotator : Modifier{

    public float Speed;
    private bool _isRotating = false;
    private int _rotateCount = 0;

	// Use this for initialization
	void Awake () {
        //CanHaveMultiplePerCube = true;
        CanParentCubes = true;
        //LeanTween.move(gameObject, Vector3.up * 10, 1);
        
	}

    void OnTap()
    {
        if (SelectionManager.Instance.GetSelection().tag != "Selector")
            return;

        RaycastHit hit;
        Ray ray;

        if (Utils.TouchCast(out hit, out ray))
        {
            if (hit.transform == _cube)
            {
                Trigger();
            }
        }
    }

    void Trigger()
    {
        if (!LeanTween.isTweening(gameObject))
        {
            LeanTween.rotateAround(gameObject, transform.parent.forward, 90f, 1.0f / Speed).setEase(LeanTweenType.linear).setOnComplete(FinishRotation);
        }
        else
        {
            _rotateCount++;
        }
    }

    public void FinishRotation()
    {
        if (_rotateCount > 0)
        {
            _rotateCount--;
            Trigger();
        }
    }

}
