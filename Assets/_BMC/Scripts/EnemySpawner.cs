using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace BMC
{
    /// <summary>
    /// 적 소환하는 클래스, Room에서 명령을 내려야 함
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        static EnemySpawner _instance;
        public static EnemySpawner Instance => _instance;

        GameObject[] _enemyPrefabs;

        [Header("웨이브 관련")]
        [SerializeField] bool _isAllWaveClear;
        [SerializeField] List<EnemyWave> _enemyWaveList;
        [SerializeField] int _currentWaveIndex;          // 현재 웨이브 인덱스
        [SerializeField] int _enemyCount;                // 적 수
        [SerializeField] int _enemyDieCount;             // 죽은 적 수

        [Header("위치 관련")]
        Tilemap _spawnAreaTilemap;
        Vector3 _offset = new Vector3(0.5f, 0.5f, 0);                                           // 적 배치는 타일 모서리 위치이므로 타일 중앙에 배치할 수 있도록하는 offset
        [SerializeField] List<Vector3> _possibleSpawnPositionList = new List<Vector3>();        // 소환 가능한 위치 리스트

        public static Action OnWaveStart;

        void Awake()
        {
            // 적 프리팹 로드
            _enemyPrefabs = Resources.LoadAll<GameObject>("Prefabs/Monster/Stage1");

            if (_instance == null)
            {
                _instance = this;
                OnWaveStart = StartWave;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Init()
        {
            SpawnArea spawnArea = StageManager.Instance.CurrentRoom.GetComponentInChildren<SpawnArea>();
            _spawnAreaTilemap = spawnArea.GetComponent<Tilemap>();

            // 타일맵의 경계를 압축하여 정확한 크기를 반영, Tilemap의 Bounds 재설정 (맵을 수정했을 때 Bounds가 변경되지 않는 문제 해결
            _spawnAreaTilemap.CompressBounds();

            // 소환 가능한 타일 계산
            CalculateSpawnPossibleTiles();
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.G))
            {
                StartWave();
            }
        }

        // 웨이브 시작
        public void StartWave()
        {
            if(_currentWaveIndex == _enemyWaveList.Count && !_isAllWaveClear)
            {
                // 클리어
                _isAllWaveClear = true;
                StageManager.Instance.CurrentRoom.RoomClearComplete();
                return;
            }

            // 죽은 적 수와 현재 웨이브의 적 수가 불일치 or 웨이브 리스트가 비어있음 or 모든 웨이브 클리어
            if (_enemyDieCount != _enemyCount || _enemyWaveList.Count == 0 || _isAllWaveClear)
                return;

            // 웨이브 시작
            _enemyCount = _enemyWaveList[_currentWaveIndex].enemySpawnInfoList.Count;
            _enemyDieCount = 0;

            if (_currentWaveIndex < _enemyWaveList.Count)
            {
                for(int i=0; i< _enemyWaveList[_currentWaveIndex].enemySpawnInfoList.Count; i++)
                {
                    if (_enemyWaveList[_currentWaveIndex].enemySpawnInfoList[i].EnemyPrefab == null)
                    {
                        Debug.LogError($"적 웨이브 {_currentWaveIndex}의 {i}번째 적 정보가 설정되지 않았음!");
                        return;
                    }

                    int idx = Random.Range(0, _possibleSpawnPositionList.Count);
                    Instantiate(
                        _enemyWaveList[_currentWaveIndex].enemySpawnInfoList[i].EnemyPrefab,
                        _possibleSpawnPositionList[idx],
                        Quaternion.identity,
                        transform
                    );
                }
                _currentWaveIndex++;
            }
        }

        //// 적 소환
        //public void SpawnEnemy()
        //{
        //    // 임의의 타일에 enemyCount 숫자만큼 적 생성
        //    for (_currentSpawnCount = 0; _currentSpawnCount < _spawnCount; _currentSpawnCount++)
        //    {
        //        int index = Random.Range(0, _possibleSpawnPositionList.Count);
        //        GameObject enemyPrefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
        //        GameObject enemyInstance = Instantiate(enemyPrefab, _possibleSpawnPositionList[index], Quaternion.identity, transform);
        //        _spawnedEnemyList.Add(enemyInstance);
        //    }
        //}

        //public void SpawnBoss()
        //{
        //    _currentSpawnCount = 0;
        //    GameObject bossGO = StageManager.Instance.CurrentRoom.GetComponentInChildren<BossFSM>(true).gameObject;
        //    _spawnedEnemyList.Add(bossGO);
        //    _currentSpawnCount++;
        //    bossGO.SetActive(true);
        //}

        // 소환 가능한 타일 계산
        void CalculateSpawnPossibleTiles()
        {
            // 타일맵의 위치, 크기, min, center, max 위치 정보를 가지고 있는 BoundsInt 객체를 가져옴
            BoundsInt bounds = _spawnAreaTilemap.cellBounds;

            // 타일맵 내부 모든 타일의 정보를 불러와 allTiles 배열에 저장
            TileBase[] allTiles = _spawnAreaTilemap.GetTilesBlock(bounds);

            // 외곽 라인을 제외한 모든 타일 검사(Background에서 벽이 배치된 부분은 비워놓았기 때문)
            for (int y = 1; y < bounds.size.y - 1; y++)
            {
                for (int x = 1; x < bounds.size.x - 1; x++)
                {
                    // [y * bounds.size.x + x]번째 방의 타일 정보를 불러옴
                    TileBase tile = allTiles[y * bounds.size.x + x];

                    // 해당 타일이 비어있지 않으면 적 배치 가능 타일로 판단
                    if (tile != null)
                    {
                        Vector3Int localPosition = bounds.position + new Vector3Int(x, y);
                        Vector3 position = _spawnAreaTilemap.CellToWorld(localPosition) + _offset;
                        position.z = 0;
                        _possibleSpawnPositionList.Add(position);
                    }
                }
            }
        }

        //// TODO: 현재는 Room 클래스에서 Update로 확인하고 있는데, vertical slice 이후에 몬스터가 죽었을 때, action 호출하는 식으로 변경해야 함
        //// 소환된 적 전부 죽였는지 여부 반환
        //public void IsClear()
        //{
        //    int dieEnemyCount = 0; // 죽은 적 수
        //    foreach (var enemy in _spawnedEnemyList)
        //    {
        //        if (enemy == null)
        //        {
        //            dieEnemyCount++;
        //        }
        //    }

        //    if(dieEnemyCount == _spawnCount)
        //    {
        //        _spawnedEnemyList.Clear(); // 소환된 적 리스트 초기화
        //        _possibleSpawnPositionList.Clear();
        //        _currentSpawnCount = 0; // 현재 소환된 적 수 초기화
        //        StageManager.Instance.CurrentRoom.RoomClearComplete(); // 방 클리어
        //    }
        //}

        void OnDestroy()
        {
            _instance = null;
        }
    }
}