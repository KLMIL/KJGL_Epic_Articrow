using System;
using UnityEngine;

namespace BMC
{
    [Serializable]
    public class EnemySpawnInfo
    {
        public GameObject EnemyPrefab;  // 소환할 적 프리팹
        public Vector2 SpawnPosition;   // 소환 위치
        public float SpawnDelay;        // 소환 지연 시간
    }
}