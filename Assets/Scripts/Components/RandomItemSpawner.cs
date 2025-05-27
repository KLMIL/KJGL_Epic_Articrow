using System.Collections;
using UnityEngine;

public class RandomItemSpawner : MonoBehaviour
{
    public float spawnCoolTime = 1f;
    Coroutine spawnCoroutine;
    void Start()
    {
    }

    void Update()
    {
        if (transform.childCount == 0 && spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(WaitCoolTime());
        }
    }

    IEnumerator WaitCoolTime() 
    {
        yield return new WaitForSeconds(spawnCoolTime);
        GameObject spawnedObj = Instantiate(GameManager.Instance.MagicItems[Random.Range(0, GameManager.Instance.MagicItems.Count)]);
        spawnedObj.transform.SetParent(transform, true);
        spawnedObj.transform.localPosition = new Vector3(0, 1, 0);
        spawnCoroutine = null;
    }
}
