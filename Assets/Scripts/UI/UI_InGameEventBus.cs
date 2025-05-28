using System;
using UnityEngine;

public class UI_InGameEventBus : MonoBehaviour
{
    public static Action OnPaused;                   // 일시 정지
    public static Action OnChangedMana;
    public static Action OnChangedHealth;
    public static Action OnChangedTwoHand;
}