using UnityEngine;

public class Item_TripleShot : Item
{
    GameObject LinkedSkill;
    public override GameObject GetSkillObject()
    {
        return LinkedSkill;
    }

    private void Awake()
    {
        LinkedSkill = Resources.Load<GameObject>("Skill/BuffSkill_TripleShot");
    }
}
