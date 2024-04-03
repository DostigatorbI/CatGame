using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _startCameraPoint;
    [SerializeField] private float _cameraSpeed;

    private Vector3 _cameraPointPosition;
    private bool _isMoveStarted;
    // Start is called before the first frame update
    void Awake()
    {
        //TryGetComponent<Transform>(out _cameraPoints);
        //_cameraPoints;
        transform.position = _startCameraPoint.position;
    }

    void OnEnable()
    {
        CameraMove.CameraMoveTriggerHit += MoveToNextPoint;
    }
    void OnDisable()
    {
        CameraMove.CameraMoveTriggerHit -= MoveToNextPoint;
    }

    // Update is called once per frame
    void MoveToNextPoint(Vector3 pointPosition)
    {
        _cameraPointPosition = pointPosition;
        _isMoveStarted = true;
    }
    void Update()
    {
        if(_isMoveStarted)
        {
            if(transform.position == _cameraPointPosition)
            {
                _isMoveStarted = false;
            }
            transform.position = Vector3.MoveTowards(transform.position,
            _cameraPointPosition,
            _cameraSpeed * Time.deltaTime);
        }
    }
}
