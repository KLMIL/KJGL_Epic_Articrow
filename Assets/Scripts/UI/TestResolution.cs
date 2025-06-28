using UnityEngine;

public class TestResolution : MonoBehaviour
{
    void Start()
    {
        // 해상도 설정 함수 호출
        SetTestResolution();

        // 카메라의 Viewport Rect를 조절해서 해상도 비율이 달라도 동일한 씬 영역을 보여주도록 하려고 함
        // Camera.rect로 출력 영역을 조절하게 되면 씬을 전체 화면이 아닌 일부 영역만 그리게 되기 때문에 나머지 영역은 초기화되지 않고, 이전 프레임의 화면이 남는 경우가 발생함
        // 이는 Unity가 Clear Flags 설정에 따라 카메라의 클리어 방식을 처리하기 때문임
        // 따라서 카메라가 렌더하지 않는 바깥 영역은 명시적으로 색으로 지운다.
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = Color.black;
    }

    /* 해상도 설정하는 함수 */
    public void SetTestResolution()
    {
        int setWidth = 1920; // 사용자 설정 너비
        int setHeight = 1080; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }
}