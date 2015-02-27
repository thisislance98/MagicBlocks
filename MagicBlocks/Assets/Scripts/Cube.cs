using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cube : MonoBehaviour {

    Transform initializeAnchor;

	public List<GameObject> _connectedCubes = new List<GameObject>();
	static List<Transform> _myCubes = new List<Transform>();

    public Material OnTriggerMaterial;
    private Material _nonTriggerMaterial;

    public List<Transform> MyAnchors = new List<Transform>();

	Vector3 _startPos;
    Vector3 _lasPos;
	bool _dying;
	Ray _touchRay;

    private bool _isMoving = false;

    bool _isTriggered;
    public bool IsTriggered
    {
        get { return _isTriggered; }

        set
        {
            _isTriggered = value;
        }

    }

    void Awake()
    {
        _lasPos = transform.position;
    }

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
			
		}

		transform.rotation = rotation;
		transform.position = pos;

		_startPos = pos;

        _nonTriggerMaterial = renderer.material;
	}

    void Update()
    {

        _isMoving = transform.position != _lasPos;


        _lasPos = transform.position;

        if (!IsChildTriggered() && IsTriggered) { DeTrigger(); }
        else if (IsChildTriggered() && !IsTriggered) { OnTrigger(); }
    }

    void OnTap()
    {
        if (SelectionManager.Instance.GetSelection().tag != "Selector")
            return;

        RaycastHit hit;
        Ray ray;

        if (Utils.TouchCast(out hit, out ray))
        {
            if (hit.transform == transform)
            {
                OnTrigger();
            }
        }
    }

    void OnTrigger()
    {
        IsTriggered = true;
        gameObject.renderer.material = OnTriggerMaterial;
        Transform child, subChild;

        for (int i = 0; i < transform.childCount; i++)
        {
            child = transform.GetChild(i);//this child is an anchor
            //child.SendMessage("Trigger", SendMessageOptions.DontRequireReceiver);
            for (int j = 0; j < child.childCount; j++)
            {
                subChild = child.GetChild(j);//this child is the cubemodifier
                subChild.SendMessage("Trigger", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    public void DeTrigger()
    {
        IsTriggered = false;
        Transform child, subChild;

        for (int i = 0; i < transform.childCount; i++)
        {
            child = transform.GetChild(i);
            //transform.GetChild(i).SendMessage("DeTrigger", SendMessageOptions.DontRequireReceiver);
            for (int j = 0; j < child.childCount; j++)
            {
                subChild = child.GetChild(j);
                subChild.SendMessage("DeTrigger", SendMessageOptions.DontRequireReceiver);
            }
        }
        renderer.material = _nonTriggerMaterial;
        Vector3 pos = transform.position;
        pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
        transform.position = pos;
        
    }

    private bool IsChildTriggered()
    {
        Transform child, subChild;

        for (int i = 0; i < transform.childCount; i++)
        {
            child = transform.GetChild(i);
            //transform.GetChild(i).SendMessage("DeTrigger", SendMessageOptions.DontRequireReceiver);
            for (int j = 0; j < child.childCount; j++)
            {
                subChild = child.GetChild(j);
                if (subChild.gameObject.GetComponent<Modifier>() != null &&
                    subChild.gameObject.GetComponent<Modifier>().IsTriggered) { return true; }
            }
        }
        return false;
    }

    private bool HasModifier()
    {
        Transform child, subChild;

        for (int i = 0; i < transform.childCount; i++)
        {
            child = transform.GetChild(i);
            //transform.GetChild(i).SendMessage("DeTrigger", SendMessageOptions.DontRequireReceiver);
            for (int j = 0; j < child.childCount; j++)
            {
                subChild = child.GetChild(j);
                if (subChild.gameObject.GetComponent<Modifier>() != null) { return true; }
            }
        }
        return false;
    }

    public bool AnchorContainsModifier(Transform anchor, string modifier_name)
    {
        if (anchor == null) { return false; }
        for (int i = 0; i < anchor.childCount; i++)
        {
            Modifier modifier = anchor.GetChild(i).GetComponent<Modifier>();
            if (modifier != null && modifier.name == modifier_name)
            {
                return true;
            }
        }
        return false;
    }

    //removes the modifier from the anchor if it exists
    public void RemoveModifierFromAnchor(Transform anchor, string modifier_name)
    {
        for (int i = 0; i < anchor.childCount; i++)
        {
            Modifier modifier = anchor.GetChild(i).GetComponent<Modifier>();
            if (modifier != null && modifier.name == modifier_name)
            {
                modifier.transform.parent = null;//deparent from anchor
                DestroyImmediate(modifier.gameObject);
            }
        }
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

    //returns the anchor of the cube that is closest to the RaycastHit
    public Transform getClosestAnchor(RaycastHit hit)
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

        return closestAnchor;
    }


    //parents the Transform to the anchor nearest the Ray
    public void attachToClosestAnchor(GameObject modifier, RaycastHit hit)
    {
        Transform closestAnchor = getClosestAnchor(hit);

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

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Cube")
             return;
        if (IsTriggered)
            DeTrigger();
        else if(HasModifier())
        {
            OnTrigger();
        }
    }

    private bool IsMoving()
    {
        return _isMoving;
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
