using UnityEngine;

namespace Modules.Spawners.Scripts
{
    public class CircleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _prefabs;
        [SerializeField] private int _count = 1;
        [SerializeField] private float _radius;

        private void Start()
        {
            if (_prefabs.Length == 0) return;
            
            for (var i = 0; i < _count; i++)
                Spawn();
        }
        private void Spawn()
        {
            float alpha = Random.Range(0, 360);
            float r = Random.value * _radius;

            float x = Mathf.Cos(alpha) * r;
            float y = Mathf.Sin(alpha) * r;

            var local = new Vector3(x, y, 0);
            Vector3 world = transform.position + local;

            Instantiate(_prefabs[Random.Range(0, _prefabs.Length)], world, Quaternion.identity, transform);
        }
    }
}