using UnityEngine;

namespace BMC
{
    /// <summary>
    /// ProjectSetting에서 변경해도 되지만, 크기랑 화질 변경이 번거로워서 코드로 제어하게 함
    /// </summary>
    public class CursorController : MonoBehaviour
    {
        private void Start()
        {
            Cursor.visible = false;
        }

        void Update()
        {
            Vector3 pos = Input.mousePosition;
            transform.position = pos;
        }
    }
}