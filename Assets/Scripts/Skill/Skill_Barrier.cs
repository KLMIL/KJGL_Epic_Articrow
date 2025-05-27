using UnityEngine;

public class Skill_Barrier : MagicSkill
{
    public void Awake()
    {
        Init();

        magicPrefab = Resources.Load<GameObject>("Magic/Barrier");
        if (magicPrefab == null)
        {
            Debug.LogError("배리어 프리팹 찾을 수 없음");
        }
    }

    public void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }
}
