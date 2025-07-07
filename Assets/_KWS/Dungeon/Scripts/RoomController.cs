using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Dungeon
{
    /// <summary>
    /// 웨이브 순서에 따라 몬스터 활성화
    /// 살아 있는 몬스터 개수 카운트 등
    /// </summary>
    public class RoomController : MonoBehaviour
    {
        [SerializeField] List<GameObject> _enemyWaves;

        int _waveIndex = 0;
        int _enemyCount = 0;


        private void Start()
        {
            StartWave(_waveIndex);
        }

        private void StartWave(int index)
        {
            // 모든 Wave 클리어
            if (index >= _enemyWaves.Count)
            {
                OnRoomCleared();
                return;
            }

            // 이번 웨이브 몬스터 활성화
            foreach (Transform enemy in _enemyWaves[index].transform)
            {
                // enemy의 사망 이벤트 구독
                enemy.gameObject.SetActive(true);

            }
            // 현재 몬스터 수 탐색
            _enemyCount = _enemyWaves[index].transform.childCount;
        }

        private void OnRoomCleared()
        {
            // 문 열고 보상 지급
        }
    }
}
