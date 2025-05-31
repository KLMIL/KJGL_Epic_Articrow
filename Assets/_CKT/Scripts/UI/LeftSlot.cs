using UnityEngine;
using UnityEngine.EventSystems;

namespace CKT
{
    public class LeftSlot : MonoBehaviour, IAddSlotable, IDropHandler
    {
        RectTransform _rect;

        void Start()
        {
            _rect = GetComponent<RectTransform>();
        }

        public void AddSlot(GameObject obj)
        {
            obj.transform.SetParent(transform);
            obj.GetComponent<RectTransform>().position = _rect.position;
            GameManager.Instance.Inventory.LeftSlot.Add(obj);
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameObject pointerDrag = eventData.pointerDrag;
            //LeftSlot 위에서 Drop했을 때만 호출
            if ((pointerDrag != null) && (pointerDrag.transform.parent == this.transform.parent))
            {
                AddSlot(pointerDrag);
            }
        }
    }
}