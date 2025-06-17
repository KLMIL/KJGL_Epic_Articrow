using UnityEngine;
using System.Collections;
using YSJ;

public class Spirit : MonoBehaviour
{
    TrailRenderer _trailRenderer;

    bool _isStop = false;
    float _speed = 1.5f;

    [Header("승천")]
    Vector2 _startPos;
    float _spawnElapsedTime = 0f;           // 소환 경과 시간
    float _waveDirection;

    [Header("봉인")]
    bool _isSealed = false;                 // 봉인 여부
    Vector3 _sealStartPos;                  // p1 (시작 지점)
    Vector3 _controlPoint;                  // p2 (컨트롤 포인트)
    Transform _destinationTransform;        // p3 (도착 지점)
    int _upDownDirection;                   // p1과 p3의 중점 기준에서의 p2 방향
    float _upDownOffset = 1f;              // p2를 얼마나 띄울지
    public float _duration;

    void Awake()
    {
        _trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        _startPos = transform.position;
        _spawnElapsedTime = 0f;
        _waveDirection = Random.Range(-1, 2);
        if(_waveDirection == 0)
            _waveDirection = 1;
    }

    void Update()
    {
        if (_isStop)
            return;

        if (!_isSealed && Input.GetKeyDown(KeyCode.LeftControl))
            SetSealed();
    }

    // 봉인 설정
    public void SetSealed(Transform collector = null)
    {
        _isSealed = true;

        // p3 설정
        _destinationTransform = collector;

        // p1 설정
        _sealStartPos = transform.position;

        // p2 설정
        Vector3 middlePos = (_sealStartPos + _destinationTransform.position) / 2;
        Vector3 perpendicularDir = Vector2.Perpendicular(middlePos).normalized;
        _upDownDirection = Random.Range(-1, 2);
        _controlPoint = middlePos + perpendicularDir * _upDownDirection * _upDownOffset;

        StartCoroutine(MoveBezierCurveToTargetCoroutine());
    }

    // 목표까지 베이지어 곡선으로 이동
    IEnumerator MoveBezierCurveToTargetCoroutine(float duration = 1.0f)
    {
        float time = 0f;
        while (Vector2.Distance(transform.position, _destinationTransform.position) >= 0.1f)
        {
            //if (time > 1f)
            //{
            //    time = 0f;
            //}

            // p4, p5 설정
            Vector3 p4 = Vector3.Lerp(_sealStartPos, _controlPoint, time);
            Vector3 p5 = Vector3.Lerp(_controlPoint, _destinationTransform.position, time);

            // 이동
            transform.position = Vector3.Lerp(p4, p5, time);
            if (Vector2.Distance(transform.position, _destinationTransform.position) < 0.1f)
            {
                _isStop = true;
                //_trailRenderer.time = 0.3f;
                Managers.TestPool.Return(Define.PoolID.Mana, gameObject);
                break;
            }

            time += Time.deltaTime / duration;
            yield return null;
        }
    }
}