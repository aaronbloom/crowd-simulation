using RTS_Cam;
using UnityEngine;

namespace Assets.Scripts.Camera {
    public class CameraManager {

        private const string FirstPersonCameraName = "FirstPersonCharacterParent";
        private const string RTSCameraName = "RTS_Camera";
        private const string StatsCameraName = "StatsCamera";

        public GameObject RTSCamera { get; private set; }
        public RTS_Camera RTSCameraScript { get; private set; }
        public GameObject StatsCamera { get; private set; }
        public GameObject FirstPersonCamera { get; private set; }

        public CameraManager() {
            FirstPersonCamera = (GameObject)BootStrapper.Initialise(FirstPersonCameraName);
            StatsCamera = (GameObject)BootStrapper.Initialise(StatsCameraName);
            //RTS camera not initialised here
        }

        public void SwitchToStatsCamera(Vector3 position, Vector3 direction) {
            deactivateFirstPersonCamera();
            deactivateRTSCamera();
            StatsCamera.GetComponent<UnityEngine.Camera>().enabled = true;
            StatsCamera.GetComponent<UnityEngine.Camera>().transform.position = position + Vector3.back;
            StatsCamera.GetComponent<UnityEngine.Camera>().transform.LookAt(StatsCamera.GetComponent<UnityEngine.Camera>().transform.position + direction);
        }

        public void ActivateRTSCamera() {
            deactivateStatsCamera();
            deactivateFirstPersonCamera();
            if (RTSCamera != null) {
                deactivateRTSCamera();
            }
            RTSCamera = ((GameObject)BootStrapper.Initialise(RTSCameraName));
            RTSCameraScript = RTSCamera.GetComponent<RTS_Camera>();
        }

        public void ActivateFirstPersonCamera() {
            deactivateRTSCamera();
            deactivateStatsCamera();
            FirstPersonCamera.transform.position =
                BootStrapper.EnvironmentManager.CurrentEnvironment.World.Entrances[0].FrontPosition();
            FirstPersonCamera.SetActive(true);
        }

        private void deactivateStatsCamera() {
            StatsCamera.GetComponent<UnityEngine.Camera>().enabled = false;
        }

        private void deactivateFirstPersonCamera() {
            FirstPersonCamera.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void deactivateRTSCamera() {
            Object.Destroy(RTSCamera);
        }
    }
}
