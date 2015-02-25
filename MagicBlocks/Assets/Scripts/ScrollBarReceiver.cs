using UnityEngine;
using System.Collections;

public class ScrollBarReceiver : MonoBehaviour {

    public static ScrollBarReceiver Instance;
	// Use this for initialization
	void Awake () {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnSliderChange()
    {
        Debug.Log("SliderChanged");
    }
}
