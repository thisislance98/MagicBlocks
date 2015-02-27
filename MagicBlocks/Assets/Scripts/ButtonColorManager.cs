using UnityEngine;
using System.Collections;

public class ButtonColorManager : MonoBehaviour {

    public int MyIndex;

	void Awake () {
	
	}

    public void OnClick()
    {
        CubeColorManager.Instance.SetColor(MyIndex);
    }
}
