using UnityEngine;
using System.Collections;

public class ButtonSelectionManager : MonoBehaviour {

    public int MyIndex;

	void Start () {
	
	}
	
	public void OnClick () 
    {
        SelectionManager s = GameObject.Find("SelectionManager").GetComponent<SelectionManager>();
        if (s != null)
        {
            s.SetSelection(MyIndex);
            Debug.Log("Set selection index: " + MyIndex);
        }
	}
}
