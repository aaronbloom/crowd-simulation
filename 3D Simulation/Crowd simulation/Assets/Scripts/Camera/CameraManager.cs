using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class CameraManager : MonoBehaviour {

    private Camera[] CCTV;

    private static GameObject FirstPersonCamera;
    private static GameObject EnvironmentCamera;

    private Boolean isFirstPerson = false;


    void Awake() {
        FirstPersonCamera = GameObject.Find("FirstPersonCharacterParent");
        EnvironmentCamera = GameObject.Find("Camera(Clone)");
        CCTV = new Camera[0];
    }

	void Start () {
	    //activateEnvironmentCamera();
	}

	void Update () {
	}

    public void activateFirstPersonCamera() {
        GameObject.Find("Camera(Clone)").GetComponent<Camera>().enabled = false;
        FirstPersonCamera.SetActive(true);
    }

    public void activateEnvironmentCamera() {
        FirstPersonCamera.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameObject.Find("Camera(Clone)").GetComponent<Camera>().enabled = true;
    }

    public void toggleFirstPersonCamera() {
        if (isFirstPerson) {
            activateEnvironmentCamera();
            isFirstPerson = false;
        } else {
            activateFirstPersonCamera();
            isFirstPerson = true;
        }
    }

    public void activateCCTV(int index) {
        disableAllCameras();
        CCTV[index].enabled = true;
    }


    private void disableAllCameras() {
        foreach (Camera camera in CCTV) {
            camera.enabled = false;
        }
    }
}
