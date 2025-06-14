using UnityEngine;
using YSJ;

public class HealItem : MonoBehaviour
{
    [SerializeField] float _healAmount = 35f;

    public void Heal(PlayerStatus player)
    {
        player.RecoverHealth(_healAmount);
        Destroy(gameObject);
    }
}
