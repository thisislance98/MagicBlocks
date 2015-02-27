﻿using UnityEngine;
using System.Collections;

public class ButtonColorManager : MonoBehaviour {

    public int MyIndex;

	void Awake () {
	
	}

    void OnClick()
    {
        CubeColorManager c = GameObject.Find("CubeColorManager").GetComponent<CubeColorManager>();
        if (c != null)
        {
            c.SetColor(MyIndex);
        }
    }
}
