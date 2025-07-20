using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using YSJ;

namespace BMC
{
    public class InteractWindow : MonoBehaviour
    {
        InputSystemActions _inputSystemActions;
        SpriteRenderer _sprite;
        TextMeshPro _text;

        void Awake()
        {
            _sprite = GetComponentInChildren<SpriteRenderer>();
            _text = GetComponentInChildren<TextMeshPro>();
            _inputSystemActions = Managers.Input.InputSystemActions;
            HideWindow();
        }

        public void ShowWindow()
        {
            _sprite.enabled = true;
            _text.enabled = true;

            if (Managers.Scene.CurrentScene.SceneType != Define.SceneType.TutorialScene)
            {
                var interactAction = _inputSystemActions.Player.Interact;
                for (int i = 0; i < interactAction.bindings.Count; i++)
                {
                    if (interactAction.bindings[i].isComposite)
                        continue;

                    // 키보드, 마우스 등의 다양한 입력 방식에 대응
                    _text.text = interactAction.GetBindingDisplayString(i);
                    break;
                }
            }
        }

        public void HideWindow()
        {
            _sprite.enabled = false;
            _text.enabled = false;
            _text.text = "F";
        }
    }
}