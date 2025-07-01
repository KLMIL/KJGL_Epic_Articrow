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
        public PlayerAttack PlayerAttack { get; private set; }
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

            PlayerMove = this.gameObject.GetComponent<PlayerMove>();
            PlayerDash = this.gameObject.GetComponent<PlayerDash>();
            PplayerInteract = this.gameObject.GetComponent<PlayerInteract>();
            PlayerStatus = this.gameObject.GetComponent<PlayerStatus>();
            PlayerAttack = this.gameObject.GetComponent<PlayerAttack>();
            CheckPlayerDirection = this.gameObject.GetComponent<CheckPlayerDirection>();
            Inventory = this.gameObject.GetComponentInChildren<Inventory>();

            PlayerStatus.Init();
        }

        void Update()
        {
            if(PlayerStatus.IsDead)
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
            if(PlayerStatus.IsDead)
                return;

            //_playerMove.enabled = !_playerDash.Silhouette.IsActive;
            if (!PlayerDash.Silhouette.IsActive && !PlayerAttack.IsAttack)
                PlayerMove.Move();
            //else
            //    _playerMove.Stop();


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