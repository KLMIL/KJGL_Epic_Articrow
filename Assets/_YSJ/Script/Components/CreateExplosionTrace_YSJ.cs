using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateExplosionTrace_YSJ : MonoBehaviour
{
    List<GameObject> Traces;

    private void Awake()
    {
        Traces = Resources.LoadAll<GameObject>("ExplosionTraces").ToList();
    }

    void Start()
    {
        Instantiate(Traces[Random.Range(0, Traces.Count)], transform.position, Quaternion.identity);
    }
}
