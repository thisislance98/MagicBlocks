using UnityEngine;
using System.Collections;

public class Thruster : Modifier {

    public ParticleSystem Particles;
    public float Speed;

	// Use this for initialization
	void Awake () {
        CanHaveMultiplePerCube = false;
	}

    void Update()
    {
        if(transform.parent != null && transform.parent.parent != null && !transform.parent.parent.GetComponent<Cube>().IsTriggered)
            return;

        Debug.Log("1");
         _cube.position += -transform.forward * Speed * Time.deltaTime;
    }
}
