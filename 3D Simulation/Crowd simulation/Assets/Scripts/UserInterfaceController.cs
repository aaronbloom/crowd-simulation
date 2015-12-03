using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class UserInterfaceController : MonoBehaviour {

    private GameObject mainMenu;
    private GameObject setupMenu;
    private GameObject environmentBuilderMenu;
    private UserWorldBuilder userWorldBuilder;

    void Awake() {
        mainMenu = GameObject.Find("MainMenu");
        setupMenu = GameObject.Find("SetupMenu");
        environmentBuilderMenu = GameObject.Find("EnvironmentBuilderMenu");
    }

	void Start () {
	    mainMenu.SetActive(true);
        setupMenu.SetActive(false);
        environmentBuilderMenu.SetActive(false);
    }
	
	void Update () {
	    if (userWorldBuilder != null) {
	        userWorldBuilder.Update();
	    }
	}

    public void NewSimulation() {
        mainMenu.SetActive(false);
        setupMenu.SetActive(true);
    }

    public void StartEnvironmentBuilder() {
        setupMenu.SetActive(false);
        environmentBuilderMenu.SetActive(true);
        userWorldBuilder = new UserWorldBuilder();
    }

    public void StartSimulation() {
        userWorldBuilder.Destroy();
        userWorldBuilder = null;
        mainMenu.SetActive(false);
        setupMenu.SetActive(false);
        environmentBuilderMenu.SetActive(false);
        int numberOfBoids = setupMenu.GetComponent<SliderController>().Value;
        BootStrapper.StartSimulation(numberOfBoids);
    }

    public void SetCurrentPlacementObject(string objectName) {
        userWorldBuilder.SetCurrentPlacementObject(objectName);
    }
}
