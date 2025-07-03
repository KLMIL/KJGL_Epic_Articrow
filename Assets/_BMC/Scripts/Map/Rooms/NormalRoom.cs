using static Define;

namespace BMC
{
    public class NormalRoom : Room
    {
        public override void Init()
        {
            // 방 데이터 초기화
            _roomData = new RoomData
            {
                RoomType = RoomType.None,
                IsCleared = false,
            };
        }

        void Start()
        {
            StageManager.Instance.CurrentRoom = this; // 현재 방 설정
            PlacePlayer();
            Init();
            EnemySpawner.Instance.Init();
        }
    }
}