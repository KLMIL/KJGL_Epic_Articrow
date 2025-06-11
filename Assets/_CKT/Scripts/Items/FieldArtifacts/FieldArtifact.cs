using UnityEngine;

namespace CKT
{
    public class FieldArtifact : MonoBehaviour, IInteractable
    {
        public ItemType ItemType => _itemType;
        ItemType _itemType;

        public string ArtifactName;
        GameObject _equipedArtifact;

        void Awake()
        {
            _itemType = ItemType.Artifact;
            _equipedArtifact = Resources.Load<GameObject>($"EquipedArtifacts/{ArtifactName}");
        }

        public void Interact(Transform trans)
        {
            GameObject equiped = Instantiate(_equipedArtifact);
            equiped.transform.parent = trans;
            equiped.transform.localPosition = Vector3.zero;
            equiped.transform.localRotation = Quaternion.identity;
            Destroy(gameObject);
        }
    }
}