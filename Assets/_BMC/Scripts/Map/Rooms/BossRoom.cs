using UnityEngine;
using static Define;

namespace BMC
{
    public class BossRoom : Room
    {
        public override void Init(int row, int col)
        {
            // 방 데이터 초기화
            _roomData = new RoomData
            {
                RoomType = RoomType.BossRoom,
                RoomState = RoomState.Undiscover,
                Row = row,
                Col = col,
                IsVisited = true,
                IsCleared = false,
            };

            gameObject.name = gameObject.name + $"({RoomData.Row}, {RoomData.Col})";
            DisposeInvalidDoor();
            //OpenAllValidDoor();
            //GetComponentInChildren<EnemySpawner>().SpawnBoss();
        }

        void Start()
        {
            // 보스 방 아이콘 보여주기
            UI_InGameEventBus.OnActiveMinimapRoom?.Invoke(RoomData.Row * MapManager.Instance.MaxCol + RoomData.Col);
            UI_InGameEventBus.OnDeactivateMinimapRoom?.Invoke(RoomData.Row * MapManager.Instance.MaxCol + RoomData.Col);
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