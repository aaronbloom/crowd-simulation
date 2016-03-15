using System;
using UnityEngine;

namespace Assets.Scripts.Camera {
    public class CameraManager {

        private static readonly string EnvironmentCameraName = "EnvironmentCamera";
        private static readonly string FirstPersonCameraName = "FirstPersonCharacterParent";

        private static GameObject FirstPersonCamera;
        private static GameObject EnvironmentCamera;

        private Boolean isFirstPerson = false;

        public CameraManager () {
            FirstPersonCamera = ((GameObject)BootStrapper.Initialise(FirstPersonCameraName));
            EnvironmentCamera = ((GameObject)BootStrapper.Initialise(EnvironmentCameraName));
        }

        public void activateFirstPersonCamera() {
            FirstPersonCamera.transform.position = Vector3.zero;
            EnvironmentCamera.SetActive(false);
            FirstPersonCamera.SetActive(true);
        }

        public void activateEnvironmentCamera() {
            FirstPersonCamera.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            EnvironmentCamera.SetActive(true);
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
    }
}
