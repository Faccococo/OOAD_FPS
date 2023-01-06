using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraChange_ol : NetworkBehaviour
{
    public Camera first_person_camera;
    public Camera third_person_camera;

    public override void OnStartLocalPlayer()
    {
        first_person_camera.enabled = true;
        third_person_camera.enabled = false;
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
