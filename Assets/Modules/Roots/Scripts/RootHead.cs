using Modules.Core;
using UnityEngine;

namespace Modules.Roots.Scripts
{
    public class RootHead : MonoBehaviour
    {
        [SerializeField] private Transform _meshTransform;
        private RootSegment _currentSegment;
        private Vector3 _localMeshPosition;
        private Vector3 _start;
        private Vector3 _end;

        public bool IsDied { get; private set; } = false;
        public RootSegment GetCurrentSegment => _currentSegment;

        public void SetSegment(RootSegment segment, Vector3 start, Vector3 end, float rotation)
        {
            _currentSegment = segment;
            _localMeshPosition = _meshTransform.localPosition;
            _start = start;
            _end = end;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
        }
        
        public void Move(float percent, float offsetY)
        {
            transform.position = Vector3.Lerp(_start, _end, percent);
            _localMeshPosition.y = Mathf.Lerp(_localMeshPosition.y, offsetY, 0.5f);
            _meshTransform.localPosition = _localMeshPosition;
        }

        public void SetPauseGross(bool pause) =>
            _currentSegment.SetPauseGross(pause);
        
        public void Die()
        {
            IsDied = true;
            _currentSegment.Die();
            AliveService.Die(this);
        }
    }
}