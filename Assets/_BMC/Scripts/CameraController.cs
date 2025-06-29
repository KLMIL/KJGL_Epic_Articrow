using UnityEngine;
using Unity.Cinemachine;

namespace BMC
{
    public class CameraController : MonoBehaviour
    {
        CameraTarget _cameraTarget;

        Camera _mainCamera;
        Camera _backgroundCamera;

        [Header("방 모드")]
        [SerializeField] CinemachineCamera _roomCinemachineCamera;

        [Header("플레이어 모드")]
        [SerializeField] CinemachineCamera _playerCinemachineCamera;
        [SerializeField] CinemachineConfiner2D _confiner;

        [Header("흔들림")]
        [SerializeField] CinemachineBasicMultiChannelPerlin _roomCinemachineBasicMultiChannelPerlin;
        [SerializeField] CinemachineBasicMultiChannelPerlin _playerCinemachineBasicMultiChannelPerlin;
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
            _backgroundCamera = transform.Find("BackgroundCamera").GetComponent<Camera>();
            CinemachineCamera[] cinemachineCameras = GetComponentsInChildren<CinemachineCamera>();
            
            // 방 카메라
            _roomCinemachineCamera = cinemachineCameras[0];
            _cameraTarget = new CameraTarget();

            // 플레이어 카메라
            _playerCinemachineCamera = cinemachineCameras[1];
            _confiner = _playerCinemachineCamera.GetComponent<CinemachineConfiner2D>();

            // 카메라 쉐이킹
            _roomCinemachineBasicMultiChannelPerlin = _roomCinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            _playerCinemachineBasicMultiChannelPerlin = _playerCinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        }

        void Start()
        {
            //SetCameraRect();
            _cameraTarget.TrackingTarget = PlayerManager.Instance.transform;
            _playerCinemachineCamera.Target = _cameraTarget;
        }

        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Q))
            //    SetCameraTargetPlayer();
            //else if(Input.GetKeyDown(KeyCode.E))
            //    SetCameraTargetRoom(MapManager.Instance.CurrentRoom.transform);

            if (_shakeTimer > 0)
            {
                _shakeTimer -= Time.deltaTime;
                if (_shakeTimer <= 0)
                {
                    _roomCinemachineBasicMultiChannelPerlin.AmplitudeGain = Mathf.Lerp(_startIntensity, 0f, 1 - (_shakeTimer / _shakeTimerTotal));
                    _playerCinemachineBasicMultiChannelPerlin.AmplitudeGain = Mathf.Lerp(_startIntensity, 0f, 1 - (_shakeTimer / _shakeTimerTotal));
                }
            }
        }

        //public void SetCameraRect()
        //{
        //    _backgroundCamera.clearFlags = CameraClearFlags.SolidColor;
        //    _backgroundCamera.backgroundColor = Color.black;
        //    _mainCamera.rect = SettingsResolutionDropdown.CameraRect;
        //}

        // 카메라 흔들기
        public void ShakeCamera(float intensity = 5f, float time = 0.1f)
        {
            _roomCinemachineBasicMultiChannelPerlin.AmplitudeGain = intensity;
            _playerCinemachineBasicMultiChannelPerlin.AmplitudeGain = intensity;
            _startIntensity = intensity;
            _shakeTimerTotal = time;
            _shakeTimer = time;
        }

        #region 카메라 목표 관련
        // 카메라 방 목표 설정
        public void SetCameraTargetRoom(Transform target)
        {
            _roomCinemachineCamera.enabled = true;
            _playerCinemachineCamera.enabled = false;
            if (_cameraTarget.TrackingTarget != target)
            {
                // 목표 설정
                _cameraTarget.TrackingTarget = target;
                _roomCinemachineCamera.Lens.OrthographicSize = 7f;
                _roomCinemachineCamera.Target = _cameraTarget;
                Debug.LogWarning($"카메라 목표 설정: {target.name}");
            }
        }

        public void SetCameraTargetPlayer(Transform player = null)
        {
            _confiner.BoundingShape2D = StageManager.Instance.CurrentRoom.GetComponent<PolygonCollider2D>();
            _roomCinemachineCamera.enabled = false;
            _playerCinemachineCamera.enabled = true;
        }
        #endregion
    }
}