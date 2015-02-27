using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class CubeManager : MonoBehaviour {
	
	public GameObject CubePrefab;
	public GameObject ExplosionPrefab;
	public List<GameObject> _undoCubes = new List<GameObject>();

	public static CubeManager Instance;
	int _teamNumber;
	bool _isRestarting;

	

	void Awake()
	{
		Instance = this;

	}

	

	void OnTap()
	{
        if (SelectionManager.Instance.GetSelection().tag != "Cube" || UICamera.isOverUI)
            return;

		RaycastHit hit;
		Ray ray;

		if (Utils.TouchCast(out hit, out ray))
		{

			AddCube(hit,ray);
		}
		
	}



	public void AddCube(RaycastHit hit, Ray ray)
	{

		GameObject obj = (GameObject)Instantiate(CubePrefab, new Vector3(100,0,100), Quaternion.identity);

        obj.renderer.material = GameObject.Find("CubeColorManager").GetComponent<CubeColorManager>().GetColor();

		obj.SendMessage("Initialize",hit);

		obj.GetComponent<Cube>().SetTouchRay(ray);

        if (hit.transform.tag == "Cube")
        {
            obj.transform.parent = hit.transform;
            Modifier m;
            Transform anchor = hit.transform.GetComponent<Cube>().getClosestAnchor(hit);//get nearest anchor
            for (int j = 0; j < anchor.childCount; j++)
            {
                Transform modifier = anchor.GetChild(j);
                if ((m = modifier.GetComponent<Modifier>()) != null && m.CanParentCubes)// && (subChild.name != "Rotator(Clone)" && subChild.childCount == 1))
                {
                    obj.transform.parent = m.transform;
                }
            }
            
        }
        else
        {
            //Debug.Log("Not parented");
        }

		_undoCubes.Add(obj);
		Cube.GetMyCubes().Add(obj.transform);
		CameraController.Instance.UpdatePosition(Cube.GetBaryCenterOfCubes());

	}


	public void OnCubeDestroyed(GameObject cube)
	{
		if (_undoCubes.Contains(cube))
		{
			_undoCubes.Remove(cube);
		}

	}


	public void OnUndoTouch()
	{
		if (_undoCubes.Count > 0)
		{
	//		_undoCubes[_undoCubes.Count-1].GetComponent<PhotonView>().RPC("Die",PhotonTargets.AllBuffered);

			_undoCubes.RemoveAt( _undoCubes.Count-1 );

		}

	}

	void OnFingerHold()
	{


		RaycastHit hit;

		if (Utils.TouchCast(out hit))
		{
            if (hit.transform.tag == "Cube")
            {
                //Deparent all children first
                for (int i = 0; i < hit.transform.childCount; i++)
                {
                    if(hit.transform.GetChild(i).tag == "Cube")
                        hit.transform.GetChild(i).parent = null;
                }
                Destroy(hit.transform.gameObject);
            }
		}
		
	}


	public void RestartGame()
	{
		Cleanup();

	}

	public bool IsRestarting()
	{
		return _isRestarting;
	}

	void Cleanup()
	{
		GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");

		foreach(GameObject cube in cubes)
			DestroyImmediate(cube);



	}




}
