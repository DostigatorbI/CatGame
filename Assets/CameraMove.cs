using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public delegate void CameraViewpoint(Vector3 pointPosition);
    public static event CameraViewpoint  CameraMoveTriggerHit;
    private Vector3 _cameraPointPosition;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _cameraPointPosition = transform.GetChild(0).position;
            CameraMoveTriggerHit?.Invoke(_cameraPointPosition);
        }
    }
}
