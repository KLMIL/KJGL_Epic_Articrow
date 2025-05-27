using UnityEngine;

public class Mana : MonoBehaviour
{
    public float maxMana;
    public float currentMana;
    public float ManaRecovery;
    void Start()
    {
        currentMana = maxMana;
    }

    private void Update()
    {
        IncreaseMana(ManaRecovery * Time.deltaTime);
        UIManager.Instance.manaBar.ManabarUpdate(currentMana / maxMana);
    }

    public bool DecreaseMana(float value) 
    {
        if (currentMana < value) 
        {
            return false;
        }
        currentMana -= value;
        return true;
    }

    public void IncreaseMana(float value)
    {
        currentMana += value;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
    }
}
