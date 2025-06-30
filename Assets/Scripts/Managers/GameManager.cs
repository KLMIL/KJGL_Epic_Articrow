using BMC;
using System.Collections.Generic;
using UnityEngine;
using YSJ;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance => _instance;

    public Camera Camera { get; private set; }
    public CameraController CameraController { get; private set; }

    [Header("일시 정지")]
    [field: SerializeField] public bool IsPaused { get; private set; } = false;

    [SerializeField] int _pauseLevel = 0; // 일시 정지 레벨 (0: 게임 진행 중, 1 이상 일시 정지)

    [Header("몬스터 소환")]
    [SerializeField] public BMC.EnemySpawner EnemySpawner { get; private set; }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }

        TrySpawnPlayer();

        EnemySpawner = GetComponent<BMC.EnemySpawner>();
        //EnemySpawner.Init();
        Camera = Camera.main;
        CameraController = Camera.GetComponentInParent<CameraController>();
    }

    public List<GameObject> MagicItems = new();

    public void TrySpawnPlayer()
    {
        if (PlayerManager.Instance == null)
        {
            GameObject playerGO = Managers.Resource.Instantiate("PlayerPrefab");
            playerGO.name = "PlayerPrefab";
        }

        // TODO: 플레이어 소환 위치 설정
    }

    // 일시 정지 및 재개 기능
    public void TogglePauseGame(bool isActive)
    {
        int offset = isActive ? 1 : -1;
        _pauseLevel += offset;
        IsPaused = (_pauseLevel > 0) ? true : false;
        Time.timeScale = IsPaused ? 0f : 1f;
    }
}