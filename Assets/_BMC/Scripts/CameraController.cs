using UnityEngine;
using Unity.Cinemachine;

namespace BMC
{
    public class CameraController : MonoBehaviour
    {
        CameraTarget _cameraTarget;

        Camera _mainCamera;

        [Header("방 모드")]
        [SerializeField] CinemachineCamera _cinemachineCamera;

        [Header("플레이어 모드")]
        [SerializeField] CinemachineCamera _playerCinemachineCamera;
        [SerializeField] CinemachineConfiner2D _confiner;

        [Header("흔들림")]
        [SerializeField] CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
        float _startIntensity = 0.5f;   // 흔들기 시작할 때, 첫 강도
        float _shakeTimer;              // 흔드는 시간
        float _shakeTimerTotal;         // 전체 흔드는 시간

        void Awake()
        {
            Init();
        }

        public void Init()
        {
            _mainCamera = Camera.main;
            _cinemachineCamera = GetComponentInChildren<CinemachineCamera>();
            
            // 카메라 목표
            _cameraTarget = new CameraTarget();

            // 카메라 쉐이킹
            _cinemachineBasicMultiChannelPerlin = _cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }

        void Update()
        {
            if (_shakeTimer > 0)
            {
                _shakeTimer -= Time.deltaTime;
                if (_shakeTimer <= 0)
                {
                    _cinemachineBasicMultiChannelPerlin.AmplitudeGain = Mathf.Lerp(_startIntensity, 0f, 1 - (_shakeTimer / _shakeTimerTotal));
                }
            }
        }

        // 카메라 흔들기
        public void ShakeCamera(float intensity = 5f, float time = 0.1f)
        {
            _cinemachineBasicMultiChannelPerlin.AmplitudeGain = intensity;
            _startIntensity = intensity;
            _shakeTimerTotal = time;
            _shakeTimer = time;
        }

        #region 카메라 목표 관련
        // 카메라 방 목표 설정
        public void SetCameraTargetRoom(Transform target)
        {
            _cinemachineCamera.enabled = true;
            _playerCinemachineCamera.enabled = false;
            if (_cameraTarget.TrackingTarget != target)
            {
                // 목표 설정
                _cameraTarget.TrackingTarget = target;
                _cinemachineCamera.Lens.OrthographicSize = 7f;
                _cinemachineCamera.Target = _cameraTarget;
                Debug.LogWarning($"카메라 목표 설정: {target.name}");
            }
        }

        // 카메라 목표 설정
        public void SetCameraTarget(Transform target)
        {
            _cameraTarget.TrackingTarget = target;
            _cinemachineCamera.Target = _cameraTarget;
        }
        #endregion
    }
}