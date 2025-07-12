using System;
using System.Collections;
using UnityEngine;
using YSJ;

namespace BMC
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance => _instance;
        static TutorialManager _instance;

        [field: SerializeField] public bool IsEquipParts { get; set; } // 아티팩트에 파츠를 장착했는지
        public Action<bool> OnEquipPartsAction;

        [SerializeField] Artifact_YSJ _artifact;                // 아티팩트
        [field: SerializeField] public CanDragItem_YSJ Part;    // 인벤토리에 들어온 파츠
        [SerializeField] ArtifactWindow_YSJ _artifactWindow;    // 아티팩트 창

        [Header("카메라")]
        CameraController _cameraController;

        [Header("튜토리얼 입력")]
        InputSystemActions _inputSystemActions;
        public TutorialInput TutorialInput { get; private set; }
        
        void Awake()
        {
            _instance = this;

            _cameraController = Camera.main.GetComponentInParent<CameraController>();
        }

        void Start()
        {
            _cameraController.SetCameraTarget(PlayerManager.Instance.transform);
            
            OnEquipPartsAction += Pause;
            YSJ.Managers.Input.OnInteractAction += CheckInteraction;

            _inputSystemActions = Managers.Input.InputSystemActions;
            TutorialInput = GetComponent<TutorialInput>();
        }

        void OnDisable()
        {
            if(_artifact != null)
                Destroy(_artifact.gameObject);
            if(_artifactWindow != null)
                _artifactWindow.ResetWindow();
            if(Managers.UI.InventoryCanvas != null)
                Managers.UI.InventoryCanvas.inventory.Clear();

            OnEquipPartsAction = null;

            Managers.Input.OnInteractAction -= CheckInteraction;

            _instance = null;
        }

        #region [CheckInteraction]
        void CheckInteraction()
        {
            StartCoroutine(CheckInteractionCoroutine());
        }

        IEnumerator CheckInteractionCoroutine()
        {
            yield return null;
            _artifact = PlayerManager.Instance.transform.Find("Hand").GetComponentInChildren<Artifact_YSJ>();
            Part = PlayerManager.Instance.GetComponentInChildren<Inventory_YSJ>().GetComponentInChildren<CanDragItem_YSJ>();
            _artifactWindow = Managers.UI.InventoryCanvas.ArtifactWindow;

            // 아티팩트 + 파츠 장착한 경우
            if (!IsEquipParts && _artifact != null && Part != null)
            {
                OnEquipPartsAction?.Invoke(true);
                Managers.Input.OnInteractAction -= CheckInteraction;

                // 튜토리얼 액션 맵 부분 활성화
                TutorialInput.EnableOnlyAction(_inputSystemActions.Tutorial.Interact);
                Debug.Log("아티팩트와 파츠를 먹어서 상호작용 확인 제거");
            }
        }
        #endregion

        public void Pause(bool isActive)
        {
            Time.timeScale = isActive ? 0f : 1f;
        }

        #region 튜토리얼 클리어
        public void TutorialClear()
        {
            if (IsEquipParts && !StageManager.Instance.CurrentRoom.RoomData.IsCleared)
            {
                // 튜토리얼 안내 문구
                UI_TutorialEventBus.OnTutorialText?.Invoke(6); // TODO: 숫자 6 대신 추후에 변수로 할 수 있도록 해야 함

                SteamAchievement.instance.Achieve(SteamAchievement.AchievementType.TutorialClear);

                // 방 클리어
                StageManager.Instance.CurrentRoom.SetRoomClear();
                StageManager.Instance.CurrentRoom.OpenAllValidDoor();
            }
        }
        #endregion
    }
}