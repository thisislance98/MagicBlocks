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
        if (transform.parent != null && transform.parent.parent != null && !transform.parent.parent.GetComponent<Cube>().IsTriggered)
        {
            DeTrigger();
            return;
        }

        if (!IsTriggered)//only trigger if not already triggered (do no re-trigger on each update)
        {
            Trigger();
        }
         _cube.position += -transform.forward * Speed * Time.deltaTime;
    }

    void Trigger()
    {
        IsTriggered = true;
        Particles.Play();
    }

    void DeTrigger()
    {
        IsTriggered = false;
        Particles.Stop();
    }
}
