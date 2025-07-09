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

        [field: SerializeField] public bool IsEquipParts { get; set; } //아티팩트에 파츠를 장착했는지
        [SerializeField] bool _isUsedRightHand;                        //우클릭으로 스킬을 사용했는지
        public Action<bool> OnEquipPartsAction;

        [SerializeField] Artifact_YSJ _artifact;
        [SerializeField] CanDragItem_YSJ _part;
        [SerializeField] ArtifactWindow_YSJ _artifactWindow;

        [Header("카메라")]
        CameraController _cameraController;

        [Header("튜토리얼 진행도")]

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
            OnEquipPartsAction += Pause;

            _cameraController.SetCameraTarget(PlayerManager.Instance.transform);

            _inputSystemActions = Managers.Input.InputSystemActions;
            TutorialInput = GetComponent<TutorialInput>();

            YSJ.Managers.Input.OnInteractAction += CheckInteraction;
            YSJ.Managers.Input.OnRightHandAction += CheckRightHand;
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
            Managers.Input.OnRightHandAction -= CheckRightHand;

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
            _part = PlayerManager.Instance.GetComponentInChildren<Inventory_YSJ>().GetComponentInChildren<CanDragItem_YSJ>();
            _artifactWindow = Managers.UI.InventoryCanvas.ArtifactWindow;

            // 아티팩트 + 파츠 장착한 경우
            if (!IsEquipParts && _artifact != null && _part != null)
            {
                OnEquipPartsAction?.Invoke(true);

                // 튜토리얼 액션 맵 부분 활성화
                TutorialInput.EnableOnlyAction(_inputSystemActions.Tutorial.Interact);
                //Managers.Input.OnInteractAction -= CheckInteraction;
                Debug.Log("아티팩트와 파츠를 먹어서 상호작용 확인 제거");
            }
        }
        #endregion

        public void UnsubscribeCheckInteraction()
        {
            Managers.Input.OnInteractAction -= CheckInteraction;
        }

        #region [CheckRightHand]
        void CheckRightHand()
        {
            StartCoroutine(CheckRightHandCoroutine());
        }

        IEnumerator CheckRightHandCoroutine()
        {
            yield return null;            
            //아티팩트를 장착해야 한다 + 파츠를 장착해야 한다
            if (_artifact != null && _part != null && !_isUsedRightHand)
            {
                _isUsedRightHand = true;
                Managers.Input.OnRightHandAction -= CheckRightHand;
                Debug.Log("아티팩트와 파츠를 먹고 난 후, 스킬 공격 했으니 스킬 공격 확인 제거");
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
            if (_isUsedRightHand && !StageManager.Instance.CurrentRoom.RoomData.IsCleared)
            {
                // 튜토리얼 안내 문구
                UI_TutorialEventBus.OnTutorialText?.Invoke(6);

                // 방 클리어
                StageManager.Instance.CurrentRoom.SetRoomClear();
                StageManager.Instance.CurrentRoom.OpenAllValidDoor();

                // 튜토리얼 클리어 여부 저장 시도
                TrySaveTutorialClear();
            }
        }

        public void TrySaveTutorialClear()
        {
            if (Managers.Data.IsClearTutorial == false)
            {
                Managers.Data.IsClearTutorial = true;
            }
        }
        #endregion
    }
}