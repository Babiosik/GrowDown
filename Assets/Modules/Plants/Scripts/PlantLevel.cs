using UnityEngine;

namespace Modules.Plants.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlantLevel : MonoBehaviour
    {
        [SerializeField] private Sprite _dead;

        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Die() =>
            _spriteRenderer.sprite = _dead;
    }
}