using System;
using UnityEngine;

namespace Modules.Roots.Scripts
{
    public class RootDiedSegmentMesh : MonoBehaviour
    {
        private Vector2 _uv;
        private Material _localMaterial;
        private MeshRenderer _meshRenderer;
        
        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            SetClonedMaterial();
        }

        private void Update()
        {
            _localMaterial.mainTextureScale = _uv;
            enabled = false;
        }

        public void SetUv(Vector2 uv) =>
            _uv = uv;

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