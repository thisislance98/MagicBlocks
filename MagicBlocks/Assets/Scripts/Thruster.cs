using UnityEngine;
using System.Collections;

public class Thruster : Modifier {

    public ParticleSystem Particles;
    public float Speed;
    private Transform _cube = null;
    private bool _isThrusting = false;

	// Use this for initialization
	void Awake () {
        CanHaveMultiplePerCube = false;

	}

    void Update()
    {
        if (_cube == null && transform.parent != null && transform.parent.parent != null) { _cube = transform.parent.parent; }
        if(!_isThrusting)
            return;

         _cube.position += -transform.forward * Speed * Time.deltaTime;
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
                _isThrusting = true;
                Particles.Play();
            }
        }
    }
}
