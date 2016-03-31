using RTS_Cam;
using UnityEngine;

namespace Assets.Scripts.Camera {
    /// <summary>
    /// Controls the switching between active camera viewpoints
    /// </summary>
    public class CameraManager {

        private const string FirstPersonCameraName = "FirstPersonCharacterParent";
        private const string RTSCameraName = "RTS_Camera";
        private const string StatsCameraName = "StatsCamera";

        public GameObject RTSCamera { get; private set; }
        public RTS_Camera RTSCameraScript { get; private set; }
        public GameObject StatsCamera { get; private set; }
        public GameObject FirstPersonCamera { get; private set; }

        /// <summary>
        /// Default constructor to initialise cameras
        /// </summary>
        public CameraManager() {
            FirstPersonCamera = (GameObject)BootStrapper.Initialise(FirstPersonCameraName);
            StatsCamera = (GameObject)BootStrapper.Initialise(StatsCameraName);
            //RTS camera not initialised here
        }

        /// <summary>
        /// Activate the camera for viewing statistics to a given position and rotation
        /// </summary>
        /// <param name="position">position the camera should be located at</param>
        /// <param name="direction">direction in which the camera should be facing</param>
        public void SwitchToStatsCamera(Vector3 position, Vector3 direction) {
            deactivateFirstPersonCamera();
            deactivateRTSCamera();
            StatsCamera.GetComponent<UnityEngine.Camera>().enabled = true;
            StatsCamera.GetComponent<UnityEngine.Camera>().transform.position = position + Vector3.back;
            StatsCamera.GetComponent<UnityEngine.Camera>().transform.LookAt(StatsCamera.GetComponent<UnityEngine.Camera>().transform.position + direction);
        }

        /// <summary>
        /// Activate the 'default' camera for viewing the simulation
        /// </summary>
        public void ActivateRTSCamera() {
            deactivateStatsCamera();
            deactivateFirstPersonCamera();
            if (RTSCamera != null) {
                deactivateRTSCamera();
            }
            RTSCamera = ((GameObject)BootStrapper.Initialise(RTSCameraName));
            RTSCameraScript = RTSCamera.GetComponent<RTS_Camera>();
        }

        /// <summary>
        /// Activate the first person camera for exploring the simulation
        /// </summary>
        public void ActivateFirstPersonCamera() {
            deactivateRTSCamera();
            deactivateStatsCamera();
            FirstPersonCamera.transform.position =
                BootStrapper.EnvironmentManager.CurrentEnvironment.World.Entrances[0].FrontPosition();
            FirstPersonCamera.SetActive(true);
        }

        /// <summary>
        /// Deactivates the camera for viewing statistics
        /// </summary>
        private void deactivateStatsCamera() {
            StatsCamera.GetComponent<UnityEngine.Camera>().enabled = false;
        }

        /// <summary>
        /// Deactivates the first person camera
        /// </summary>
        private void deactivateFirstPersonCamera() {
            FirstPersonCamera.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        /// <summary>
        /// Deactivates the 'default' camera for viewing the simulation
        /// </summary>
        private void deactivateRTSCamera() {
            Object.Destroy(RTSCamera);
        }
    }
}
