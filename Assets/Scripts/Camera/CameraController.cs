using Unity.Cinemachine;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    static CameraController _instance;
    public static CameraController Instance => _instance;

    [SerializeField] CinemachineCamera _followCamera;
    [SerializeField] CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    float _startIntensity = 0.5f;   // 흔들기 시작할 때, 첫 강도
    float _shakeTimer;              // 흔드는 시간
    float _shakeTimerTotal;         // 전체 흔드는 시간

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _followCamera = FindAnyObjectByType<CinemachineCamera>();
        _cinemachineBasicMultiChannelPerlin = _followCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        Init();
    }

    void Update()
    {
        // 테스트 코드
        //if (Input.GetMouseButtonDown(0))
        //{
        //    ShakeCamera(1f, 0.1f);
        //}

        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0)
            {
                _cinemachineBasicMultiChannelPerlin.AmplitudeGain = Mathf.Lerp(_startIntensity, 0f, 1 - (_shakeTimer / _shakeTimerTotal));
            }
        }
    }

    public void Init()
    {
        // 목표 설정
        //CameraTarget cameraTarget = new CameraTarget();
        //cameraTarget.TrackingTarget = GameObject.FindGameObjectWithTag("Player").transform;
        //_followCamera.Target = cameraTarget;
    }

    // 카메라 흔들기
    public void ShakeCamera(float intensity = 5f, float time = 0.1f)
    {
        _cinemachineBasicMultiChannelPerlin.AmplitudeGain = intensity;
        _startIntensity = intensity;
        _shakeTimerTotal = time;
        _shakeTimer = time;
    }
}