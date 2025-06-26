using UnityEngine;
using static Define;

namespace BMC
{
    public class BossRoom : Room
    {
        public override void Init()
        {
            // 방 데이터 초기화
            _roomData = new RoomData
            {
                RoomType = RoomType.BossRoom,
                RoomState = RoomState.Undiscover,
                IsCleared = false,
            };

            //DisposeInvalidDoor();
            //OpenAllValidDoor();
            //GetComponentInChildren<EnemySpawner>().SpawnBoss();
        }

        void Start()
        {
            PlacePlayer();
        }

        //public void SpawnBoss()
        //{
        //    GetComponentInChildren<EnemySpawner>().SpawnBoss();
        //}

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (collision.CompareTag("Player"))
        //    {
        //        if (!_roomData.IsCleared)
        //        {
        //            Debug.Log("보스 소환 가자고~");
        //            // 보스 소환
        //            SpawnBoss();
        //        }
        //    }
        //}
    }
}