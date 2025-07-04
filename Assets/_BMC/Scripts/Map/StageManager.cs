using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YSJ;
using static Define;

namespace BMC
{
    public class StageManager : MonoBehaviour
    {
        static StageManager s_instance;
        public static StageManager Instance => s_instance;

        [Header("방 이동 관련")]
        [field: SerializeField] public Room CurrentRoom { get; set; }

        [Header("방 보상 관련")]
        public Dictionary<RoomType, List<GameObject>> RoomTypeRewardListDict { get; private set; } = new Dictionary<RoomType, List<GameObject>>(); // 방 타입별 보상 (추후에 스테이지별로 관리할 수 있도록 수정 예정)
        public GameObject PartPrefab { get; private set; } // 파츠 프리팹

        void Awake()
        {
            if (Instance == null)
            {
                s_instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            Init();
        }

        public void Init()
        {
            foreach (RoomType roomType in System.Enum.GetValues(typeof(RoomType)))
            {
                // 방 타입별 보상 오브젝트
                GameObject[] rewards = Managers.Resource.LoadAll<GameObject>($"Prefabs/Rewards/{roomType}");
                RoomTypeRewardListDict.Add(roomType, rewards.ToList());
            }

            // TODO: 승준님 쪽에서 파츠 정보 수정하면 마저 바꾸기
            PartPrefab = Managers.Resource.Load<GameObject>("Prefabs/Rewards/FieldPart");
        }
    }
}