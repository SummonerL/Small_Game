using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    /* TODO: Needs Refactoring */

    public Camera defaultCamera;
    public Camera closeCamera;
    public Camera leftCamera;
    public Camera rightCamera;
    public List<Camera> cameras = new List<Camera> ();
    public Camera activeCamera;

    public int cameraIndex;

    [SerializeField]
    private GameObject uiController;

    // start is called before the first frame update
    void Start()
    {
        defaultCamera = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Camera>();
        closeCamera = this.gameObject.transform.GetChild(1).gameObject.GetComponent<Camera>();
        leftCamera = this.gameObject.transform.GetChild(2).gameObject.GetComponent<Camera>();
        rightCamera = this.gameObject.transform.GetChild(3).gameObject.GetComponent<Camera>();
        
        cameras.Add(defaultCamera);
        cameras.Add(closeCamera);
        cameras.Add(leftCamera);
        cameras.Add(rightCamera);

        defaultCamera.enabled = true;
        closeCamera.enabled = false;
        leftCamera.enabled = false;
        rightCamera.enabled = false;
        activeCamera = defaultCamera;

        cameraIndex = 0;

        // make sure the canvas is focused on the active camera
        uiController.GetComponent<UIControllerScript>().BillboardCanvas(activeCamera);
    }

    // update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Keyboard_Tab") || Input.GetButtonDown("Joystick_Button_Up"))
        {
            cameras[cameraIndex].enabled = false;
            if (cameraIndex == cameras.Count - 1) 
                cameraIndex = 0;
            else
                cameraIndex++;

            cameras[cameraIndex].enabled = true;
            activeCamera = cameras[cameraIndex];

            // make sure the canvas is focused on the active camera
            uiController.GetComponent<UIControllerScript>().BillboardCanvas(activeCamera);
        }
    }
}
