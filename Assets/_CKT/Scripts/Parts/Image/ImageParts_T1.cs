using BMC;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CKT
{
    public class ImageParts_T1 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        GameObject _fieldParts;

        RectTransform _rect;
        Image _image;

        GameObject _emptyObj;

        private void Awake()
        {
            _fieldParts = Resources.Load<GameObject>("FieldParts/FieldParts_T1");

            _rect = GetComponent<RectTransform>();
            _image = GetComponent<Image>();

            _emptyObj = null;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _emptyObj = new GameObject("Empty");
            _emptyObj.AddComponent<RectTransform>();
            _emptyObj.transform.SetParent(this.transform.parent);
            _emptyObj.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

            this.transform.SetParent(transform.root);
            this.transform.SetAsLastSibling();
            _image.raycastTarget = false;

            //Managers.Inventory.RemoveAtSlot(this.gameObject);
            GameManager.Instance.Inventory.RemoveAtSlot(this.gameObject);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rect.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _image.raycastTarget = true;
            if (this.transform.parent.parent == null)
            {
                //필드 파츠 생성
                GameObject fieldParts = Instantiate(_fieldParts);
                fieldParts.transform.SetParent(null);
                
                // TODO: 더미 플레이어로 변경함 확인바람
                //Vector3 playerPos = FindAnyObjectByType<PlayerController>().transform.position;
                Vector3 playerPos = FindAnyObjectByType<DummyPlayerController>().transform.position;
                fieldParts.transform.position = playerPos + Vector3.down;

                //인벤토리 리스트에서 제거
                Destroy(this.gameObject);
            }
            Destroy(_emptyObj);
        }
    }
}