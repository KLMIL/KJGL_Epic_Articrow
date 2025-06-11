using UnityEngine;

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

        public void SpawnBoss()
        {
            GetComponentInChildren<EnemySpawner>().SpawnBoss();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (!_roomData.IsCleared)
                {
                    // 보스 소환
                    SpawnBoss();
                }
            }
        }
    }
}