using System;
using Modules.Services;
using Modules.Singletones.Factories;
using UnityEngine;

namespace Modules.Roots.Scripts
{
    public class RootSegment : MonoBehaviour, IRootSegment
    {
        public static event Action<RootSegment> OnClick;

        [SerializeField] private float _speed;
        [SerializeField] private float _waterEat = 0.1f;

        private RootHead _rootHead;
        private Vector3 _endPoint;
        private float _percentGross;
        private RootSegmentMesh _rootSegmentMesh;
        private AudioSource _audioSource;

        private IRootSegment _prevSegment;
        private IRootSegment _nextSegment;

        private bool _isPause = true;
        public bool IsDied { get; private set; } = false;
        public event Action OnDie;

        private void OnMouseDown()
        {
            if (IsDied || !ResourcesService.IsCanStartRoot) return;
            OnClick?.Invoke(this);
        }
        
        private void OnEnable()
        {
            if (_rootSegmentMesh != null)
                _rootSegmentMesh.OnClick += OnMeshClick;
        }

        private void OnDisable()
        {
            if (_rootSegmentMesh != null)
                _rootSegmentMesh.OnClick -= OnMeshClick;
        }

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
            _audioSource = GetComponent<AudioSource>();
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
            SetPauseGross(false);
        }

        public void SetMesh(RootSegmentMesh mesh)
        {
            _rootSegmentMesh = mesh;
            _rootSegmentMesh.OnClick += OnMeshClick;
        }

        public void SetPauseGross(bool pause)
        {
            _isPause = pause || IsDied;
            if (_isPause)
                _audioSource.Stop();
            else
                _audioSource.Play();
        }

        [Obsolete("Obsolete")]
        public void Die()
        {
            SetPauseGross(true);
            IsDied = true;
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
            OnDie?.Invoke();
        }

        private void Clone()
        {
            SetPauseGross(true);
            
            RootSegment next = RootFactory.CreateRootSegment();
            RootFactory.CreateRootSegmentMesh(next);
            
            next.Init(this, _rootHead, _endPoint, transform.rotation);
            _nextSegment = next;
        }

        private void OnMeshClick() =>
            OnClick?.Invoke(this);
    }
}