using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Detonator : Modifier {

    public float BlastRadius;
    public float ExplosionForce;
    private bool _hasDetonated;

	// Use this for initialization
	void Awake () {
        CanHaveMultiplePerCube = false;
        CanParentCubes = false;
        _hasDetonated = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        Trigger();
    }

    private void Detonate()
    {
        if (_hasDetonated) { return; }//can only detonate once

        //transform.parent.parent.rigidbody.AddExplosionForce(ExplosionForce, transform.position, BlastRadius, 3f);

       /* 
       foreach (var fragment in fragments)
        {
            var posToFragmentVector = (fragment.transform.position - transform.position);
            var distance = posToFragmentVector.magnitude;
            var dir      = posToFragmentVector.normalized;
            if (distance < BlastRadius)
                fragment.rigidbody.AddForce(dir * BlastForce * (1 - (distance / BlastRadius)), ForceMode.Impulse);
        }
        */
        DeTrigger();
        _hasDetonated = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        /*
        List<GameObject> objsEffected = Utils.getAllObjectsWithinDistanceOf(transform.parent.parent, "Cube", BlastRadius);
        foreach (GameObject obj in objsEffected)
        {
            Debug.Log("Here");
            if (obj.rigidbody != null)
            {
                Vector3 pos = (obj.transform.position - transform.position);
                obj.rigidbody.AddForce(pos.normalized * ExplosionForce * (1 - (pos.magnitude / BlastRadius)), ForceMode.Impulse);
                
            }
        }
        */
        /*
        if (collision.gameObject.rigidbody != null)
        {
            foreach (ContactPoint point in collision.contacts)
            {
                collision.gameObject.rigidbody.AddExplosionForce(ExplosionForce, point.point, BlastRadius);
                
            }
        }
         * */
    }

    void Trigger()
    {
        IsTriggered = true;
        //Detonate();
        DeTrigger();
    }

    void DeTrigger()
    {
        IsTriggered = false;
    }
}
