using UnityEngine;

namespace CKT
{
    public enum ItemType { Artifact, Parts }

    public interface IInteractable
    {
        public ItemType ItemType { get; }

        public void Interact(Transform trans);
    }
}