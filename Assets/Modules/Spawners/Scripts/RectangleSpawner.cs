using UnityEngine;

namespace Modules.Spawners.Scripts
{
    public class RectangleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _spawnable;
        [SerializeField] private Vector2 _size;
        [SerializeField] private int _count;

        private void Start()
        {
            if (_spawnable.Length == 0) return;
            
            for (var i = 0; i < _count; i++)
                Spawn();
        }
        private void Spawn()
        {
            float x = (Random.value * 2 - 1) * _size.x;
            float y = (Random.value * 2 - 1) * _size.y;

            var local = new Vector3(x, y, 0);
            Vector3 world = transform.position + local;

            int rand = Random.Range(0, _spawnable.Length);
            Instantiate(_spawnable[rand], world, Quaternion.identity);
        }
    }
}