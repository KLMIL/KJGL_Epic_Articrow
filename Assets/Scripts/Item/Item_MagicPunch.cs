using UnityEngine;

public class Item_MagicPunch : Item
{
    GameObject LinkedSkill;
    public override GameObject GetSkillObject()
    {
        return LinkedSkill;
    }
    private void Awake()
    {
        LinkedSkill = Resources.Load<GameObject>("Skill/Skill_MagicPunch");
    }

}
