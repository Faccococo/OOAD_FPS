using Codes.Weapon;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OOADFPS;

public class MouseLookAt : OOADFPSController
{
    public CameraChange cameraController;
    private Transform cameraTransform;
    private Transform characterTransform;
    private Vector3 camaraRotation;
    private float currentRecoilTime;
    private Vector2 currentRecoil;
    private CameraSpring cameraSpring;
    private Camera camera_now;

    public float MouseSensity;
    public Vector2 Rotate_Range;
    public AnimationCurve Recoil_Curve;
    public Vector2 Recoil_Range;
    public float Recoil_Fade_Out_Time;
    public WeaponManager weaponManager;

    void Start()
    {
        cameraTransform = cameraController.getMainCamera().transform;
        characterTransform = transform;
        Cursor.lockState = CursorLockMode.Locked;
        cameraSpring = GetComponentInChildren<CameraSpring>();
    }

    void Update()
    {
        camera_now = cameraController.getMainCamera();
        cameraTransform = camera_now.transform;
        
        if (this.GetModel<IPauseModel>().IsPause.Value == true)
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        var inputX = Input.GetAxis("Mouse X");
        var inputY = Input.GetAxis("Mouse Y");

        camaraRotation.x -= inputY * MouseSensity * 0.01f;
        camaraRotation.y += inputX * MouseSensity * 0.01f;

        camaraRotation.x = Mathf.Clamp(camaraRotation.x, Rotate_Range.x, Rotate_Range.y);
        if (camera_now == cameraController.first_person_camera)
            cameraTransform.rotation = Quaternion.Euler(camaraRotation.x, camaraRotation.y, 0);
        characterTransform.rotation = Quaternion.Euler(camaraRotation.x, camaraRotation.y, 0);

        CalculateRecoilOffset();
        float recoilRate = weaponManager.getCarriedWeapon().isAiming() ? 0.5f : 1f;
        camaraRotation.x -= currentRecoil.y * recoilRate;
        camaraRotation.y += currentRecoil.x * recoilRate;
    }

    private void CalculateRecoilOffset()
    {
        currentRecoilTime += Time.deltaTime;
        float tmp_RecoilFraction = currentRecoilTime / Recoil_Fade_Out_Time;
        float tmp_RecoilValue = Recoil_Curve.Evaluate(tmp_RecoilFraction);
        currentRecoil = Vector2.Lerp(Vector2.zero, currentRecoil, tmp_RecoilValue);
    }

    public void FiringForTest()
    {
        currentRecoil += Recoil_Range;
        cameraSpring.StartCameraSpring();
        currentRecoilTime = 0;
    }

}
