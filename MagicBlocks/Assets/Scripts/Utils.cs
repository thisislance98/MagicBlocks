using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utils : MonoBehaviour {


	public static void SendMessageToAll(string message)
	{
		
		GameObject[] allObjs = GameObject.FindObjectsOfType<GameObject>();
		
		for (int i=allObjs.Length-1; i >= 0; i--)
		{
			allObjs[i].SendMessage(message,SendMessageOptions.DontRequireReceiver);
			
		}
		
	}

	public static void SendMessageToAll(string message, object param)
	{

		GameObject[] allObjs = GameObject.FindObjectsOfType<GameObject>();

		for (int i=allObjs.Length-1; i >= 0; i--)
		{
			allObjs[i].SendMessage(message,param,SendMessageOptions.DontRequireReceiver);

		}
	}

	public static bool TouchCast(out RaycastHit hit)
	{
		
		return Physics.Raycast(GetTouchRay(), out hit,1000);

	}

	public static bool TouchCast(out RaycastHit hit, out Ray ray)
	{
		ray = GetTouchRay();
		return Physics.Raycast(ray, out hit,1000);

	}

	public static RaycastHit GetHit(Ray ray)
	{
		RaycastHit hit;
		Physics.Raycast(ray, out hit,1000);
		return hit;
	}

	public static Ray GetTouchRay()
	{
		Vector3 touchPos = Input.mousePosition;
		
		if (Input.touchCount == 1)
			touchPos = Input.GetTouch(0).position;
		
		return Camera.main.ScreenPointToRay(touchPos);

	}

	public static string Vector3ToString(Vector3 v)
	{
		return "" + v.x + "," + v.y + "," + v.z;
	}

	
	public static Vector3 StringToVector3(string str)
	{
		string[] numbers = str.Split(new char[] {','}, System.StringSplitOptions.None);
		List<float> floats = new List<float>();
		
		foreach (string number in numbers)
		{
			floats.Add(float.Parse(number));
			
		}
		
		return new Vector3(floats[0],floats[1],floats[2]);
	}

	public static string RayToString(Ray ray)
	{
		return "" + ray.origin.x + "," + ray.origin.y + "," + ray.origin.z + "," + ray.direction.x + "," + ray.direction.y + "," + ray.direction.z;

	}

	public static Ray StringToRay(string str)
	{
		string[] numbers = str.Split(new char[] {','}, System.StringSplitOptions.None);
		List<float> floats = new List<float>();

		foreach (string number in numbers)
		{
			floats.Add(float.Parse(number));

		}

		return new Ray(new Vector3(floats[0],floats[1],floats[2]), new Vector3(floats[3],floats[4],floats[5]));
	}


}
