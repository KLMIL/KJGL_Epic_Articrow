using CKT;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace BMC
{
    public class StartRoom : Room
    {
        bool isDoljabied = false;
        GameObject _dolArtifact;

        public override void Init()
        {
            // 방 데이터 초기화
            _roomData = new RoomData
            {
                RoomType = RoomType.StartRoom,
                IsCleared = false
            };

            MakeDoljabi();
        }

        private void Update()
        {
            if (isDoljabied && _dolArtifact.transform.parent.name == "RightHand")
            {
                //bool isPickArtifact = PlayerManager.Instance.GetComponentInChildren<RightHand>(true).transform.childCount != 0;

                //if (isPickArtifact)
                //{
                    _roomData.IsCleared = true;
                    OpenAllValidDoor();
                //}
            }
        }

        void Start()
        {
            StageManager.Instance.CurrentRoom = this; // 현재 방 설정
            PlacePlayer();
            Init();
        }

        private void MakeDoljabi()
        {
            List<GameObject> artifactList = StageManager.Instance.RoomTypeRewardListDict[RoomType.ArtifactRoom];
            GameObject selectArtifact = artifactList[Random.Range(0, artifactList.Count)];

            _dolArtifact = Instantiate(selectArtifact, transform.position, Quaternion.identity);

            isDoljabied = true;
        }
    }
}