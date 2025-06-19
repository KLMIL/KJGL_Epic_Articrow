using System.Collections;
using UnityEngine;

namespace CKT
{
    public class Bullet_T4 : Projectile
    {   
        Pellet[] _pellets;
        float _moveXSpeed = 6f;

        public override void Init(bool isCreateFromPlayer)
        {
            base.Init(isCreateFromPlayer);

            _pellets = _pellets ?? GetComponentsInChildren<Pellet>();
            float _deltaSpeed = (_moveXSpeed * 2) / (_pellets.Length - 1);

            for (int i = 0; i < _pellets.Length; i++)
            {
                _pellets[i].gameObject.SetActive(true);
                _pellets[i].ScatterSpeed = _moveXSpeed - (_deltaSpeed * i);
                _pellets[i].Init(base._isCreateFromPlayer);
            }

            StartCoroutine(PelletEnableCoroutine());
        }  

        private new void OnDisable()
        {
            base.OnDisable();

            _pellets = _pellets ?? GetComponentsInChildren<Pellet>();
            for (int i = 0; i < _pellets.Length; i++)
            {
                _pellets[i].transform.localPosition = Vector3.zero;
                _pellets[i].gameObject.SetActive(false);
            }
        }

        IEnumerator PelletEnableCoroutine()
        {
            yield return null;


        }
    }
}