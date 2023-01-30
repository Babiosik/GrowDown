using UnityEngine;

namespace Modules.Roots.Scripts
{
    [RequireComponent(typeof(MeshRenderer))]
    public class RootSegmentMesh : MonoBehaviour
    {
        [SerializeField] private Transform[] _pathPoints;
        [SerializeField] private AnimationCurve _pathPositionOffset;

        private Vector2 _uv;
        private Vector3 _size;
        private Vector3 _scale;
        private Vector3 _position;
        private Material _localMaterial;
        private MeshRenderer _meshRenderer;

        public Vector3 Size => _size;
        public float GetHeadPositionOffset => _pathPositionOffset.Evaluate(_scale.x);

        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            SetClonedMaterial();
        }

        public void Init()
        {
            _scale = _size = transform.localScale;
            _scale.x = 0;
            _uv = new Vector2(0, 1);
            _position = Vector3.zero;
            transform.localScale = _scale;

            // float tang = -0.5f;
            // float weight = 0.1f;
            // _pathPositionOffset = new AnimationCurve();
            // foreach (Transform pathPoint in _pathPoints)
            // {
            //     Vector3 localPosition = pathPoint.localPosition;
            //     _pathPositionOffset.AddKey(new Keyframe(
            //         (localPosition.x + 0.5f) * 3,
            //         localPosition.y / 2,
            //         tang,
            //         tang,
            //         weight,
            //         weight
            //     ));
            // }
        }

        public void UpdateGross(float grossPercent)
        {
            _uv.x = grossPercent;
            _scale.x = Mathf.Lerp(0, _size.x, grossPercent);
            _position.x = _scale.x / 2;
            if (_localMaterial != null)
                _localMaterial.mainTextureScale = _uv;
            transform.localScale = _scale;
            transform.localPosition = _position;
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