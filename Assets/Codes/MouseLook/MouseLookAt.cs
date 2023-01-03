using Codes.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookAt : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    private Transform characterTransform;
    private Vector3 camaraRotation;
    private float currentRecoilTime;
    private Vector2 currentRecoil;
    private CameraSpring cameraSpring;

    public float MouseSensity;
    public Vector2 Rotate_Range;
    public AnimationCurve Recoil_Curve;
    public Vector2 Recoil_Range;
    public float Recoil_Fade_Out_Time;
    public WeaponManager weaponManager;


    void Start()
    {
        characterTransform= transform;
        Cursor.lockState = CursorLockMode.Locked;
        cameraSpring = GetComponentInChildren<CameraSpring>();
    }

    void Update()
    {
        var inputX = Input.GetAxis("Mouse X");
        var inputY = Input.GetAxis("Mouse Y");

        camaraRotation.x -= inputY * MouseSensity * 0.01f;
        camaraRotation.y += inputX * MouseSensity * 0.01f;

        camaraRotation.x = Mathf.Clamp(camaraRotation.x, Rotate_Range.x, Rotate_Range.y);
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
