using UnityEngine;

public class StatUp_ManaRecovery : StatText
{
    void Start()
    {
        UIManager.Instance.inventoryStat_ManaRecovery = this;
    }
}
