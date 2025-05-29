using UnityEngine;
using Unity.Cinemachine;

namespace BMC
{
    public class CameraController : MonoBehaviour
    {
        static CameraController _instance;
        public static CameraController Instance => _instance;

        [SerializeField] CinemachineCamera _cinemachineCamera;
        CameraTarget _cameraTarget;

        [Header("흔들림")]
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

            _cinemachineCamera = GetComponentInChildren<CinemachineCamera>();
            _cinemachineBasicMultiChannelPerlin = _cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            _cameraTarget = new CameraTarget();
        }

        void Start()
        {
            Door.OnEnterAction += SetCameraTarget;
        }

        void Update()
        {
            //// 테스트 코드
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

        // 카메라 목표 설정
        public void SetCameraTarget(Transform target)
        {
            // 목표 설정
            _cameraTarget.TrackingTarget = target;
            _cinemachineCamera.Target = _cameraTarget;
            Debug.LogWarning($"카메라 목표 설정: {target.name}");
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
}