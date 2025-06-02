using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CKT
{
    public abstract class ImageParts : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        GameObject _fieldParts;

        #region [컴포넌트]
        RectTransform _rect;
        Image _img;
        #endregion

        protected virtual void Init(string name)
        {
            _fieldParts = Resources.Load<GameObject>(name);

            _rect = GetComponent<RectTransform>();
            _img = GetComponent<Image>();
        }

        //슬롯에서 필드로 버리기
        public virtual void ThrowAway()
        {
            //필드 파츠 생성
            GameObject item = Instantiate(_fieldParts);
            item.transform.SetParent(null);

            // TODO: 더미 플레이어로 변경함 확인바람
            //Vector3 playerPos = FindAnyObjectByType<PlayerController>().transform.position;
            Vector3 playerPos = FindAnyObjectByType<BMC.DummyPlayerController>().transform.position;
            item.transform.position = playerPos + Vector3.down;

            Destroy(this.gameObject);
        }

        #region [Drag]
        public void OnBeginDrag(PointerEventData eventData)
        {
            _img.raycastTarget = false;

            this.transform.SetParent(transform.root);
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
                ThrowAway();
                GameManager.Instance.Inventory.InvokeUpdateList();
            }

            _img.raycastTarget = true;
        }
        #endregion
    }
}