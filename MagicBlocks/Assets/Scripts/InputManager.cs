using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public Transform SelectedItemPrefab;

	public float MaxTapTime = 2.0f;
	float _tapTime;
	float _totalTouchDelta;
	GameObject _touchObj;
	Vector3 _lastMousePos;
	Transform _defaultSelectedItem;

	public static InputManager Instance;

	void Awake()
	{
		Instance = this;
		_lastMousePos = Input.mousePosition;
		_defaultSelectedItem = SelectedItemPrefab;
	}

	public void OnItemSelected(GameObject itemPrefab)
	{
		Debug.Log("item selected: " + itemPrefab.name);
		SelectedItemPrefab = itemPrefab.transform;

	}

	public bool IsInteractingWithObject()
	{
		return _touchObj != null && _touchObj.tag == "Interactable";
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKey(KeyCode.R) || Input.touchCount >= 3)
			CubeManager.Instance.RestartGame();

		_tapTime += Time.deltaTime;

		// stuff for detecting taps
		if (Input.touchCount == 1 || Input.GetMouseButtonDown(0))
		{
			if (Input.GetMouseButtonDown(0) || Input.GetTouch(0).phase == TouchPhase.Began)
			{
				RaycastHit hit;
				
				if (Utils.TouchCast(out hit))
				{
					_touchObj = hit.transform.gameObject;
				}

				_tapTime = 0;
				_totalTouchDelta = 0;
			}
			else if (Input.GetTouch(0).phase == TouchPhase.Moved)
				_totalTouchDelta += Input.GetTouch(0).deltaPosition.magnitude;


		}	

		// handle one finger drag
		if (Input.touchCount == 1 && Input.GetTouch(0).deltaPosition.magnitude > 0 || (Application.isEditor && Input.GetMouseButton(0) && Input.mousePosition != _lastMousePos ))
		{
			Vector2 touchDelta;

			if (Input.touchCount == 1)
				touchDelta = Input.GetTouch(0).deltaPosition;
			else
			{
				Vector3 mouseDelta = Input.mousePosition - _lastMousePos;
				touchDelta = new Vector2(mouseDelta.x, mouseDelta.y);
			}
		
			Utils.SendMessageToAll("OnDrag",touchDelta);
		}

		if (Input.touchCount == 2 && (Input.GetTouch(0).deltaPosition.magnitude > 0 || Input.GetTouch(1).deltaPosition.magnitude > 0))
		{
			Utils.SendMessageToAll("OnTwoFingerDrag");

		}

		// touch and hold to destroy cube
		if ((Input.GetMouseButtonDown(1) && Application.isEditor) || (Input.touchCount == 1 && _totalTouchDelta < 10 && _tapTime >= .5f) )
		{
			Utils.SendMessageToAll("OnFingerHold");

			_totalTouchDelta = float.MaxValue;
		}
		else if (DidTap() ) //&& UICamera.isOverUI == false) // got tap so create cube
		{

			Utils.SendMessageToAll("OnTap");
		}

		// handle touch ended
		if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && ( Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)))
		{
			Utils.SendMessageToAll("OnTouchEnded");
			_touchObj = null;
		}

	
		_lastMousePos = Input.mousePosition;
	}

	public GameObject GetTouchObj()
	{
		return _touchObj;
	}


	bool DidTap()
	{
		if (Input.touchCount == 1)
		{
		//	Debug.Log("tap time: " + _tapTime + " phase: " + Input.GetTouch(0).phase + " tap count: " + Input.GetTouch(0).tapCount);
			return (Input.GetTouch(0).phase == TouchPhase.Ended && _tapTime < MaxTapTime && Input.GetTouch(0).tapCount == 1);
		}
		else
			return Input.GetMouseButtonUp(0) && _tapTime < MaxTapTime;

	}
		    

	public void OnRestart()
	{
		SelectedItemPrefab = _defaultSelectedItem;

	}
}
