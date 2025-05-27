using UnityEngine;

public class StatUp_MaxMana : StatText
{
    void Start()
    {
        UIManager.Instance.inventoryStat_MaxMana = this;
    }
}
