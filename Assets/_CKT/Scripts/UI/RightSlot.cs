using UnityEngine;
using UnityEngine.EventSystems;

namespace CKT
{
    public class RightSlot : MonoBehaviour, IDropHandler
    {
        RectTransform _rect;

        void Start()
        {
            _rect = GetComponent<RectTransform>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameObject pointerDrag = eventData.pointerDrag;
            if (pointerDrag != null)
            {
                //InventorySlot 위에서 Drop했을 때만 호출
                if (pointerDrag.transform.parent == this.transform.parent)
                {
                    pointerDrag.transform.SetParent(transform);
                    pointerDrag.GetComponent<RectTransform>().position = _rect.position;
                    GameManager.Instance.Inventory.RightSlot.Add(pointerDrag);
                }
            }
        }
    }
}