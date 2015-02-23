using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour {

    public static SelectionManager Instance;
    private GameObject _selection;
    public GameObject[] SelectionPrefabs;


	// Use this for initialization
	void Awake () {
        Instance = this;
        _selection = SelectionPrefabs[1];
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _selection = SelectionPrefabs[0];
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _selection = SelectionPrefabs[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _selection = SelectionPrefabs[2];
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _selection = SelectionPrefabs[3];
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _selection = SelectionPrefabs[4];
        }
	}

    public GameObject GetSelection() { return _selection; }

    public void SetSelection(GameObject selection)
    {
        _selection = selection;
    }
}
