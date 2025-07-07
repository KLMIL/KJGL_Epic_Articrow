using Game.Enemy;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace BMC
{
    public class NormalRoom : Room
    {
        [SerializeField] List<GameObject> _enemyWaves;
        int _currentWaveIndex = 0;
        int _aliveEnemyCount;

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
            //EnemySpawner.Instance.Init();
            StartWave(_currentWaveIndex);
        }

        public void StartWave(int waveIndex)
        {
            if (waveIndex < 0 || waveIndex >= _enemyWaves.Count) return;

            // 해당 웨이브 모든 몬스터 활성화
            GameObject wave = _enemyWaves[waveIndex];
            wave.SetActive(true); // 이미 활성화 상태긴 함

            foreach (Transform child in wave.transform)
            {
                child.gameObject.SetActive(true);
                //EnemyController enemy = child.GetComponent<EnemyController>();
                //if (enemy != null)
                //{
                //    _aliveEnemyCount++;
                //    enemy.OnDeath -= OnEnemyDie; // 혹시 모르니 중복 대비로 한번 빼줌
                //    enemy.OnDeath += OnEnemyDie;
                //}
            }
        }

        public void EnrollEnemy(EnemyController enemyController)
        {
            _aliveEnemyCount++;
            enemyController.OnDeath -= OnEnemyDie;
            enemyController.OnDeath += OnEnemyDie;
        }

        private void OnEnemyDie()
        {
            _aliveEnemyCount--;
            if (_aliveEnemyCount <= 0)
            {
                TryNextWave();
            }
        }

        private void TryNextWave()
        {
            _currentWaveIndex++;
            if (_currentWaveIndex < _enemyWaves.Count)
            {
                StartWave(_currentWaveIndex);
            }
            else
            {
                RoomClearComplete();
            }
        }
    }
}