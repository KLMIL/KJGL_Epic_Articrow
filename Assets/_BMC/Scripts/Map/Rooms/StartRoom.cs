using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace BMC
{
    public class StartRoom : Room
    {
        public override void Init()
        {
            // 방 데이터 초기화
            _roomData = new RoomData
            {
                RoomType = RoomType.StartRoom,
                IsCleared = false
            };

            SpawnArtifact();
        }

        void Update()
        {
            if (!_roomData.IsCleared)
            {
                if (PlayerManager.Instance.PlayerHand.RightHand.IsHoldingArtifact())
                {
                    _roomData.IsCleared = true;
                    OpenAllValidDoor();
                }
            }
        }

        void Start()
        {
            StageManager.Instance.CurrentRoom = this; // 현재 방 설정
            PlacePlayer();
            Init();
            //SteamAchievement.instance.Achieve(SteamAchievement.AchievementType.StartFirstGame); // 첫 게임 시작 도전과제 달성
            AnalyticsManager.Instance.StartAnalystics();
        }

        void SpawnArtifact()
        {
            List<GameObject> artifactList = StageManager.Instance.RoomTypeRewardListDict[RoomType.ArtifactRoom];
            GameObject selectedArtifact = artifactList[Random.Range(0, artifactList.Count)];
            Instantiate(selectedArtifact, transform.position, Quaternion.identity);
        }
    }
}