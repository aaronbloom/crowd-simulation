using UnityEngine;
using System.Collections;
using Assets.Scripts.Environment;


namespace Assets.Scripts.Camera
{
    public class CameraController : MonoBehaviour
    {

        private static readonly KeyCode Up = KeyCode.W;
        private static readonly KeyCode Left = KeyCode.A;
        private static readonly KeyCode Down = KeyCode.S;
        private static readonly KeyCode Right = KeyCode.D;
        private static readonly string Scrollwheel = "Mouse ScrollWheel";
        private static readonly int MiddleMouse = 2;

        private readonly float strafeSpeed = 1f;
        private readonly float forwardBackwardSpeed = 80f;
        private readonly float lookSpeed = 2f;

        private EnvironmentManager environmentManager;

        // Use this for initialization
        void Start()
        {
            this.environmentManager = EnvironmentManager.Shared();
        }

        // Update is called once per frame
        void Update()
        {
            checkKeypress();
            checkMouseScroll();
            checkForMouseMovement();
        }

        public void LookAt(Vector3 position)
        {
            transform.LookAt(position);
        }

        public void LookAtEnvironmentCenter()
        {
            LookAt(environmentManager.CurrentEnvironment.EnvironmentCenter);
        }

        private void checkKeypress()
        {
            if (isKeyInput(Up))
            {
                moveCamera(Vector3.up);
            }
            if (isKeyInput(Down))
            {
                moveCamera(Vector3.down);
            }
            if (isKeyInput(Left))
            {
                moveCamera(Vector3.left);
            }
            if (isKeyInput(Right))
            {
                moveCamera(Vector3.right);
            }
        }

        private void moveCamera(Vector3 direction)
        {
            transform.Translate(direction*strafeSpeed);
        }

        private static bool isKeyInput(KeyCode keyCode)
        {
            return Input.GetKey(keyCode);
        }

        private void checkMouseScroll()
        {
            float mouseWheelChange = Input.GetAxis(Scrollwheel);
            if (mouseWheelChange != 0)
            {
                transform.Translate(Vector3.forward*forwardBackwardSpeed*mouseWheelChange);
            }
        }

        private void checkForMouseMovement()
        {
            if (isMiddleMousePressed())
            {
                calculateRoationFromMouseLocation();

            }
        }

        private void calculateRoationFromMouseLocation()
        {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 mousePoint = ray.GetPoint(0.0f);
            Quaternion targetRotation = rotationToMouseLocation(mousePoint);
            transform.rotation = rotateTowardsMouse(targetRotation);
        }

        private static bool isMiddleMousePressed()
        {
            return Input.GetMouseButton(MiddleMouse);
        }

        private Quaternion rotationToMouseLocation(Vector3 mousePoint)
        {
            return Quaternion.LookRotation(mousePoint - transform.position);
        }

        private Quaternion rotateTowardsMouse(Quaternion targetRotation)
        {
            return Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed*Time.deltaTime);
        }
    }
}
