using System;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    public class Inventory : MonoBehaviour
    {
        public CKT.SkillManager SkillManager { get; private set; } = new();

        #region [인벤토리 리스트]
        List<GameObject> _inventoryList = new List<GameObject>();
        #endregion

        #region [인벤토리 갱신]
        public ActionT1<List<GameObject>> OnUpdateInventoryListActionT1 = new();
        #endregion

        #region [인벤토리 개수]
        public FuncT0<int> GetSlotCountInt = new();
        #endregion

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            _inventoryList = new List<GameObject>();
            OnUpdateInventoryListActionT1.Init();
            GetSlotCountInt.Init();
        }

        public bool CheckInventorySlotFull()
        {
            return _inventoryList.Count >= GetSlotCountInt.Trigger();
        }

        /// <summary>
        /// 왼손, 오른손, 인벤토리 내용물 갱신, 왼손, 오른손 스킬 레벨 갱신
        /// </summary>
        public void InvokeUpdateList()
        {
            //인벤토리 내용물 확인
            OnUpdateInventoryListActionT1.Trigger(_inventoryList);
            SkillManager.CheckSkill();
        }
    }
}