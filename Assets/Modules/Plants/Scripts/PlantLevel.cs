using UnityEngine;

namespace Modules.Plants.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlantLevel : MonoBehaviour
    {
        [SerializeField] private Sprite _dead;
        private Animator _animator;

        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        public void Die()
        {
            if (_animator != null)
                _animator.gameObject.SetActive(false);
            _spriteRenderer.sprite = _dead;
        }
    }
}