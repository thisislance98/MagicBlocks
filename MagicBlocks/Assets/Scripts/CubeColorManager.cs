using UnityEngine;
using System.Collections;

public class CubeColorManager : MonoBehaviour 
{
    public Material[] CubeColors;
    private Material _currentColor;
    public int ColorIndex = 0;

    public static CubeColorManager Instance;
	// Use this for initialization
	void Awake () {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {

        if (_currentColor == null && CubeColors.Length > 0) { _currentColor = CubeColors[0]; }
        //handle color switch
        if (Application.isEditor && Input.GetKeyDown(KeyCode.Equals))
        {
            NextColor();
        }
        else if (Application.isEditor && Input.GetKeyDown(KeyCode.Minus))
        {
            PreviousColor();
        }
	}

    public void SetColor(int colorIndex)
    {
        if (colorIndex >= 0 && colorIndex < CubeColors.Length)
        {
            _currentColor = CubeColors[colorIndex];
        }
    }

    public void SetColor()
    {
        if (ColorIndex >= 0 && ColorIndex < CubeColors.Length)
        {
            _currentColor = CubeColors[ColorIndex];
        }
    }

    public Material GetColor() 
    {
        return _currentColor; 
    }

    void NextColor()
    {
        //find where _currentColor is currently
        int index = 0;
        for (; index < CubeColors.Length && _currentColor != CubeColors[index]; index++) ; 
        //move to next
        _currentColor = CubeColors[(index + 1) % CubeColors.Length];
    }

    void PreviousColor()
    {
        //find where _currentColor is currently
        int index = 0;
        for (; index < CubeColors.Length && _currentColor != CubeColors[index]; index++) ; 
        //move to previous
        if (index - 1 > 0)
        {
            _currentColor = CubeColors[(index - 1)];
        }
        else
        {
            _currentColor = CubeColors[CubeColors.Length - 1];
        }

    }
}
