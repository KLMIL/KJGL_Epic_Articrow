using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BMC
{
    public enum KeyAction
    {
        Move,
        Interact,
        Inventroy,
        LeftHand,
        RightHand,
        Roll,
    }

    // 키 재지정 시스템 클래스
    public class KeyRebindSystem : MonoBehaviour
    {
        Dictionary<KeyAction, RebindActionUI> _rebindActionDict = new Dictionary<KeyAction, RebindActionUI>();
        public Dictionary<KeyAction, RebindActionUI> RebindActionDict => _rebindActionDict;

        RebindActionUI[] _rebindActionUIs;

        void Awake()
        {
            _rebindActionUIs = GetComponentsInChildren<RebindActionUI>();
        }

        // 모든 키 액션 초기화
        void ResetToDefault()
        {
            foreach (RebindActionUI rebindActionUI in _rebindActionUIs)
            {
                if (rebindActionUI != null)
                {
                    rebindActionUI.ResetToDefault();
                }
            }

            //foreach (KeyValuePair<KeyAction, RebindActionUI> rebindAction in _rebindActionDict)
            //{
            //    rebindAction.Value.ResetToDefault();
            //}
        }
    }
}