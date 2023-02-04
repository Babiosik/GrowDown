using System;
using Modules.Services;
using Modules.Singletones.Factories;
using UnityEngine;

namespace Modules.Roots.Scripts
{
    public class RootSegment : MonoBehaviour, IRootSegment
    {
        // [SerializeField] private GameObject[] _rootSegmentMeshPrefab;
        // [SerializeField] private GameObject _rootSegmentPrefab;
        [SerializeField] private float _speed;
        [SerializeField] private float _waterEat = 0.1f;

        private RootHead _rootHead;
        private Vector3 _endPoint;
        private float _percentGross;
        private RootSegmentMesh _rootSegmentMesh;

        private IRootSegment _prevSegment;
        private IRootSegment _nextSegment;

        private bool _isPause = true;
        public bool IsDied { get; private set; } = false;
        public event Action OnDie;

        private void Update()
        {
            if (_isPause)
                return;

            _percentGross += _speed * Time.deltaTime;
            _rootSegmentMesh.UpdateGross(_percentGross);
            _rootHead.Move(_percentGross, _rootSegmentMesh.GetHeadPositionOffset);
            
            ResourcesService.Water.Value -= _waterEat * (AliveService.GetCurrentLevel + 1) * Time.deltaTime;
            if (ResourcesService.Water.Value <= 0)
            {
                _rootHead.Die();
                return;
            }

            if (_percentGross >= 1) Clone();
        }

        public void Init(IRootSegment prev, RootHead head, Vector3 position, Quaternion rotation)
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

        [Obsolete("Obsolete")]
        public void Die()
        {
            _isPause = true;
            IsDied = true;
            OnDie?.Invoke();
            _rootSegmentMesh
                .Die()
                .GetAwaiter()
                .OnCompleted(() =>
                {
                    if (_prevSegment == null)
                        AliveService.Die(_rootHead);
                    else
                        _prevSegment?.Die();
                });
        }

        private void Clone()
        {
            _isPause = true;
            
            RootSegment next = RootFactory.CreateRootSegment();
            RootFactory.CreateRootSegmentMesh(next);
            
            next.Init(this, _rootHead, _endPoint, transform.rotation);
            _nextSegment = next;
        }
    }
}