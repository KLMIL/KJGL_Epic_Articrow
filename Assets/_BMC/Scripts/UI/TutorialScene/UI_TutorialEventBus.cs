using System;

namespace BMC
{
    public static class UI_TutorialEventBus
    {
        // 튜토리얼 텍스트 표시
        public static Action<int> OnTutorialText;
    }
}