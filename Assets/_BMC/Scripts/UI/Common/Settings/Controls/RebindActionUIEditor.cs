#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BMC
{
    // RebindActionUI를 인스펙터에 보여줄 때, 이 클래스(RebindActionUIEditor)를 사용해서 커스텀 인스펙터를 그려 달라고 지시하는 어트리뷰트
    [CustomEditor(typeof(RebindActionUI))]
    public class RebindActionUIEditor : UnityEditor.Editor
    {
        // SerializedPropertysms RebindActionUI 컴포넌트가 가진 private 필드에 대응
        private SerializedProperty _inputSystemActionProperty;
        private SerializedProperty _bindingIdProperty;
        private SerializedProperty _actionLabelProperty;
        private SerializedProperty _bindingTextProperty;
        private SerializedProperty m_RebindStartEventProperty;
        private SerializedProperty m_RebindStopEventProperty;
        private SerializedProperty m_UpdateBindingUIEventProperty;
        private SerializedProperty m_DisplayStringOptionsProperty;

        // 에디터 상에 출력할 라벨 텍스트를 보관
        // 인스펙터에 굵은 제목으로 표시되는 것들
        private GUIContent m_BindingLabel = new GUIContent("Binding");
        private GUIContent m_DisplayOptionsLabel = new GUIContent("Display Options");
        private GUIContent m_UILabel = new GUIContent("UI");
        private GUIContent m_EventsLabel = new GUIContent("Events");
        private GUIContent[] m_BindingOptions;
        private string[] m_BindingOptionValues;
        private int m_SelectedBindingOption;

        protected void OnEnable()
        {
            _inputSystemActionProperty = serializedObject.FindProperty("_inputSystemAction");
            _bindingIdProperty = serializedObject.FindProperty("_bindingId");
            _actionLabelProperty = serializedObject.FindProperty("_actionLabel");
            _bindingTextProperty = serializedObject.FindProperty("_bindingText");
            m_UpdateBindingUIEventProperty = serializedObject.FindProperty("m_UpdateBindingUIEvent");
            m_RebindStartEventProperty = serializedObject.FindProperty("m_RebindStartEvent");
            m_RebindStopEventProperty = serializedObject.FindProperty("m_RebindStopEvent");
            m_DisplayStringOptionsProperty = serializedObject.FindProperty("m_DisplayStringOptions");

            RefreshBindingOptions();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            // Binding section.
            EditorGUILayout.LabelField(m_BindingLabel, Styles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(_inputSystemActionProperty);

                var newSelectedBinding = EditorGUILayout.Popup(m_BindingLabel, m_SelectedBindingOption, m_BindingOptions);
                if (newSelectedBinding != m_SelectedBindingOption)
                {
                    var bindingId = m_BindingOptionValues[newSelectedBinding];
                    _bindingIdProperty.stringValue = bindingId;
                    m_SelectedBindingOption = newSelectedBinding;
                }

                var optionsOld = (InputBinding.DisplayStringOptions)m_DisplayStringOptionsProperty.intValue;
                var optionsNew = (InputBinding.DisplayStringOptions)EditorGUILayout.EnumFlagsField(m_DisplayOptionsLabel, optionsOld);
                if (optionsOld != optionsNew)
                    m_DisplayStringOptionsProperty.intValue = (int)optionsNew;
            }

            // UI section.
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(m_UILabel, Styles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(_actionLabelProperty);
                EditorGUILayout.PropertyField(_bindingTextProperty);
            }

            // Events section.
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(m_EventsLabel, Styles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(m_RebindStartEventProperty);
                EditorGUILayout.PropertyField(m_RebindStopEventProperty);
                EditorGUILayout.PropertyField(m_UpdateBindingUIEventProperty);
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                RefreshBindingOptions();
            }
        }

        // Composite 파트의 바인딩을 포함해서, 현재 액션에 설정된 모든 바인딩을 가져와서 업데이트
        protected void RefreshBindingOptions()
        {
            // 1. 인스펙터에서 드래그&드롭된 InputActionReference를 꺼내고, 실제 런타임의 InputAction 객체를 가져온다.
            var actionReference = (InputActionReference)_inputSystemActionProperty.objectReferenceValue;
            var action = actionReference?.action;

            // 2. 만약 InputActionReference가 비어 있거나, 액션을 가져올 수 없다면, 옵션 리스트를 빈 상태로 초기화하고 빠져나간다.
            if (action == null)
            {
                m_BindingOptions = new GUIContent[0];
                m_BindingOptionValues = new string[0];
                m_SelectedBindingOption = -1;
                return;
            }

            // 3. InputAction 객체 내부에 정의된 모든 바인딩 정보를 List 형태로 가져온다.
            //    Composite 바인딩(2D Vector)도 이 리스트에 섞여 있고, 
            //    Composite 안의 개별 파트(Up/Down/Left/Right)도 포함되어 있다.
            var bindings = action.bindings;
            var bindingCount = bindings.Count;

            // 4. 팝업 메뉴에 보여줄 문자열(GUIContent) 배열과, 각 바인딩의 GUID 문자열을 저장할 값(string) 배열을 바인딩 개수만큼 크기를 잡아 생성
            m_BindingOptions = new GUIContent[bindingCount];
            m_BindingOptionValues = new string[bindingCount];
            m_SelectedBindingOption = -1;

            // 5. 현재 인스펙터에 저장된 “_bindingId” 값(문자열 GUID)을 가져와 둔다.
            //    이 GUID와 나중에 비교하여, “현재 선택된 인덱스”를 미리 지정할 때 사용
            var currentBindingId = _bindingIdProperty.stringValue;

            // 6. 모든 바인딩을 순회하며, 각각 팝업 메뉴에 넣을 문자열과 GUID를 준비
            for (var i = 0; i < bindingCount; ++i)
            {
                // 6-1. i번째 바인딩 하나를 꺼낸다.
                var binding = bindings[i];

                // 6-2. 이 바인딩의 고유 ID(GUID)를 문자열 형태로 꺼낸다.
                //      ("{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}" 형태)
                var bindingId = binding.id.ToString();

                // 6-3. 만약 이 바인딩에 Control Scheme(예: "Keyboard", "Gamepad") 정보가 있다면, 
                //      binding.groups에 “;”로 구분된 bindingGroup 문자열이 담겨 있다.
                //      binding.groups가 비어 있지 않으면 haveBindingGroups = true가 된다.
                var haveBindingGroups = !string.IsNullOrEmpty(binding.groups);

                #region 사람이 읽기 좋게 바꾸는 부분
                // 6-4. 사람이 보기 좋은 “키 이름”(예: "W", "A", "Left Punch")을 얻기 위한 옵션 설정
                var displayOptions =
                    InputBinding.DisplayStringOptions.DontUseShortDisplayNames  // 짧은 별칭 대신 전체 이름 사용
                    | InputBinding.DisplayStringOptions.IgnoreBindingOverrides; // 오버라이드된 바인딩 대신 원본 바인딩 사용
                if (!haveBindingGroups)
                {
                    // Control Scheme이 없다면, “장치 정보(기기 이름)”도 생략하지 않도록 설정
                    displayOptions |= InputBinding.DisplayStringOptions.DontOmitDevice;
                }

                // 6-5. action.GetBindingDisplayString(...)을 호출해서, 
                //      i번째 바인딩이 실제 어떤 키/버튼에 연결됐는지를 사람이 보기 좋은 문자열로 변환
                //      예: i번째 바인딩이 “<Keyboard>/w”였다면 "W"가 되고,
                //          i번째 바인딩이 “<Gamepad>/buttonSouth”라면 "Gamepad Button South" 같은 문자열
                var displayString = action.GetBindingDisplayString(i, displayOptions);

                // 6-6. 만약 이 바인딩이 Composite 파트(Up/Down/Left/Right 등)라면, 
                //      binding.isPartOfComposite가 true임. 이때 binding.name에는 “Up” 혹은 “Left” 같은 파트 이름이 들어 있다.
                if (binding.isPartOfComposite)
                {
                    // 6-6-1. ObjectNames.NicifyVariableName(binding.name) → 
                    //        “Up” 같은 문자열을 더 예쁘게(공백 처리 등) 반환. (사실 여기선 이미 “Up” 형태라 동일)
                    // 6-6-2. displayString(예: "W") 앞에 “Up:” 를 붙여서 최종적으로 “Up: W” 형태를 만든다.
                    displayString = $"{ObjectNames.NicifyVariableName(binding.name)}: {displayString}";
                }

                // 6-7. 팝업 메뉴에서 “/”가 있으면 드롭다운 서브메뉴가 생기므로, 
                //      보기 좋게 그냥 “\”(백슬래시)로 치환해 준다.
                displayString = displayString.Replace('/', '\\');
                #endregion

                // 6-8. 만약 Control Scheme(“Keyboard” 또는 “Gamepad” 등) 정보가 있으면, 팝업 항목 끝에 괄호 형태로 덧붙여 준다.
                if (haveBindingGroups)
                {
                    // 6-8-1. 액션이 속한 ActionMap의 asset(InputActionAsset)을 가져옴
                    var asset = action.actionMap?.asset;
                    if (asset != null)
                    {
                        // 6-8-2. binding.groups 문자열(예: "Keyboard&Mouse;Gamepad")을 
                        //        세미콜론(;) 구분자로 분리해서 각각의 controlScheme 이름을 얻는다.
                        var controlSchemes = string.Join(", ",
                            binding.groups.Split(InputBinding.Separator)
                                .Select(x => asset.controlSchemes.FirstOrDefault(c => c.bindingGroup == x).name));

                        // 6-8-3. “displayString” 뒤에 “ (Keyboard, Gamepad)” 같은 형태로 덧붙인다.
                        displayString = $"{displayString} ({controlSchemes})";
                    }
                }

                // 6-9. 최종적으로 준비된 “팝업에 표시할 문자열”과 “해당 바인딩의 GUID 문자열”을 배열에 저장
                m_BindingOptions[i] = new GUIContent(displayString);
                m_BindingOptionValues[i] = bindingId;

                // 6-10. 만약 현재 인스펙터에 저장된 _bindingId(현재 선택된 GUID)와
                //        이 bindingId가 같다면, m_SelectedBindingOption을 i로 설정해서 
                //        드롭다운에서 현재 선택된 항목으로 표시되게 한다.
                if (currentBindingId == bindingId)
                    m_SelectedBindingOption = i;
            }
        }

        private static class Styles
        {
            public static GUIStyle boldLabel = new GUIStyle("MiniBoldLabel");
        }
    }
}
#endif