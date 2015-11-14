using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    private float strafeSpeed = 1f;
    private float forwardBackwardSpeed = 80f;
    private float lookSpeed = 2f;

    private EnvironmentManager environmentManager;
    
    // Use this for initialization
    void Start()
    {
        this.environmentManager = EnvironmentManager.Shared();
        //setup initial camera view
        Vector3 environmentCenter = (this.environmentManager.Bounds / 2) + this.environmentManager.transform.position;
        transform.LookAt(environmentCenter);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) //forward
        {
            transform.Translate(Vector3.up * strafeSpeed);
        }
        if (Input.GetKey(KeyCode.S)) //back
        {
            transform.Translate(Vector3.down * strafeSpeed);
        }
        if (Input.GetKey(KeyCode.A)) //left strafe
        {
            transform.Translate(Vector3.left * strafeSpeed);
        }
        if (Input.GetKey(KeyCode.D)) //right strafe
        {
            transform.Translate(Vector3.right * strafeSpeed);
        }

        float mouseWheelChange = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheelChange != 0)
        {
            if (mouseWheelChange > 0)
            {
                transform.Translate(Vector3.forward * forwardBackwardSpeed * mouseWheelChange);
            }
            else
            {
                transform.Translate(Vector3.forward * forwardBackwardSpeed * mouseWheelChange);
            }
        }

        if (Input.GetMouseButton(2)) //middle mouse
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 mousePoint = ray.GetPoint(0.0f);

            // find rotation needed to look at mouse point
            Quaternion targetRotation = Quaternion.LookRotation(mousePoint - transform.position);

            // rotate towards the mouse point - smoothly
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);

        }
    }
}
