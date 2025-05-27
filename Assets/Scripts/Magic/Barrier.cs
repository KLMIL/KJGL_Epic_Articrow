using System.Collections.Generic;
using UnityEngine;

public class Barrier : Magic
{
    public float maintenanceTime = 1f;
    float elapsedTime = 0f;
    void Start()
    {
        transform.position += (Vector3)direction;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > maintenanceTime) 
        {
            Destroy(gameObject);
        }
    }
}
