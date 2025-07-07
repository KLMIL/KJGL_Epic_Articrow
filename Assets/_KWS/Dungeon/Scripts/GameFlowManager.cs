using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using YSJ;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }

    [SerializeField] int _currentStage = 1;
    [SerializeField] int _currentRoom = -1;


    [SerializeField] List<string> _easyRooms;
    [SerializeField] List<string> _normalRooms;
    [SerializeField] List<string> _hardRooms;

    const int _easyRoomCount = 2;
    const int _normalRoomCount = 3;
    const int _hardRoomCount = 4;

    [SerializeField] int _easyRoomIndex = 0;
    [SerializeField] int _normalRoomIndex = 0;
    [SerializeField] int _hardRoomIndex = 0;



    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _currentStage = 0;
        _currentRoom = 0;

        _easyRoomIndex = 0;
        _normalRoomIndex = 0;
        _hardRoomIndex = 0;
    }

    public void RequestNextRoom()
    {
        // Stage- :: 타이틀 씬 -> 알아서 넘어옴

        _currentRoom++;

        if (_currentStage == 0) // Stage0 :: 튜토리얼 씬 -> Stage1 시작 씬으로 이동
        {
            _currentStage = 1;
            _currentRoom = 0;
            StartCoroutine(Managers.Scene.LoadSceneCoroutine("StageStartScene"));
            return;
        }

        if (_currentStage == 1) // Stage1 :: Room Number에 따라 분기
        {
            PickRandomRooms();

            if (_currentRoom < 2) // 0 -> 1, 2는 쉬움 난이도
            {
                //Debug.Log("Here?");
                StartCoroutine(Managers.Scene.LoadSceneCoroutine(_easyRooms[_easyRoomIndex++]));
            }
            else if (_currentRoom < 5) // 2 -> 3, 4, 5는 보통 난이도
            {
                StartCoroutine(Managers.Scene.LoadSceneCoroutine(_normalRooms[_normalRoomIndex++]));
            }
            else if (_currentRoom == 5) // 5 -> 6은 중간보스
            {
                StartCoroutine(Managers.Scene.LoadSceneCoroutine("MiniBossScene"));
            }
            else if (_currentRoom < 9) // 6 -> 7, 8, 9는 어려움 난이도
            {
                StartCoroutine(Managers.Scene.LoadSceneCoroutine(_hardRooms[_hardRoomIndex++]));
            }
            else if (_currentRoom == 9) // 9 -> 10은 보스
            {
                StartCoroutine(Managers.Scene.LoadSceneCoroutine("GolemBossScene"));
            }
            else // 엔딩씬 호출
            {
                StartCoroutine(Managers.Scene.LoadSceneCoroutine("EndingScene"));
            }
        }
    }

    #region Generate Random Rooms
    private void PickRandomRooms()
    {
        _easyRooms = GenerateRoomNames("Easy", _easyRoomCount);
        _normalRooms = GenerateRoomNames("Normal", _normalRoomCount);
        _hardRooms = GenerateRoomNames("Hard", _hardRoomCount);
    }

    private List<string> GenerateRoomNames(string difficulty, int n)
    {
        // 방 번호 선택을 위한 리스트 -> 랜덤 셔플 후 중복 비허용 n개 선택
        List<int> roomNumbers = new();
        for (int i = 1; i <= 14; i++)
        {
            roomNumbers.Add(i);
        }

        for (int i = 0; i < roomNumbers.Count; i++)
        {
            int j = Random.Range(i, roomNumbers.Count);
            int tmp = roomNumbers[j];

            roomNumbers[i] = roomNumbers[j];
            roomNumbers[j] = tmp;
        }
        List<int> chosenRooms = roomNumbers.GetRange(0, n);


        // 방 종류 선택일 위한 리스트 -> 랜덤 셔플 후 중복 허용 n개 선택
        List<int> typeNumbers = new List<int>();
        for (int i = 0; i < n; i++)
        {
            typeNumbers.Add(Random.Range(1, 6));
        }


        // 방 이름 생성
        List<string> result = new();
        for (int i = 0; i < n; i++)
        {
            string name = $"{difficulty}-{_currentStage:D2}-{chosenRooms[i]:D2}-{typeNumbers[i]:D2}";
            result.Add(name);
        }


        return result;
    }
    #endregion

    #region StartGame
    public void SetTutorial()
    {
        _currentStage = 0;
        _currentRoom = 0;
    }

    public void SetStartGame()
    {
        _currentStage = 1;
        _currentRoom = 0;
    }
    #endregion
}
