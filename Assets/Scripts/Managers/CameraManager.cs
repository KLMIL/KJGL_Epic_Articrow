using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static CameraManager _instance;
    public static CameraManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType(typeof(CameraManager)) as CameraManager;
                if (Instance == null)
                {
                    Debug.LogError("CameraManager없음!");
                }
            }
            return _instance;
        }
    }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    CinemachineCamera _cinemachineCamera;

    private void Start()
    {
        _cinemachineCamera = transform.GetComponentInChildren<CinemachineCamera>();
    }

    public void SetFollowTarget(Transform target) 
    {
        _cinemachineCamera.Follow = target;
    }
}
