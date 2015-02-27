using UnityEngine;
using System.Collections;

public class ButtonSelectionManager : MonoBehaviour {

    public int MyIndex;

	void Start () {
	
	}
	
	public void OnClick () 
    {
        SelectionManager.Instance.SetSelection(MyIndex);
	}
}
