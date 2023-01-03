using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraSpring : MonoBehaviour
{

    public float Frequence;
    public float Damp;
    public Vector2 Min_Recoil_Range;
    public Vector2 Max_Recoil_Range;
    public int Motion_Intensity;

    private CameraSpringUtil cameraSpringUtil;
    private Transform cameraSpringTransform;

    private void Start()
    {
        cameraSpringUtil = new CameraSpringUtil(Frequence, Damp);
        cameraSpringTransform = transform;
        Motion_Intensity /= 10;
    }

    private void Update()
    {
        cameraSpringUtil.UpdateSpring(Time.deltaTime, Vector3.zero);
        cameraSpringTransform.localRotation = Quaternion.Slerp(cameraSpringTransform.localRotation, Quaternion.Euler(cameraSpringUtil.Values), Time.deltaTime * Motion_Intensity);
    }

    public void StartCameraSpring()
    {
        cameraSpringUtil.Values = new Vector3(0,
            UnityEngine.Random.Range(Min_Recoil_Range.x, Max_Recoil_Range.x),
            UnityEngine.Random.Range(Min_Recoil_Range.y, Max_Recoil_Range.y));
    }
}
