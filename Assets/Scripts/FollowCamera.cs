using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("Target")]
    [HideInInspector]
    public Transform target;
    [SerializeField]
    private List<Transform> _balls;

    [Header("Properties")]
    [SerializeField]
    private float _moveSpeed = 1f;
    [SerializeField]
    private float _turnSpeed = 10f;
    private float _angleX;
    private float _angleY;
    private int _targetNumber = 0;

    public void LeftButton()
    {
        if (_targetNumber < _balls.Count - 1)
        {
            target = _balls[++_targetNumber];
        }      
    }

    public void RightButton()
    {
        if (_targetNumber > 0)
        {
            target = _balls[--_targetNumber];
        }
    }

    private void Start()
    {
        target = _balls[0];
    }

    private void LateUpdate()
    {
        FollowTarget();

        if (Input.GetKey(KeyCode.LeftControl))
        {
            HandleCameraRotation();
        }       
    }

    private void FollowTarget()
    {
        if (target == null) { return; }
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * _moveSpeed);
    }

    private void HandleCameraRotation()
    {
        _angleX += Input.GetAxis("Mouse X") * _turnSpeed;
        _angleY += Input.GetAxis("Mouse Y") * _turnSpeed;
        _angleY = Mathf.Clamp(_angleY, -90, 90);
        transform.localEulerAngles = new Vector3(-_angleY, _angleX, 0);
    }
}
