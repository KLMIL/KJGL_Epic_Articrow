using UnityEngine;

namespace CKT
{
    public class FieldArtifact_T1 : MonoBehaviour, IInteractable
    {
        public ItemType ItemType => _itemType;
        ItemType _itemType;

        GameObject _equipedArtifact;

        void Awake()
        {
            _itemType = ItemType.Artifact;
            _equipedArtifact = Resources.Load<GameObject>("EquipedArtifacts/EquipedArtifact_T1");
        }

        public void Interact(Transform trans)
        {
            GameObject equiped = Instantiate(_equipedArtifact);
            equiped.transform.parent = trans;
            equiped.transform.localPosition = Vector3.zero;
            Destroy(this.gameObject);
        }
    }
}