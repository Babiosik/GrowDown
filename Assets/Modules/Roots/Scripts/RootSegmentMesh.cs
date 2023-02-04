using System;
using Cysharp.Threading.Tasks;
using Modules.Services;
using UnityEngine;

namespace Modules.Roots.Scripts
{
    [RequireComponent(typeof(MeshRenderer))]
    public class RootSegmentMesh : MonoBehaviour
    {
        public event Action OnClick;
        private const float DieAnimationPerTick = 2f;
        
        [SerializeField] private AnimationCurve _pathPositionOffset;
        [SerializeField] private GameObject _diedSegment;

        private Vector2 _uv;
        private Vector3 _size;
        private Vector3 _scale;
        private Vector3 _position;
        private Material _localMaterial;
        private MeshRenderer _meshRenderer;
        private Transform _diedClone;

        public Vector3 Size => _size;
        public float GetHeadPositionOffset => _pathPositionOffset.Evaluate(_scale.x);

        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            SetClonedMaterial();
        }

        private void OnMouseDown() =>
            OnClick?.Invoke();

        public void Init()
        {
            _scale = _size = transform.localScale;
            _scale.x = 0;
            _uv = new Vector2(0, 1);
            _position = Vector3.zero;
            transform.localScale = _scale;
        }

        public void UpdateGross(float grossPercent)
        {
            grossPercent = Mathf.Clamp01(grossPercent);
            _uv.x = grossPercent;
            _scale.x = Mathf.Lerp(0, _size.x, grossPercent);
            _position.x = _scale.x / 2;
            if (_localMaterial != null)
                _localMaterial.mainTextureScale = _uv;
            transform.localScale = _scale;
            transform.localPosition = _position;
        }

        async public UniTask Die(float to)
        {
            if (_diedClone == null)
            {
                _diedClone = Instantiate(_diedSegment, transform.position + Vector3.forward * 0.00001f, transform.rotation, transform.parent).transform;
                _diedClone.localScale = transform.localScale;
                _diedClone.GetComponent<RootDiedSegmentMesh>().SetUv(_uv);
            }
            
            await UniTask.WaitForEndOfFrame();
            if (ResourcesService.Water.Value < 1) _uv.x = 0.000001f;
            to /= _size.x;
            while(_uv.x > to)
            {
                await UniTask.WaitForEndOfFrame();
                UpdateGross(_uv.x - DieAnimationPerTick * Time.deltaTime);
            }
            if (_uv.x <= 0)
            {
                Collider[] children = GetComponentsInChildren<Collider>();
                foreach (Collider child in children)
                    child.gameObject.SetActive(false);
            }
        }

        private void SetClonedMaterial()
        {
        #if UNITY_EDITOR
            _localMaterial = new Material(_meshRenderer.sharedMaterial);
            _meshRenderer.sharedMaterial = _localMaterial;
            _meshRenderer.material = _localMaterial;
        #else
            _localMaterial = new Material(_meshRenderer.material);
            _meshRenderer.material = _localMaterial;
        #endif
        }
    }
}