using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BMC;

namespace CKT
{
    public abstract class  ImageParts : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        GameObject _fieldParts;

        /// <summary>
        /// Cast, Hit, Passive 설정 (Dictionary 구분 용도)
        /// </summary>
        public abstract Define.SkillType SkillType { get; }

        /// <summary>
        /// Dictionary 안에서 구분하는 용도 + FieldParts 가져오는 용도
        /// </summary>
        public abstract string SkillName { get; }

        #region [컴포넌트]
        RectTransform _rect;
        Image _img;
        #endregion

        #region [부모, 자식 오브젝트]
        Transform _previousParent;
        #endregion

        void Awake()
        {
            Init();
        }

        void Init()
        {
            _fieldParts = Resources.Load<GameObject>($"FieldParts/FieldParts_{SkillName}");

            _rect = GetComponent<RectTransform>();
            _img = GetComponent<Image>();
        }

        //슬롯에서 필드로 버리기
        public virtual void ThrowAway()
        {
            //필드 파츠 생성
            GameObject item = Instantiate(_fieldParts);
            item.transform.SetParent(null);

            //Vector3 playerPos = FindAnyObjectByType<PlayerController>().transform.position;
            Vector3 playerPos = FindAnyObjectByType<BMC.PlayerManager>().transform.position;
            item.transform.position = playerPos + Vector3.down;

            Destroy(this.gameObject);
        }

        #region [Drag]
        public void OnBeginDrag(PointerEventData eventData)
        {
            _previousParent = this.transform.parent;
            _img.raycastTarget = false;

            this.transform.SetParent(this.transform.root.GetComponentInChildren<UI_Inventory>().transform);
            this.transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rect.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //어느 슬롯에도 들어가지 않았다면
            //(슬롯에 들어가는 방식이 IDropHandler로 들어가기 때문에 부모 오브젝트에 IDropHandler가 없다면 어느 슬롯에도 들어가지 않은 것)
            if (this.transform.parent.GetComponent<IDropHandler>() == null)
            {
                Debug.Log("아이템 버리기");

                //// 패시브 스킬이면 효과 제거
                //if (TryGetComponent<PassiveSkill>(out PassiveSkill passiveSkill))
                //{
                //    // 아티팩트에서 바로 버릴 때만 적용되게 하기
                //    if (_previousParent.GetComponentInParent<RightSlot>() != null)
                //    {
                //        Debug.LogWarning("패시브를 아티팩트에서 필드로 버리기");
                //        passiveSkill.Remove();
                //    }
                //}

                ThrowAway();
            }
            //슬롯에 들어갔는데 해당 슬롯에 이미 다른 ImageParts가 있다면 (자신 포함 ImageParts가 2개 이상이면)
            //서로 위치 바꾸기
            else
            {
                ImageParts[] imageParts = this.transform.parent.GetComponentsInChildren<ImageParts>();
                //if (imageParts.Length > 1)
                {
                    for (int i = 0; i < imageParts.Length; i++)
                    {
                        if (imageParts[i].transform != this.transform)
                        {
                            //겹치는 다른 ImageParts를 자신의 이전 위치로 이동시키기
                            imageParts[i].transform.SetParent(_previousParent);
                            imageParts[i].GetComponent<RectTransform>().position = _previousParent.GetComponent<RectTransform>().position;
                        }
                    }
                }

                //// 패시브 효과 적용 여부 확인
                //if (TryGetComponent<PassiveSkill>(out PassiveSkill passiveSkill))
                //{
                //    if (GetComponentInParent<RightSlot>() != null && _previousParent.parent != transform.parent.parent)
                //    {
                //        // 아티팩트 슬롯에 들어갔을 때는 패시브 스킬 효과 적용
                //        passiveSkill.Apply();
                //        Debug.LogWarning("패시브 아이템 착용");
                //    }
                //    else if(_previousParent.parent != transform.parent.parent)
                //    {
                //        // 아티팩트 슬롯이 아닌 경우 패시브 스킬 효과 제거
                //        passiveSkill.Remove();
                //        Debug.LogWarning("다른 종류의 슬롯으로 버리기");
                //    }
                //}
            }

            _img.raycastTarget = true;
            _previousParent = null;

            //BMC.PlayerManager.Instance.Inventory.InvokeUpdateList();
        }
        #endregion
    }
}