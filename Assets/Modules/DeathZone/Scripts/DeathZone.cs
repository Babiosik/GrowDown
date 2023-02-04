using Modules.Roots.Scripts;
using UnityEngine;

namespace Modules.DeathZone.Scripts
{
    public class DeathZone : MonoBehaviour
    {
        private const string RootHeadTag = "RootHead";
        
        [SerializeField] private bool _randomRotation;
        [SerializeField] private bool _randomHalfRotation;
        [SerializeField] private float _randomHalfRotationSize;
        [SerializeField] private bool _randomScale;
        [SerializeField] private Vector2 _randomScaleSize;

        private void Start()
        {
            if (_randomRotation)
                transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            if (_randomHalfRotation)
                transform.rotation = Quaternion.Euler(0, 0, Random.Range(-_randomHalfRotationSize, _randomHalfRotationSize));
            if (_randomScale)
                transform.localScale = Vector3.one * Random.Range(_randomScaleSize.x, _randomScaleSize.y);
            
            transform.position += Vector3.forward * 0.0001f;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(RootHeadTag)) return;
            
            if (other.transform.parent.TryGetComponent(out RootHead rootHead))
            {
                rootHead.Die();
            }
        }
    }
}