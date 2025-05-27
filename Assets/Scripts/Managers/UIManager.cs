using UnityEngine;

public class UIManager : MonoBehaviour
{
    static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType(typeof(UIManager)) as UIManager;
                if (_instance == null)
                {
                    Debug.LogError("UIManager없음!");
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

    public Transform LeftHand;
    public Transform RightHand;
    public LeftHandCoolTime leftHandCoolTime;
    public RightHandCoolTime rightHandCoolTime;
    public Inventory inventory;

    public Stat_ManaBar manaBar;
    public Stat_HealthBar HealthBar;

    public StatUp_MaxHealth inventoryStat_MaxHealth;
    public StatUp_MaxMana inventoryStat_MaxMana;
    public StatUp_ManaRecovery inventoryStat_ManaRecovery;
    public StatUp_MoveSpeed inventoryStat_MoveSpeed;
    public StatUp_L_CoolTime inventoryStat_L_CoolTime;
    public StatUp_R_CoolTime InventoryStat_R_CoolTime;

    public Canvas_Restart CanvasRestart;
}
