using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public Camera first_person_camera;
    public Camera third_person_camera;
    private Camera main_camera;

    private void Start()
    {
        main_camera = first_person_camera;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            cameraSwap();
        }
    }

    private void cameraSwap()
    {
        Camera next_camera = (main_camera == first_person_camera) ? third_person_camera :               first_person_camera;
        main_camera.GetComponent<AudioListener>().enabled = false;
        main_camera.enabled = false;
        main_camera = next_camera;
        main_camera.GetComponent<AudioListener>().enabled = true;
        main_camera.enabled = true;
    }

    public Camera getMainCamera()
    {
        return main_camera;
    }
}
