using UnityEngine;

public class MagicRoom : Room
{
    Door[] doors;

    void Awake()
    {
        doors = GetComponentsInChildren<Door>();
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        foreach (var door in doors)
        {
            door.Open();
        }
    }
}