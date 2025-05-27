using System;
using UnityEngine;

public abstract class Magic : MonoBehaviour
{
    public Action<Collider2D> A_CollisionEvent;
    public Vector2 direction;
}
