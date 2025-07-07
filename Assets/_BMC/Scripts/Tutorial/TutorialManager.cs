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

        InputSystemActions _inputSystemActions;
        TutorialInput _tutorialInput;
        
        void Awake()
        {
            _instance = this;
        }

        void Start()
        {
            _inputSystemActions = Managers.Input.InputSystemActions;
            _tutorialInput = GetComponent<TutorialInput>();

            YSJ.Managers.Input.OnInteractAction += CheckInteraction;
            YSJ.Managers.Input.OnLeftHandActionEnd += CheckLeftHandEnd;
            YSJ.Managers.Input.OnRightHandAction += CheckRightHand;
        }

        void OnDisable()
        {
            if(_artifact != null)
                Destroy(_artifact.gameObject);
            if(_artifactWindow != null)
                _artifactWindow.ResetWindow();

            Managers.UI.InventoryCanvas.inventory.Clear();

            OnEquipPartsAction = null;

            Managers.Input.OnInteractAction -= CheckInteraction;
            Managers.Input.OnLeftHandActionEnd -= CheckLeftHandEnd;
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
            if (_artifact != null && _part != null)
            {
                OnEquipPartsAction.Invoke(true);
                _tutorialInput.EnableOnlyAction(_inputSystemActions.Tutorial.LeftHand);
                Managers.Input.OnInteractAction -= CheckInteraction;
                Debug.Log("아티팩트와 파츠를 먹어서 상호작용 확인 제거");
            }
        }
        #endregion

        #region [CheckLeftHand]
        void CheckLeftHandEnd()
        {
            StartCoroutine(CheckLeftHandEndCoroutine());
        }

        IEnumerator CheckLeftHandEndCoroutine()
        {
            yield return null;
            if (_part != null && IsEquipParts)
            {
                OnEquipPartsAction.Invoke(false);
                Managers.Input.OnLeftHandActionEnd -= CheckLeftHandEnd;
                _tutorialInput.EnableActionMap();
                Debug.Log("아티팩트와 파츠를 먹어서 평타 공격 확인 제거");
            }
        }
        #endregion

        #region [CheckRightHand]
        void CheckRightHand()
        {
            StartCoroutine(CheckRightHandCoroutine());
        }

        IEnumerator CheckRightHandCoroutine()
        {
            yield return null;            
            //아티팩트를 장착해야 한다 + 파츠를 장착해야 한다
            if ((_artifact != null) && (_part != null) && _isUsedRightHand == false)
            {
                _isUsedRightHand = true;
                Managers.Input.OnRightHandAction -= CheckRightHand;
                Debug.Log("아티팩트와 파츠를 먹고 난 후, 스킬 공격 했으니 스킬 공격 확인 제거");
            }
        }
        #endregion

        #region 튜토리얼 클리어
        public void TutorialClear()
        {
            if (_isUsedRightHand && !StageManager.Instance.CurrentRoom.RoomData.IsCleared)
            {
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