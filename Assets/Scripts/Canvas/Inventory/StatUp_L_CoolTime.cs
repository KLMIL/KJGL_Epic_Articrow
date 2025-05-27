using UnityEngine;

public class StatUp_L_CoolTime : StatText
{
    private void Start()
    {
        UIManager.Instance.inventoryStat_L_CoolTime = this;
    }
}
