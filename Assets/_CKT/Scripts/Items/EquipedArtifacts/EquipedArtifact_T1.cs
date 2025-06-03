using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    public class EquipedArtifact_T1 : EquipedArtifact
    {
        private void Awake()
        {
            base.Init("FieldArtifacts/FieldArtifact_T1", "Bullet");
        }

        private void Start()
        {
            base.CheckWhichHand();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                base.ThrowAway();
            }
        }
    }
}