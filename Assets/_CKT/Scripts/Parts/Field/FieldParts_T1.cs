using UnityEngine;

namespace CKT
{
    public class FieldParts_T1 : MonoBehaviour, IInteractable
    {
        public ItemType ItemType => _itemType;
        ItemType _itemType;

        GameObject _imageParts;

        void Awake()
        {
            _itemType = ItemType.Parts;
            _imageParts = Resources.Load<GameObject>("ImageParts/ImageParts_T1");
        }

        public void Interact(Transform trans)
        {
            GameObject imageParts = Instantiate(_imageParts);
            //Managers.Inventory.InventorySlot.Add(imageParts);
            //Managers.UIManager.UI_AddInventory(imageParts);
            GameManager.Instance.Inventory.InventorySlot.Add(imageParts);
            YSJ.Managers.UI.UI_AddInventory(imageParts);
            Destroy(this.gameObject);
        }
    }
}