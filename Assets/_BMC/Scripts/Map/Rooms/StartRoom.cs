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
                RoomState = RoomState.Active,
                IsCleared = true
            };

            OpenAllValidDoor();
        }

        void Start()
        {
            StageManager.Instance.CurrentRoom = this; // 현재 방 설정
            PlacePlayer();
            Init();
        }
    }
}