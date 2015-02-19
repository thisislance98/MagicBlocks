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
        if (SelectionManager.Instance.GetSelection().tag != "Cube")
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

		obj.SendMessage("Initialize",hit);

		obj.GetComponent<Cube>().SetTouchRay(ray);


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
				Destroy(hit.transform.gameObject);
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
