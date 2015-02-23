using UnityEngine;
using System.Collections;

public class Rotator : Modifier{

    public float Speed;

    private int _rotateCount = 0;

	// Use this for initialization
	void Awake () {
        CanHaveMultiplePerCube = true;
        CanParentCubes = true;
	}

    void Trigger()
    {
        if (!LeanTween.isTweening(gameObject))
        {
            IsTriggered = true;
            LeanTween.rotateAround(gameObject, transform.parent.forward, 90f, 1.0f / Speed).setEase(LeanTweenType.linear).setOnComplete(FinishRotation);
        }
        else
        {
            _rotateCount++;
        }
    }

    void DeTrigger()
    {
        _rotateCount = 0;
        IsTriggered = false;
    }

    public void FinishRotation()
    {
        if (_rotateCount > 0)
        {
            _rotateCount--;
            Trigger();
        }
        else
        {
            DeTrigger();
        }
    }

}
