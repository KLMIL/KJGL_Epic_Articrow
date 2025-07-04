using static Define;

namespace CKT
{
    public class TutorialRoom : BMC.Room
    {
        public override void Init()
        {
            // 방 데이터 초기화
            _roomData = new BMC.RoomData
            {
                RoomType = RoomType.None,
                IsCleared = false
            };

            //OpenAllValidDoor();
        }

        void Start()
        {
            //BMC.StageManager.Instance.CurrentRoom = this; // 현재 방 설정
            PlacePlayer();
            Init();

            TutorialManager.Instance.OnOpenDoorActionT0.SingleSubscribe(() => OpenAllValidDoor());
        }
    }
}