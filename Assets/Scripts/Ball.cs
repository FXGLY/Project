using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class Ball : MonoBehaviour
{
    private FollowCamera Camera;

    [Header("Path")]
    public TextAsset pathAsset;
    [SerializeField]
    private Vector3[] _pathPoints;

    [Header("UI")]
    [SerializeField]
    private Slider _slider;

    [Header("Properties")]
    [SerializeField]
    private float _moveSpeed = 1f;
    [SerializeField]
    private TrailRenderer _trailRenderer;
    private bool _isMove = false;
    private int _pointNumber = 0;
    private int _clickCounter;

    private IEnumerator DoubleClickEvent()
    {
        yield return new WaitForSeconds(.3f);

        if (_clickCounter > 1 && Camera.target == transform)
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

    private void Awake()
    {
        if (pathAsset != null)
        {
            _pathPoints = JsonConvert.DeserializeObject<JsonPathConverter>(pathAsset.text).Points;
            transform.position = _pathPoints[0];
        }
        Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowCamera>();
    }

    private void Update()
    {
        if (Camera.target != transform || transform.position == _pathPoints[_pathPoints.Length - 1])
        {
            _isMove = false;
        }

        if (_isMove)
        {
            BallMovement();
        }

        if(Camera.target == transform && !_isMove)
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
}
