using System.Collections.Generic;
using UnityEngine;

namespace Game.Dungeon
{
    /// <summary>
    /// 방 생성, 문 열기 등 스테이지 전체의 흐름을 관리
    /// </summary>
    public enum RoomDifficulty { Easy, Normal, Hard }

    public class StageController : MonoBehaviour
    {
        public int StageIndex;

        int _roomIndex = 0;


        [Header("Room Prefabs")]
        [SerializeField] List<GameObject> _easyRoomPrefabs = new();
        [SerializeField] List<GameObject> _normalRoomPrefabs = new();
        [SerializeField] List<GameObject> _hardRoomPrefabs = new();

        [SerializeField] GameObject _miniBossRoomPrefab;
        [SerializeField] GameObject _bossRoomPrefab;




        #region Public APIs
        public void GotoNextRoom()
        {
            // 다음 방으로 넘어가야 할 때 호출하는 함수
        }
        #endregion
    }
}



