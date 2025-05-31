using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance => _instance;

    public Camera MainCamera { get; private set; }

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
    }

    public List<GameObject> MagicItems = new();
    public PlayerController player;

    void Start()
    {
        MagicItems = Resources.LoadAll<GameObject>("Item").ToList();
    }

    public void L_DelayDown() 
    {
        if (player)
        {
            player.GetComponent<MagicHand>().L_Delay -= 0.05f;
        }
        else 
        {
            print("플레이어 없음");
        }
    }

    public void R_DelayDown()
    {
        if (player)
        {
            player.GetComponent<MagicHand>().R_Delay -= 0.05f;
        }
        else
        {
            print("플레이어 없음");
        }
    }

    public void StatUp_MaxHealth() 
    {
        float addValue = 10f;
        player.GetComponent<PlayerStatus>().MaxHealth += (int)addValue;
        player.GetComponent<PlayerStatus>().Health += addValue;
    }
    public void StatUp_MoveSpeed() 
    {
        float addValue = 2f;
        player.GetComponent<PlayerStatus>().MoveSpeed += (int)addValue;
    }

    public void StatUp_L_Cooltime() 
    {
        float minusValue = 0.05f;
        player.GetComponent<MagicHand>().L_CoolTime -= minusValue;
    }

    public void StatUp_R_Cooltime() 
    {
        float minusValue = 0.05f;
        player.GetComponent<MagicHand>().R_CoolTime -= minusValue;
    }
}