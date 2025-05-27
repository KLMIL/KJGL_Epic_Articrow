using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType(typeof(GameManager)) as GameManager;
                if (Instance == null)
                {
                    Debug.LogError("GameManager없음!");
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

    public List<GameObject> MagicItems = new();
    public GameObject player;

    private void Start()
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
        player.GetComponent<Health>().maxHealth += addValue;
        player.GetComponent<Health>().currentHealthPoint += addValue;
    }

    public void StatUp_MaxMana() 
    {
        float addValue = 10f;
        player.GetComponent<Mana>().maxMana += addValue;
    }

    public void StatUp_ManaRecovery() 
    {
        float addValue = 1f;
        player.GetComponent<Mana>().ManaRecovery += addValue;
    }

    public void StatUp_MoveSpeed() 
    {
        float addValue = 2f;
        player.GetComponent<Move>().speed += addValue;
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

    public void RestartGame() 
    {
        SceneManager.LoadScene(0);
    }
}
