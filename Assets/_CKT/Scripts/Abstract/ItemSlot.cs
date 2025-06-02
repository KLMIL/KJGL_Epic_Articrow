using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    public abstract class ItemSlot : MonoBehaviour
    {
        protected Transform[] _dropAreas;

        protected virtual void Init()
        {
            //자식 오브젝트로 있는 슬롯들 가져오기
            UI_DropArea[] ui_DropAreas = GetComponentsInChildren<UI_DropArea>();
            _dropAreas = new Transform[ui_DropAreas.Length];
            for (int i = 0; i < ui_DropAreas.Length; i++)
            {
                _dropAreas[i] = ui_DropAreas[i].transform;
            }
        }

        /// <summary>
        /// 왼손, 오른손, 인벤토리 중 해당 오브젝트의 내용물 갱신
        /// </summary>
        /// <param name="list"></param>
        protected virtual void UpdateItemSlotList(List<ImageParts> list)
        {
            list.Clear();
            for (int i = 0; i < _dropAreas.Length; i++)
            {
                ImageParts imageParts = _dropAreas[i].GetComponentInChildren<ImageParts>();
                if (imageParts != null)
                {
                    list.Add(imageParts);
                }
            }
        }
    }
}