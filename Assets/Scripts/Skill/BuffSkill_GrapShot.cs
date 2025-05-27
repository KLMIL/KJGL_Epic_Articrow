using UnityEngine;

public class BuffSkill_GrapShot : BuffSkill
{
    Transform fireMan;
    public float grabForce = 1f;

    public override void BuffShot(Transform fireMan, GameObject magicObj)
    {
        base.BuffShot(fireMan, magicObj);
        this.fireMan = fireMan;
        magicObj.GetComponent<Magic>().A_CollisionEvent += Grab;
    }

    void Grab(Collider2D collision)
    {
        print("그랩!");
        if(collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb2d))
        {
            rb2d.AddForce( ( (Vector2)fireMan.position - rb2d.position) * grabForce, ForceMode2D.Impulse );
        }
    }
}
