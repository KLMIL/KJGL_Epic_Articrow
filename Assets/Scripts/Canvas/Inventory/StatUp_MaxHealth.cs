using TMPro;
using UnityEngine;

public class StatUp_MaxHealth : StatText
{
    private void Start()
    {
        UIManager.Instance.inventoryStat_MaxHealth = this;
    }
}
