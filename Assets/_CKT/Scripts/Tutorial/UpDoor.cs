using System.Collections;
using UnityEngine;

namespace CKT
{
    public class UpDoor : MonoBehaviour
    {
        Animator _animator;
        SpriteRenderer _renderer;

        bool _isOpen = false;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _renderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Update()
        {
            if (_isOpen && (_renderer.color.a > 0))
            {
                Color color = _renderer.color;
                color.a -= Time.deltaTime;
                _renderer.color = color;
            }
        }

        public void OpenDoor()
        {
            _animator.Play("Open");
            _isOpen = true;
        }
    }
}