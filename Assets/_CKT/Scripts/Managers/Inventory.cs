using System;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    [System.Serializable]
    public class Inventory
    {
        event Action<List<GameObject>> OnLeftHandEvent;
        public void SubLeftHand(Action<List<GameObject>> newSub)
        {
            OnLeftHandEvent = null;
            OnLeftHandEvent += newSub;
        }
        public void InvokeLeftHand()
        {
            OnLeftHandEvent?.Invoke(_leftSlot);
        }

        event Action<List<GameObject>> OnRightHandEvent;
        public void SubRightHand(Action<List<GameObject>> newSub)
        {
            OnRightHandEvent = null;
            OnRightHandEvent += newSub;
        }
        public void InvokeRightHand()
        {
            OnRightHandEvent?.Invoke(_rightSlot);
        }

        public List<GameObject> LeftSlot => _leftSlot;
        List<GameObject> _leftSlot = new List<GameObject>();

        public List<GameObject> RightSlot => _rightSlot;
        List<GameObject> _rightSlot = new List<GameObject>();

        public List<GameObject> InventorySlot => _inventorySlot;
        List<GameObject> _inventorySlot = new List<GameObject>();

        int _maxCount = 8;

        public void Init()
        {
            OnLeftHandEvent = null;
            OnRightHandEvent = null;
            _leftSlot = new List<GameObject>();
            _rightSlot = new List<GameObject>();
            _inventorySlot = new List<GameObject>();
        }

        public bool CheckInventorySlotFull()
        {
            return _inventorySlot.Count >= _maxCount;
        }

        public void RemoveAtSlot(GameObject item)
        {
            if (_leftSlot.Contains(item))
            {
                _leftSlot.Remove(item);
            }
            else if (_rightSlot.Contains(item))
            {
                _rightSlot.Remove(item);
            }
            else if (_inventorySlot.Contains(item))
            {
                _inventorySlot.Remove(item);
            }
        }
    }
}