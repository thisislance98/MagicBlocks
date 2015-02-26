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
        if (collider.transform != transform.parent.parent && collider.transform.name != "Plane" && collider.transform.tag != "GUI")
        {
            //Debug.Log(collider.transform.tag +":"+collider.transform.name);//shows what caused the detonation
            Cube cube = collider.transform.GetComponent<Cube>();
            if (cube != null) { cube.DeTrigger(); }
            Detonate();
        }
    }

    private void Detonate()
    {
        Debug.Log("boom");
        if (_hasDetonated) { return; }//can only detonate once

        List<GameObject> objsEffected = Utils.getAllObjectsWithinDistanceOf(transform.parent.parent, "Cube", BlastRadius);
        foreach (GameObject obj in objsEffected)
        {
            if (obj.rigidbody != null)
            {
                Vector3 pos = (obj.transform.position - transform.position);
                obj.rigidbody.AddExplosionForce(ExplosionForce, transform.parent.parent.position, BlastRadius);

            }
        }


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
        _hasDetonated = true;
        Destroy(transform.parent.parent.gameObject);//the cube with the detonator is destroyed
    }

    void OnCollisionEnter(Collision collision)
    {
        //Trigger();
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
