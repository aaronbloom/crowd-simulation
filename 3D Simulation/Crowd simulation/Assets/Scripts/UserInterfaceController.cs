using System;
using UnityEngine;
using UnityEngine.UI;
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
        ShowMenu(mainMenu);
        HideMenu(setupMenu);
        HideMenu(environmentBuilderMenu);
    }
	
	void Update () {
	    if (userWorldBuilder != null) {
            userWorldBuilder.UpdateCursorPosition();
            if (Input.GetMouseButtonDown(0)) { //left mouse clicked
                userWorldBuilder.PlaceWorldObject();
            }
	    }
	}

    public void NewSimulation() {
        HideMenu(mainMenu);
        ShowMenu(setupMenu);
    }

    public void StartEnvironmentBuilder() {
        HideMenu(setupMenu);
        ShowMenu(environmentBuilderMenu);
        userWorldBuilder = new UserWorldBuilder();
    }

    public void StartSimulation() {
        userWorldBuilder.Destroy();
        userWorldBuilder = null;
        HideMenu(mainMenu);
        HideMenu(setupMenu);
        HideMenu(environmentBuilderMenu);
        int numberOfBoids = setupMenu.GetComponent<SliderController>().Value;
        BootStrapper.StartSimulation(numberOfBoids);
    }

    public void SetCurrentPlacementObject(string objectName) {
        userWorldBuilder.SetCurrentPlacementObject(objectName);
    }

    private void HideMenu(GameObject menu) {
        menu.SetActive(false);
        CanvasGroup canvasGroup = menu.GetComponentInChildren<CanvasGroup>();
        if (canvasGroup != null) {
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void ShowMenu(GameObject menu) {
        menu.SetActive(true);
        CanvasGroup canvasGroup = menu.GetComponentInChildren<CanvasGroup>();
        if (canvasGroup != null) {
            canvasGroup.blocksRaycasts = true;
        }
    }
}
