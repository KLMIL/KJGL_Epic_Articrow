using System.Collections;
using UnityEngine;

public class TraceSpawner : MonoBehaviour
{
    Coroutine _spawnCoroutine;

    void Start()
    {
        _spawnCoroutine = _spawnCoroutine ?? StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        while (this.gameObject.activeSelf)
        {
            Debug.Log("흔적 생성");
            GameObject trace = YSJ.Managers.Pool.InstPrefab("Trace", this.transform, this.transform.position);

            yield return new WaitForSeconds(0.2f);
        }
    }
}
