using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance => _instance;

    public Camera MainCamera { get; private set; }

    public CKT.Inventory Inventory { get; private set; } = new CKT.Inventory();

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

        MainCamera = Camera.main;
        Inventory.Init();
    }

    public List<GameObject> MagicItems = new();

    void Start()
    {
    }
}