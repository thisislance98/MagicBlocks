using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform[] PlayerPositionObs;
	public float PitchSpeed;
	public float MoveSpeed;
	public float RotateSpeed;
	public float DeviceMoveSpeed;
	public float DevicePanSpeed;
	public float DeviceRotateSpeed;
	public float FollowTargetSpeed;
	public Transform SecondPositionObj;

	bool _isAtSecondPos;
	Transform _target = null;
	bool _isFollowingTarget;
	Vector3 _targetPos;
	int _teamNum;
	Vector3 _cameraStartPos;
	Quaternion _cameraStartRotation;
	Quaternion _controllerStartRotation;

	public static CameraController Instance;

	void Awake()
	{
		Instance = this;
	

	}

	public void Initialize(int teamNumber)
	{
		_teamNum = teamNumber;
		Transform posObj = PlayerPositionObs[teamNumber];

		Debug.Log("team number:" + _teamNum);

		transform.position = posObj.position;
		transform.rotation = posObj.rotation * CameraController.Instance.transform.rotation;

		_controllerStartRotation = transform.rotation;
		_cameraStartPos = Camera.main.transform.position;
		_cameraStartRotation = Camera.main.transform.rotation;
	}

	public void MoveToStartPosition()
	{
		Transform posObj = PlayerPositionObs[_teamNum];

		Debug.Log("moving to pos for team: " + _teamNum);

		float animTime = 2;
		
		LeanTween.move(gameObject, posObj.position, animTime).setEase(LeanTweenType.easeInCubic);
		LeanTween.rotate(gameObject,_controllerStartRotation.eulerAngles,animTime).setEase(LeanTweenType.easeInCubic);
		
		LeanTween.move(Camera.main.gameObject, _cameraStartPos, animTime).setEase(LeanTweenType.easeInCubic);
		LeanTween.rotate(Camera.main.gameObject,_cameraStartRotation.eulerAngles,animTime).setEase(LeanTweenType.easeInCubic);

	}

	void OnRestart()
	{
		StopFollowingTarget();
		MoveToStartPosition();

	}

	public void FollowTarget(Transform target)
	{
		_isFollowingTarget = true;
		_target = target;

	}

	public void StopFollowingTarget()
	{
		_isFollowingTarget = false;
	}

	// Update is called once per frame
	void Update () {

		float pitch = 0;
		float horizontalMotion = 0;

//		if (Input.GetKeyUp(KeyCode.F) || (Input.touchCount == 1 && Input.GetTouch(0).tapCount == 2))
//			SwitchPosition();

		if (Input.GetKey(KeyCode.E))
			pitch = 1;
		else if (Input.GetKey(KeyCode.Q))
			pitch = -1;

		if (Input.GetKey(KeyCode.C))
			horizontalMotion = 1;
		else if (Input.GetKey(KeyCode.Z))
			horizontalMotion = -1;
		
        RaycastHit hit;
        Ray ray;

        if (Utils.TouchCast(out hit, out ray))
        {
            //if (hit.transform.tag == "GUI") { return; }//if there is a collision with a GUI element do not update the camera position
        }

		ChangePitch(pitch * PitchSpeed * Time.deltaTime);		        

		Rotate(-Input.GetAxis("Horizontal") * RotateSpeed * Time.deltaTime);

		MoveForward(Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime);

		transform.position += Camera.main.transform.right * horizontalMotion * MoveSpeed * Time.deltaTime;

		if (_isFollowingTarget)
		{
			_targetPos = (_target == null) ? _targetPos : _target.position;

			Quaternion lookRot = Quaternion.LookRotation(_targetPos - Camera.main.transform.position);

			Camera.main.transform.rotation = lookRot;//Quaternion.Slerp(Camera.main.transform.rotation, lookRot, Time.deltaTime * FollowTargetSpeed);

		}
	}

	public void RotateTo(Vector3 rot, float time)
	{
		if (CubeManager.Instance.IsRestarting())
			return;

		LeanTween.rotate(Camera.main.gameObject,rot,time);
	}

	public void MoveTo(Vector3 pos, float time)
	{
		if (CubeManager.Instance.IsRestarting())
			return;

		LeanTween.move(Camera.main.gameObject,pos,time);

	}
	
	public void LookAt(Vector3 target, float time)
	{
		if (CubeManager.Instance.IsRestarting())
			return;

		Vector3 lookDir = Quaternion.LookRotation(target - Camera.main.transform.position).eulerAngles;

		LeanTween.rotate(Camera.main.gameObject, lookDir, time);
	}

	public void ChangeFOV(float newFOV, float time)
	{
		if (CubeManager.Instance.IsRestarting())
			return;

		LeanTween.value(gameObject,UpdateFOV,Camera.main.fieldOfView,newFOV,time);

	}


	void UpdateFOV(float newFOV)
	{
		Camera.main.fieldOfView = newFOV;

	}



	public void SwitchPosition()
	{
		if (_isAtSecondPos)
			transform.position = Vector3.zero;
		else
			transform.position = SecondPositionObj.position;

		_isAtSecondPos = !_isAtSecondPos;

	}

	bool IsControllingCamera()
	{
		return Camera.main.transform.parent == transform;

	}

	public void SetSecondPositionObj(Transform secondPosObj)
	{

		SecondPositionObj = secondPosObj;
	}

	void OnCameraDrag(Vector2 delta)
	{
		if (IsControllingCamera() == false || _isFollowingTarget)
			return;
		Rotate(delta.x * DeviceRotateSpeed);
		ChangePitch(delta.y * -DeviceRotateSpeed);

	}

    private bool isTouchUILayer()
    {
        Ray ray;
        RaycastHit hit;
        bool end;
        end = Utils.LayerTouchCast(out hit, out ray, 5);
        return end;
    }

	void OnTwoFingerDrag()
	{
		if (InputManager.Instance.IsInteractingWithObject() || IsControllingCamera() == false|| _isFollowingTarget)
			return;


		// zoom in and out
		Vector3 delta = (Input.GetTouch(1).position - Input.GetTouch(0).position).normalized;
		
		float touchOneDelta = Vector2.Dot( Input.GetTouch(1).deltaPosition , delta );
		float touchZeroDelta = Vector2.Dot( Input.GetTouch(0).deltaPosition , -delta );
		
		float total = (touchOneDelta + touchZeroDelta) * DeviceMoveSpeed;
		
		MoveForward (total);
		
		Vector2 averageDelta = ( Input.GetTouch(0).deltaPosition + Input.GetTouch(1).deltaPosition ) / 2.0f;
		
		transform.position -= ((averageDelta.x * Camera.main.transform.right) + (averageDelta.y * Camera.main.transform.up)) * DevicePanSpeed * GetCameraDistance();
		
	}

	public void UpdatePosition(Vector3 pos)
	{
		LeanTween.move(gameObject,pos,1);

	}

	public float GetCameraDistance()
	{
		return Vector3.Distance(transform.position, Camera.main.transform.position);
	}
	
	void ChangePitch(float angle)
	{
		transform.RotateAround(transform.position,transform.right,angle);

		if (Camera.main.transform.position.y < 0)
			transform.RotateAround(transform.position,transform.right,-angle);

	}

	void Rotate(float angle)
	{
		transform.RotateAround(transform.position,Vector3.up,angle);
	}

	void MoveForward(float delta)
	{
		Camera.main.transform.position += Camera.main.transform.forward * delta;

	}
}
