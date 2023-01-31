using Modules.Singletones.Factories;
using UnityEngine;

namespace Modules.Roots.Scripts
{
    public class RootSegment : MonoBehaviour
    {
        // [SerializeField] private GameObject[] _rootSegmentMeshPrefab;
        // [SerializeField] private GameObject _rootSegmentPrefab;
        [SerializeField] private float _speed;

        private RootHead _rootHead;
        private Vector3 _endPoint;
        private float _percentGross;
        private RootSegmentMesh _rootSegmentMesh;

        private RootSegment _prevSegment;
        private RootSegment _nextSegment;

        private bool _isPause = true;
        
        public bool IsDied { get; private set; } = false;

        public Vector3 GetHeadPosition => _rootHead.transform.GetChild(0).position;

        private void Start()
        {
            // _rootSegmentPrefab.name = _rootSegmentPrefab.name.Replace("(Clone)", "");
        }

        private void Update()
        {
            if (_isPause)
                return;

            _percentGross += _speed * Time.deltaTime;
            _rootSegmentMesh.UpdateGross(_percentGross);
            _rootHead.Move(_percentGross, _rootSegmentMesh.GetHeadPositionOffset);

            if (_percentGross >= 1) Clone();
        }

        public void Init(RootSegment prev, RootHead head, Vector3 position, Quaternion rotation)
        {
            _prevSegment = prev;
            transform.position = position;
            transform.rotation = rotation;
            _endPoint = transform.position + transform.right * _rootSegmentMesh.Size.x;

            _rootHead = head;
            _rootHead.SetSegment(
                this,
                transform.position,
                _endPoint,
                transform.rotation.eulerAngles.z
            );
            
            _percentGross = 0;
            _isPause = false;
        }

        public void SetMesh(RootSegmentMesh mesh) =>
            _rootSegmentMesh = mesh;

        public void SetPauseGross(bool pause) =>
            _isPause = pause || IsDied;
        
        public void Die()
        {
            _isPause = true;
            IsDied = true;
            if (_prevSegment != null)
                _prevSegment.Die();
        }

        private void Clone()
        {
            _isPause = true;
            _nextSegment = RootFactory.CreateRootSegment();
            RootFactory.CreateRootSegmentMesh(_nextSegment);
            
            _nextSegment.Init(this, _rootHead, _endPoint, transform.rotation);
        }
    }
}