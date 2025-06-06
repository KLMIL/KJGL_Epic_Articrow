using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private int enemyCount = 10;

    private Vector3 offset = new Vector3(0.5f, 0.5f, 0);        // 적 배치는 타일 모서리 위치이므로 타일 중앙에 배치할 수 있도록하는 offset
    private List<Vector3> possibleTiles = new List<Vector3>();  // 적배치가 가능한 모든 타일의 위치를 저장하는 리스트

    private void Awake()
    {
        // Tilemap의 Bounds 재설정 (맵을 수정했을 때 Bounds가 변경되지 않는 문제 해결)
        tilemap.CompressBounds();

        // 타일맵의 모든 타일을 대상으로 적 배치가 가능한 타일 계산
        CalculatePossibleTiles();

        // 임의의 타일에 enemyCount 숫자만큼 적 생성
        for (int i = 0; i < enemyCount; ++i)
        {
            int index = Random.Range(0, possibleTiles.Count);
            GameObject clone = Instantiate(enemyPrefab, possibleTiles[index], Quaternion.identity, transform);
            clone.GetComponent<EnemyFSM>().Setup(target);
        }
    }

    private void CalculatePossibleTiles()
    {
        // 타일맵의 위치, 크기, min, center, max 위치 정보를 가지고 있는 BoundsInt 객체를 가져옴
        BoundsInt bounds = tilemap.cellBounds;

        // 타일맵 내부 모든 타일의 정보를 불러와 allTiles 배열에 저장
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        // 외곽 라인을 제외한 모든 타일 검사(Background에서 벽이 배치된 부분은 비어있음)
        for (int y = 1; y < bounds.size.y - 1; ++y)
        {
            for (int x = 1; x < bounds.size.x - 1; ++x)
            {
                // [y * bounds.size.x + x]번째 방의 타일 정보를 불러옴
                TileBase tile = allTiles[y * bounds.size.x + x];

                // 해당 타일이 비어있지 않으면 적 배치 가능 타일로 판단
                if (tile != null)
                {
                    Vector3Int localPosition = bounds.position + new Vector3Int(x, y);
                    Vector3 position = tilemap.CellToWorld(localPosition) + offset;
                    position.z = 0;
                    possibleTiles.Add(position);
                }
            }
        }
    }
}

