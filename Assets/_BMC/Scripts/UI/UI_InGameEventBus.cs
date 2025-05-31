using System;
using UnityEngine;

namespace BMC
{
    public class UI_InGameEventBus : MonoBehaviour
    {
        public static Action OnToggleChoiceRoomCanvas;
        public static Action<float> OnHpSliderValueUpdate;
    }
}