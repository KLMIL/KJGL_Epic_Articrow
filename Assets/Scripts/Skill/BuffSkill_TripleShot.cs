using UnityEngine;

public class BuffSkill_TripleShot : BuffSkill
{
    public float addAccuracy = 0.1f;
    public int addFireCount = 2;
    public override void Buff(MagicSkill magicSkill)
    {
        base.Buff(magicSkill);
        magicSkill.addFireCount += addFireCount;
        magicSkill.addAccuracy += addAccuracy;
    }
}
