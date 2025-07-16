using BMC;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YSJ;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }

    [SerializeField] int _currentStage = 1;
    [SerializeField] int _currentRoom = -1;


    [SerializeField] List<string> _easyRooms;
    [SerializeField] List<string> _normalRooms;
    [SerializeField] List<string> _hardRooms;

    const int _easyRoomCount = 3;
    const int _normalRoomCount = 4;
    const int _hardRoomCount = 7;

    [SerializeField] int _easyRoomIndex = 0;
    [SerializeField] int _normalRoomIndex = 0;
    [SerializeField] int _hardRoomIndex = 0;

    [SerializeField] bool _isRoomGenerated = false;


    // Properties
    public int CurrentRoom => _currentRoom;


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

    private void Update()
    {
        // TODO: QA 테스트 커맨드용 코드. 제거요망. 
        if (Input.GetKeyDown(KeyCode.F7)) 
        {
            _currentRoom = _easyRoomCount + _normalRoomCount + 1;
            StartCoroutine(Managers.Scene.LoadSceneCoroutine("MiniBossScene"));
        }

        // TODO: QA 테스트 커맨드용 코드. 제거요망. 
        if(Input.GetKeyDown(KeyCode.F8))
        {
            _currentRoom = _easyRoomCount + _normalRoomCount + 1 + _hardRoomCount + 1;
            StartCoroutine(Managers.Scene.LoadSceneCoroutine("GolemBossScene"));
        }
    }

    public void Init()
    {
        _currentStage = 0;
        _currentRoom = 0;

        _easyRoomIndex = 0;
        _normalRoomIndex = 0;
        _hardRoomIndex = 0;

        _isRoomGenerated = false;
    }

    public void InitRetry()
    {
        _currentStage = 1;
        _currentRoom = 0;

        _easyRoomIndex = 0;
        _normalRoomIndex = 0;
        _hardRoomIndex = 0;

        _isRoomGenerated = false;
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
            if (!_isRoomGenerated)
            {
                PickRandomRooms();
            }
            _isRoomGenerated = true;

            if (_currentRoom <= _easyRoomCount) 
            {
                //Debug.Log("Here?");
                StartCoroutine(Managers.Scene.LoadSceneCoroutine(_easyRooms[_easyRoomIndex++]));
            }
            else if (_currentRoom <= _easyRoomCount + _normalRoomCount) 
            {
                StartCoroutine(Managers.Scene.LoadSceneCoroutine(_normalRooms[_normalRoomIndex++]));
            }
            else if (_currentRoom == _easyRoomCount + _normalRoomCount + 1) // 중간보스
            {
                StartCoroutine(Managers.Scene.LoadSceneCoroutine("MiniBossScene"));
            }
            else if (_currentRoom <= _easyRoomCount + _normalRoomCount + 1 + _hardRoomCount) // 중간보스 +1
            {
                StartCoroutine(Managers.Scene.LoadSceneCoroutine(_hardRooms[_hardRoomIndex++]));
            }
            else if (_currentRoom == _easyRoomCount + _normalRoomCount + 1 + _hardRoomCount + 1) // 최종보스
            {
                StartCoroutine(Managers.Scene.LoadSceneCoroutine("GolemBossScene"));
            }
            else // 엔딩씬 호출
            {
                int index = Managers.Scene.GetBuildIndexBySceneName("EndingScene");
                StartCoroutine(Managers.Scene.LoadSceneCoroutine(index));
            }
            PlayerManager.Instance.PlayerHand.RightHand.RecordEquipParts(); // 현재 장착된 파츠 기록
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
