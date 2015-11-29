using UnityEngine;
using System.Collections;

public class UserInterfaceController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void HideInterface() {
        this.gameObject.SetActive(false);
    }

    public void ShowInterface() {
        this.gameObject.SetActive(true);
    }
}
