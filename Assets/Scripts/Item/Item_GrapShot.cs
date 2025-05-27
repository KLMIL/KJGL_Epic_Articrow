using UnityEngine;

public class Item_GrapShot : Item
{
    GameObject LinkedSkill;
    public override GameObject GetSkillObject()
    {
        return LinkedSkill;
    }
    private void Awake()
    {
        LinkedSkill = Resources.Load<GameObject>("Skill/BuffSkill_GrapShot");
    }
}
