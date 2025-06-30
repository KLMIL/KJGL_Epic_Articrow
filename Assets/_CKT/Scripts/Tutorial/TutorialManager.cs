using System;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance => _instance;
        static TutorialManager _instance;

        public ActionT1<bool> OnEquipPartsActionT1 = new(); //파츠를 획득하면 장착하라는 UI 활성화
        public ActionT0 OnOpenDoorActionT0 = new(); //모든 문 열기
        bool _isRightHand; //우클릭으로 스킬을 사용했는지

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            YSJ.Managers.Input.OnInteractAction += CheckInteraction;
            YSJ.Managers.Input.OnLeftHandActionEnd += CheckLeftHandEnd;
            YSJ.Managers.Input.OnRightHandAction += CheckRightHand;
        }

        private void OnDisable()
        {
            OnEquipPartsActionT1.Init();
            OnOpenDoorActionT0.Init();

            YSJ.Managers.Input.OnInteractAction -= CheckInteraction;
            YSJ.Managers.Input.OnLeftHandActionEnd -= CheckLeftHandEnd;
            YSJ.Managers.Input.OnRightHandAction -= CheckRightHand;
        }

        #region [CheckInteraction]
        void CheckInteraction()
        {
            StartCoroutine(CheckInteractionCoroutine());
        }

        IEnumerator CheckInteractionCoroutine()
        {
            yield return null;
            ImageParts imageParts = BMC.PlayerManager.Instance.GetComponentInChildren<InventorySlot>().GetComponentInChildren<ImageParts>();
            if (imageParts != null)
            {
                OnEquipPartsActionT1.Trigger(true);
                YSJ.Managers.Input.OnInteractAction -= CheckInteraction;
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
            ImageParts imageParts = BMC.PlayerManager.Instance.GetComponentInChildren<RightSlot>().GetComponentInChildren<ImageParts>();
            if (imageParts != null)
            {
                OnEquipPartsActionT1.Trigger(false);
                YSJ.Managers.Input.OnLeftHandActionEnd -= CheckLeftHandEnd;
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
            EquipedArtifact equipedArtifact = BMC.PlayerManager.Instance.GetComponentInChildren<EquipedArtifact>();
            ImageParts imageParts = BMC.PlayerManager.Instance.GetComponentInChildren<RightSlot>().GetComponentInChildren<ImageParts>();
            //아티팩트를 장착해야 한다 + 파츠를 장착해야 한다
            if ((equipedArtifact != null) && (imageParts != null))
            {
                _isRightHand = true;
                YSJ.Managers.Input.OnRightHandAction -= CheckRightHand;
            }
        }
        #endregion

        #region [TutorialClear]
        public void TutorialClear()
        {
            if (_isRightHand)
            {
                //문 열림
                OnOpenDoorActionT0.Trigger();
            }
        }
        #endregion
    }
}