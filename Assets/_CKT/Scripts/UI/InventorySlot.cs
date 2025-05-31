using UnityEngine;
using UnityEngine.EventSystems;

namespace CKT
{
    public class InventorySlot : MonoBehaviour, IAddSlotable, IDropHandler
    {
        RectTransform _rect;

        void Start()
        {
            _rect = GetComponent<RectTransform>();

            YSJ.Managers.UI.OnUI_AddInventorySlotEvent += AddSlot;
        }

        public void AddSlot(GameObject item)
        {
            item.transform.SetParent(this.transform);
            item.GetComponent<RectTransform>().position = _rect.position;
            GameManager.Instance.Inventory.InventorySlot.Add(item);
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameObject pointerDrag = eventData.pointerDrag;
            //InventorySlot 위에서 Drop했을 때만 호출
            if ((pointerDrag != null) && (pointerDrag.transform.parent == this.transform.parent))
            {
                AddSlot(pointerDrag);
            }
        }
    }
}