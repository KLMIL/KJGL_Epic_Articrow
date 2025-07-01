using UnityEngine;
using CKT;
using YSJ;
using UnityEngine.SceneManagement;

namespace BMC
{
    public class PlayerManager : MonoBehaviour
    {
        static PlayerManager s_instance;
        public static PlayerManager Instance => s_instance;

        public PlayerMove PlayerMove { get; private set; }
        public PlayerDash PlayerDash { get; private set; }
        public PlayerInteract PplayerInteract { get; private set; }
        public PlayerStatus PlayerStatus { get; private set; }
        //public PlayerAttack PlayerAttack { get; private set; }
        public CheckPlayerDirection CheckPlayerDirection { get; private set; }
        public Inventory Inventory { get; private set; }

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

            PlayerStatus = this.gameObject.GetComponent<PlayerStatus>();
            CheckPlayerDirection = this.gameObject.GetComponent<CheckPlayerDirection>();
            PlayerMove = this.gameObject.GetComponent<PlayerMove>();
            PlayerDash = this.gameObject.GetComponent<PlayerDash>();
            PplayerInteract = this.gameObject.GetComponent<PlayerInteract>();
            //PlayerAttack = this.gameObject.GetComponent<PlayerAttack>();
            Inventory = this.gameObject.GetComponentInChildren<Inventory>();

            PlayerStatus.Init();
            PlayerDash.DashCoolTime = PlayerStatus.DashCoolTime;
        }

        void Update()
        {
            if(PlayerStatus.IsDead || PlayerStatus.IsStop)
                return;

            CheckPlayerDirection.CheckCurrentDirection();

            if (Input.GetMouseButtonDown(2)) // 마우스 휠 클릭
            {
                Room room = FindAnyObjectByType<Room>();
                room.RoomClearComplete();
            }
        }

        void FixedUpdate()
        {
            if (PlayerStatus.IsDead || PlayerStatus.IsStop)
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

        // Awake -> OnEnable -> SceneManager.sceneLoaded -> Start 순서로 실행
        void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnPlayerLoadedScene;
            s_instance = null;
        }

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
    }
}