using System;
using Cysharp.Threading.Tasks;
using Modules.Services;
using UnityEngine;

namespace Modules.Roots.Scripts
{
    public class RootHead : MonoBehaviour, IRootSegment
    {
        public static event Action<RootHead> OnClick;
        
        [SerializeField] private Transform _meshTransform;
        [SerializeField] private Texture _aliveTexture;
        [SerializeField] private Texture _highlightTexture;
        [SerializeField] private Texture _diedTexture;

        private MeshRenderer _meshRenderer;
        private RootSegment _currentSegment;
        private AudioSource _audioSource;
        private Vector3 _localMeshPosition;
        private Vector3 _start;
        private Vector3 _end;
        private readonly static int MainTex = Shader.PropertyToID("_MainTex");

        public bool IsDied { get; protected set; } = false;
        public RootSegment GetCurrentSegment => _currentSegment;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _meshRenderer = _meshTransform.GetComponent<MeshRenderer>();
        }

        private void OnMouseEnter()
        {
            if (IsDied) return;
            SetTexture(_highlightTexture);
        }

        private void OnMouseExit()
        {
            if (IsDied) return;
            SetTexture(_aliveTexture);
        }

        private void OnMouseDown()
        {
            if (IsDied || !ResourcesService.IsCanChangeDirection) return;
            OnClick?.Invoke(this);
        }

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
        
        async public void Die(IRootSegment from)
        {
            IsDied = true;
            _audioSource.Play();
            await UniTask.Delay(300);
            SetTexture(_diedTexture);
            AliveService.Die(this);
            _currentSegment.Die(null);
        }
        
        private void SetTexture(Texture texture) =>
            _meshRenderer.material.SetTexture(MainTex, texture);
    }
}