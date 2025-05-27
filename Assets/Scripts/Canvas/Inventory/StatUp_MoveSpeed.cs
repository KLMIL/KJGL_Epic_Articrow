using UnityEngine;

public class StatUp_MoveSpeed : StatText
{
    void Start()
    {
        UIManager.Instance.inventoryStat_MoveSpeed = this;
    }
}
