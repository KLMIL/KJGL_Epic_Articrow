using System;
using UnityEngine;

public class UI_InGameEventBus : MonoBehaviour
{
    public static Action OnPauseAction;                   // 일시 정지
    //public static Action<Buff> OnAddBuffAction;           // 버프 추가될 때
    public static Action OnChangeBuffActiveTimeAction;    // 버프 시간 변경될 때
}