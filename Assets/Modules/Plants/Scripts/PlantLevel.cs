using Modules.Services;
using UnityEngine;

namespace Modules.Plants.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlantLevel : MonoBehaviour
    {
        [SerializeField] private float _waterPerSec;
        [SerializeField] private Sprite _dead;
        private Animator _animator;

        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!_animator.enabled) return;
            ResourcesService.Water.Value -= _waterPerSec * Time.deltaTime;
        }

        public void Die()
        {
            if (_animator != null)
                _animator.enabled = false;
            _spriteRenderer.sprite = _dead;
        }
    }
}