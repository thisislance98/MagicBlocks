using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cube : MonoBehaviour {

    Transform initializeAnchor;

	public List<GameObject> _connectedCubes = new List<GameObject>();
	static List<Transform> _myCubes = new List<Transform>();

    public List<Transform> MyAnchors = new List<Transform>();

	Vector3 _startPos;
	bool _dying;
	Ray _touchRay;
	

	public void Initialize(RaycastHit hit)
	{
		Transform hitTransform = hit.transform;

		Quaternion rotation = Quaternion.identity;
		Vector3 pos = hit.point + hit.normal * .1f;
		pos = new Vector3(Mathf.Round(pos.x),Mathf.Round(pos.y),Mathf.Round(pos.z));

		if (hitTransform.tag == "Cube")
		{
			pos = hitTransform.position + hit.normal ;
			rotation = hitTransform.rotation;

			transform.rotation = rotation;
			transform.position = pos;
			
		
			ConnectToCube(hitTransform.rigidbody);
			
		}

		transform.rotation = rotation;
		transform.position = pos;

		_startPos = pos;
	}

    //removes all modifiers of one type
    public void RemoveAllModifiersOfType(string name)
    {
        foreach (Transform anchor in MyAnchors)
        {
            for (int i = anchor.childCount-1; i>= 0; i--)
            {
                if (anchor.GetChild(i).name == name)
                {
                    Destroy(anchor.GetChild(i).gameObject);
                }
            }
        }
    }


    //parents the Transform to the anchor nearest the Ray
    public void attachToClosestAnchor(GameObject modifier, RaycastHit hit)
    {
        
        //find anchor nearest to ray
        float minDist = float.MaxValue;
        Transform closestAnchor = null;

        foreach (Transform anchor in MyAnchors)
        {
            if (null == closestAnchor) { closestAnchor = anchor; }

            float distance = Vector3.Distance(hit.point, anchor.position);
            if (distance < minDist)
            {
                closestAnchor = anchor;
                minDist = distance;
            }
        }

        if (closestAnchor.childCount > 0)
            DestroyImmediate(closestAnchor.GetChild(0).gameObject);

        modifier.transform.forward = closestAnchor.forward;
        modifier.transform.parent = closestAnchor;
        modifier.transform.position = closestAnchor.position;
    }


    public void SetTouchRay(Ray ray)
	{
		_touchRay = ray;
	}

	public Ray GetTouchRay()
	{

		return _touchRay;
	}
	



	public static Vector3 GetBaryCenterOfCubes()
	{
		Vector3 center = Vector3.zero;

		foreach (Transform cube in _myCubes)
		{
			center += cube.position;
		}

		if (_myCubes.Count > 0)
			center /= _myCubes.Count;

		return center;

	}

	public void OnCollisionEnter(Collision other)
	{

		if (other.transform.tag == "Cube" && rigidbody != null && rigidbody.velocity.magnitude < 1)
			ConnectToCube(other.rigidbody);

//		float upAmount = Vector3.Dot(Vector3.up,transform.up);
//		
//		if (upAmount < .3f)
//			Destroy(gameObject);

	}
	

	public void ConnectToCube(Rigidbody cubeBody)
	{

		if (_connectedCubes.Contains(cubeBody.gameObject) || cubeBody.transform.tag != "Cube")
			return;
		
		FixedJoint joint = transform.gameObject.AddComponent<FixedJoint>();
		joint.connectedBody = cubeBody;
			
		joint.breakForce = .1f;
		_connectedCubes.Add(cubeBody.gameObject);


	}


	void Die()
	{

		Destroy(gameObject);
	}
	

	void OnDestroy()
	{
		CubeManager.Instance.OnCubeDestroyed(gameObject);

		if (_myCubes.Contains(transform))
		{
			_myCubes.Remove(transform);

			if (_myCubes.Count > 0 && CubeManager.Instance.IsRestarting() == false )
				CameraController.Instance.UpdatePosition(GetBaryCenterOfCubes());
		}

	}

	public static List<Transform> GetMyCubes()
	{
		return _myCubes;
	}



}
