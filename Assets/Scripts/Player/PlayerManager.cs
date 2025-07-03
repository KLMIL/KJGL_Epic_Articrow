using UnityEngine;
using UnityEngine.SceneManagement;
using YSJ;

namespace BMC
{
    public class PlayerManager : MonoBehaviour
    {
        static PlayerManager s_instance;
        public static PlayerManager Instance => s_instance;

        public PlayerMove PlayerMove { get; private set; }
        public PlayerDash PlayerDash { get; private set; }
        public PlayerStatus PlayerStatus { get; private set; }
        public PlayerHurt PlayerHurt { get; private set; }
        public PlayerDebuff PlayerDebuff { get; private set; }
        public PlayerTextWindow PlayerTextWindow { get; private set; }
        public CheckPlayerDirection CheckPlayerDirection { get; private set; }

        void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
                DontDestroyOnLoad(gameObject);

                SceneManager.sceneLoaded += OnPlayerLoadedScene;
            }
            else
            {
                Destroy(gameObject);
            }

            PlayerStatus = GetComponent<PlayerStatus>();
            PlayerMove = GetComponent<PlayerMove>();
            PlayerDash = GetComponent<PlayerDash>();
            PlayerHurt = GetComponent<PlayerHurt>();
            PlayerDebuff = GetComponent<PlayerDebuff>();
            CheckPlayerDirection = GetComponent<CheckPlayerDirection>();
            PlayerTextWindow = GetComponentInChildren<PlayerTextWindow>(true);

            // 초기화
            PlayerStatus.Init();
            PlayerHurt.Init();
            PlayerDash.DashCoolTime = PlayerStatus.DashCoolTime;
            PlayerTextWindow.Init();
        }

        void Update()
        {
            if (IsStop())
            {
                return;
            }

            CheckPlayerDirection.CheckCurrentDirection();

            TestRoomClear();
        }

        void FixedUpdate()
        {
            if (IsStop())
            {
                PlayerMove.Stop();
                return;
            }

            //if (!PlayerDash.Silhouette.IsActive && !PlayerAttack.IsAttack)
            if (!PlayerDash.Silhouette.IsActive)
            {
                PlayerMove.Move();
            }
        }

        void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnPlayerLoadedScene;
            s_instance = null;
        }

        // Awake -> OnEnable -> SceneManager.sceneLoaded -> Start 순서로 실행
        // 씬 로드될 때 플레이어 오브젝트에게 해줘야할 작업
        void OnPlayerLoadedScene(Scene scene, LoadSceneMode mode)
        {
            if(scene.name.Contains("Title") || scene.name.Contains("End"))
            {
                Clear();
            }
            else
            {
                Debug.LogWarning($"체력: {PlayerStatus.Health}, 마나: {PlayerStatus.Mana}");
            }
        }

        public void Clear()
        {
            Destroy(gameObject);
        }

        // 멈춘 상태 여부 확인
        public bool IsStop()
        {
            // 멈춰야 하는 상황 여기에 추가하기
            return (PlayerHurt.IsDead || PlayerDebuff.HasDebuff(DebuffType.Stun));
        }

        #region 테스트 코드
        public void TestRoomClear()
        {
            if (Input.GetMouseButtonDown(2)) // 마우스 휠 클릭
            {
                Room room = FindAnyObjectByType<Room>();
                room.RoomClearComplete();
            }
        }
        #endregion
    }
}