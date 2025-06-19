using System.Collections;
using UnityEngine;

namespace CKT
{
    public class Bullet_T4 : Projectile
    {   
        Pellet[] _pellets;
        float _moveXSpeed = 3f;

        private new void OnEnable()
        {
            base.OnEnable();

            StartCoroutine(PelletEnableCoroutine());
        }

        private new void OnDisable()
        {
            base.OnDisable();

            _pellets = _pellets ?? GetComponentsInChildren<Pellet>();
            for (int i = 0; i < _pellets.Length; i++)
            {
                _pellets[i].transform.localPosition = Vector3.zero;
            }
        }

        IEnumerator PelletEnableCoroutine()
        {
            yield return null;

            _pellets = _pellets ?? GetComponentsInChildren<Pellet>();
            float _deltaSpeed = (_moveXSpeed * 2) / (_pellets.Length - 1);

            for (int i = 0; i < _pellets.Length; i++)
            {
                _pellets[i].gameObject.SetActive(true);
                _pellets[i].ScatterSpeed = _moveXSpeed - (_deltaSpeed * i);
                _pellets[i].SkillManager = base.SkillManager;
            }
        }
    }
}