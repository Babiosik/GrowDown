using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
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
        private List<RootJoint> _middleJoints = new List<RootJoint>();

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
                _rootHead.Die(this);
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

        public void Die(IRootSegment from)
        {
            if (from == null)
                SetPauseGross(true);
            else if (_nextSegment is not { IsDied: true })
                return;

            IsDied = _middleJoints.All(joint => joint.IsDied);
            OnDie?.Invoke();
            
            if (IsDied)
            {
                DieSegmentMeshAnimation(0);
                return;
            }

            float dieTo = _middleJoints.Aggregate<RootJoint, float>(0, GetMax);
            DieSegmentMeshAnimation(dieTo);
        }

        public void AddMiddlePoint(RootJoint joint)
        {
            _middleJoints.Add(joint);
            joint.transform.SetParent(transform);
        }

        public void Rotate(RootJoint joint) =>
            _nextSegment = joint;

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

        private void DieSegmentMeshAnimation(float to)
        {
            UniTask dieAnimation = _rootSegmentMesh.Die(to);
            if (to != 0) return;
            dieAnimation.GetAwaiter()
                .OnCompleted(() =>
                {
                    if (_prevSegment == null)
                        AliveService.DieZeroSegment();
                    else
                        _prevSegment?.Die(this);
                });
        }
        
        private float GetMax(float x, RootJoint joint) =>
            joint.IsDied ? x : Mathf.Max(x, joint.transform.localPosition.x);
    }
}