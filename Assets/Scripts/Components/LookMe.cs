using UnityEngine;

public class LookMe : MonoBehaviour
{
    void Start()
    {
        CameraManager.Instance.SetFollowTarget(transform);
    }
}
