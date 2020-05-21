using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class Ball : MonoBehaviour
{
    [HideInInspector] [SerializeField] private FollowCamera Camera;

    [Header("Path")]
    [SerializeField] public TextAsset _pathAsset;
    [SerializeField] private Vector3[] _pathPoints;

    [Header("UI")]
    [SerializeField] private Slider _slider;

    [Header("Properties")]
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private TrailRenderer _trailRenderer;
    [HideInInspector] [SerializeField] private bool _isMove = false;
    [HideInInspector] [SerializeField] private int _pointNumber = 0;
    [HideInInspector] [SerializeField] private int _clickCounter;


    private void Awake()
    {
        if (_pathAsset != null)
        {
            _pathPoints = JsonConvert.DeserializeObject<JsonPathConverter>(_pathAsset.text).Points;
            transform.position = _pathPoints[0];
        }
        Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowCamera>();
    }

    private void Update()
    {
        if (Camera._target != transform || transform.position == _pathPoints[_pathPoints.Length - 1])
        {
            _isMove = false;
        }

        if (_isMove)
        {
            BallMovement();
        }

        if(Camera._target == transform && !_isMove)
        {
           _slider.gameObject.SetActive(false);
        }   
    }

    private void OnMouseDown()
    {
        _clickCounter++;

        if (_clickCounter == 1)
        {
            StartCoroutine("DoubleClickEvent");
        }
    }

    private void BallMovement()
    {
        _slider.gameObject.SetActive(true);
        _moveSpeed = _slider.value;
        transform.position = Vector3.MoveTowards(transform.position, _pathPoints[_pointNumber], _moveSpeed);

        if (transform.position == _pathPoints[_pointNumber] && _pointNumber != _pathPoints.Length - 1)
        {
            _pointNumber++;
        }
    }

    IEnumerator DoubleClickEvent()
    {
        yield return new WaitForSeconds(.3f);

        if (_clickCounter > 1 && Camera._target == transform)
        {
            _isMove = false;
            _pointNumber = 0;
            transform.position = _pathPoints[0];
            _trailRenderer.Clear();
        }
        else
        {
            if (!_isMove)
            {
                _isMove = true;
            }
            else
            {
                _isMove = false;
            }
        }

        yield return new WaitForSeconds(.05f);
        _clickCounter = 0;
    }
}
