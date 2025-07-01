using UnityEngine;

namespace CKT
{
    public class InventorySlot : ItemSlot
    {
        void Start()
        {
            base.Init();

            //BMC.PlayerManager.Instance.Inventory.OnUpdateInventoryListActionT1.SingleRegister((list) => base.UpdateItemSlotList(list));
            //BMC.PlayerManager.Instance.Inventory.GetSlotCountInt.SingleRegister(() => base._dropAreas.Length);
            YSJ.Managers.UI.OnAddInventorySlotActionT1.SingleRegister((obj) => AddInventorySlot(base._dropAreas, obj));
        }

        /// <summary>
        /// 필드 파츠를 획득했을 때 인벤토리에 이미지 파츠를 추가하는 용도
        /// </summary>
        /// <param name="dropAreas"></param>
        /// <param name="item"></param>
        public void AddInventorySlot(Transform[] dropAreas, GameObject item)
        {
            //들어갈 슬롯 위치 정하기
            Transform newParent = null;
            for (int i = 0; i < dropAreas.Length; i++)
            {
                if (dropAreas[i].transform.childCount > 1)
                {
                    continue;
                }
                else
                {
                    newParent = dropAreas[i].transform;
                    break;
                }
            }

            //들어가 슬롯을 찾으면 해당 슬롯 자식오브젝트로 넣기
            if (newParent != null)
            {
                item.transform.SetParent(newParent);
                item.GetComponent<RectTransform>().position = newParent.GetComponent<RectTransform>().position;

                //BMC.PlayerManager.Instance.Inventory.InvokeUpdateList();
            }
            else
            {
                Debug.LogWarning("인벤토리 칸이 꽉 찼습니다.");
            }
        }
    }
}