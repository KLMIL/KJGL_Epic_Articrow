using UnityEngine;

namespace CKT
{
    public class FieldParts : MonoBehaviour, IInteractable
    {
        public ItemType ItemType => _itemType;
        ItemType _itemType;

        public string ImagePartsName;
        GameObject _imageParts;

        void Awake()
        {
            _itemType = ItemType.Parts;
            _imageParts = Resources.Load<GameObject>($"ImageParts/{ImagePartsName}");
        }

        public void Interact(Transform trans)
        {
            if (_imageParts != null)
            {
                GameObject imageParts = Instantiate(_imageParts);
                YSJ.Managers.UI.OnAddInventorySlotActionT1.Publish(imageParts);
                Destroy(this.gameObject);
            }
            else
            {
                Debug.LogError("_imageParts is null. Check ImagePartsName");
            }
        }
    }
}


