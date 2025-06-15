using UnityEngine;
using Unity.Cinemachine;

namespace BMC
{
    public class CameraController : MonoBehaviour
    {
        static CameraController s_instance;
        public static CameraController Instance => s_instance;

        CameraTarget _cameraTarget;

        [Header("방 모드")]
        [SerializeField] CinemachineCamera _roomCinemachineCamera;

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
            if (s_instance == null)
            {
                s_instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            CinemachineCamera[] cinemachineCameras = GetComponentsInChildren<CinemachineCamera>();
            
            // 방 카메라
            _roomCinemachineCamera = cinemachineCameras[0];
            _cameraTarget = new CameraTarget();

            // 플레이어 카메라
            _playerCinemachineCamera = cinemachineCameras[1];
            _confiner = _playerCinemachineCamera.GetComponent<CinemachineConfiner2D>();
            _cinemachineBasicMultiChannelPerlin = _playerCinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }

        void Start()
        {
            Door.OnTransferToNextRoom = SetCameraTargetRoom;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                SetCameraTargetPlayer();
            else if(Input.GetKeyDown(KeyCode.E))
                SetCameraTargetRoom(MapManager.Instance.CurrentRoom.transform);

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
            _roomCinemachineCamera.gameObject.SetActive(true);
            _playerCinemachineCamera.gameObject.SetActive(false);
            if (_cameraTarget.TrackingTarget != target)
            {
                // 목표 설정
                _cameraTarget.TrackingTarget = target;
                _roomCinemachineCamera.Lens.OrthographicSize = 7f;
                _roomCinemachineCamera.Target = _cameraTarget;
                Debug.LogWarning($"카메라 목표 설정: {target.name}");
            }
        }

        void SetCameraTargetPlayer()
        {
            _confiner.BoundingShape2D = MapManager.Instance.CurrentRoom.GetComponent<PolygonCollider2D>();
            _roomCinemachineCamera.gameObject.SetActive(false);
            _playerCinemachineCamera.gameObject.SetActive(true);
        }
        #endregion

        void OnDestroy()
        {
            Door.OnTransferToNextRoom = null;
        }
    }
}