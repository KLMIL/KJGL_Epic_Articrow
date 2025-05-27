using UnityEngine;

public class Item_Bomb : Item
{
    GameObject LinkedSkill;
    public override GameObject GetSkillObject()
    {
        return LinkedSkill;
    }

    private void Awake()
    {
        LinkedSkill = Resources.Load<GameObject>("Skill/Skill_Bomb");
    }
}
