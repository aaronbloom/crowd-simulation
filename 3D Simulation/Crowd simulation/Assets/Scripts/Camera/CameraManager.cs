using RTS_Cam;
using UnityEngine;

namespace Assets.Scripts {
    public class CameraManager {
        public RTS_Camera RTSCamera { get; private set; }
        public GameObject StatsCamera { get; private set; }

        public CameraManager() {
            RTSCamera = (BootStrapper.Initialise("RTS_Camera") as GameObject).GetComponent<RTS_Camera>();
            StatsCamera = (BootStrapper.Initialise("Camera") as GameObject);
            StatsCamera.GetComponent<Camera>().enabled = false; //initially not active
        }

        public void SwitchToStatsCamera(Vector3 position, Vector3 direction) {
            StatsCamera.GetComponent<Camera>().enabled = true;
            RTSCamera.enabled = false;

            StatsCamera.GetComponent<Camera>().transform.position = position + Vector3.back;
            StatsCamera.GetComponent<Camera>().transform.LookAt(StatsCamera.GetComponent<Camera>().transform.position + direction);
        }

        public void SwitchToRTSCamera() {
            StatsCamera.GetComponent<Camera>().enabled = false;
            RTSCamera.enabled = true;
        }
    }
}
