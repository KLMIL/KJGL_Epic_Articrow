using UnityEngine;

public class StatUp_R_CoolTime : StatText
{
    void Start()
    {
        UIManager.Instance.InventoryStat_R_CoolTime = this;
    }
}
