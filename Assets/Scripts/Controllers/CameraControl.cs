using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Transform mTarget;
        
    // Start is called before the first frame update
    void Start()
    {
        mTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        transform.localPosition = new Vector3(mTarget.position.x, transform.position.y, mTarget.position.z);
    }
}
