using BMC;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance => _instance;

    public Camera MainCamera { get; private set; }

    public CKT.Inventory Inventory { get; private set; } = new CKT.Inventory();

    public CKT.SkillManager LeftSkillManager { get; private set; } = new();
    public CKT.SkillManager RightSkillManager { get; private set; } = new();

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

        MainCamera = Camera.main;
        Inventory.Init();
        LeftSkillManager.Init();
        RightSkillManager.Init();
        EnemySpawner = GetComponent<BMC.EnemySpawner>();
    }

    public List<GameObject> MagicItems = new();

    // 일시 정지 및 재개 기능
    public void TogglePauseGame(bool isActive)
    {
        int offset = isActive ? 1 : -1;
        _pauseLevel += offset;
        IsPaused = (_pauseLevel > 0) ? true : false;
        Time.timeScale = IsPaused ? 0f : 1f;
    }
}