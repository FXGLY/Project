using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private List<Transform> _balls;
    [HideInInspector] [SerializeField] public Transform _target;

    [Header("Properties")]
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _turnSpeed = 10f;
    [HideInInspector] [SerializeField] private int _targetNumber = 0;
    [HideInInspector] [SerializeField] private float _angleX;
    [HideInInspector] [SerializeField] private float _angleY;

    protected void Start()
    {
        _target = _balls[0];
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
        if (_target == null) { return; }
        transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * _moveSpeed);
    }

    private void HandleCameraRotation()
    {
        var x = Input.GetAxis("Mouse X");
        var y = Input.GetAxis("Mouse Y");

        _angleX = transform.localEulerAngles.y + x * _turnSpeed;
        _angleY += y * _turnSpeed;
        _angleY = Mathf.Clamp(_angleY, -90, 90);

        transform.localEulerAngles = new Vector3(-_angleY, _angleX, 0);
    }

    public void LeftButton()
    {
        if (_targetNumber < _balls.Count - 1)
        {
            _target = _balls[++_targetNumber];
        }      
    }

    public void RightButton()
    {
        if (_targetNumber > 0)
        {
            _target = _balls[--_targetNumber];
        }
    }
}
