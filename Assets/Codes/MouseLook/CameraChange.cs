using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public Camera first_person_camera;
    public Camera third_person_camera;

    private void Start()
    {
        first_person_camera.enabled= true;
        third_person_camera.enabled= false;
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
        first_person_camera.enabled = !first_person_camera.enabled;
        third_person_camera.enabled = !third_person_camera.enabled;
    }

    public Camera getMainCamera()
    {
        return first_person_camera.enabled?first_person_camera:third_person_camera;
    }
}
