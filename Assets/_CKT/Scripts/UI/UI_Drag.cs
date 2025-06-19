using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BMC;

namespace CKT
{
    public class UI_Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region [컴포넌트]
        RectTransform _rect;
        Image _image;
        #endregion

        void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _image.raycastTarget = false;

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
                GetComponent<ImageParts>().ThrowAway();
                GameManager.Instance.Inventory.InvokeUpdateList();
            }

            _image.raycastTarget = true;
        }
    }
}