using UnityEngine;

namespace CKT
{
    public abstract class FieldParts : MonoBehaviour, IInteractable
    {
        public ItemType ItemType => _itemType;
        ItemType _itemType;

        GameObject _imageParts;

        protected virtual void Init(string name)
        {
            _itemType = ItemType.Parts;
            _imageParts = Resources.Load<GameObject>(name);
        }

        public void Interact(Transform trans)
        {
            GameObject imageParts = Instantiate(_imageParts);
            YSJ.Managers.UI.InvokeAddInventorySlot(imageParts);
            Destroy(this.gameObject);
        }
    }
}


