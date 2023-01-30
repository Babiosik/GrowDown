using UnityEngine;

namespace Modules.Backgrounds
{
    [RequireComponent(typeof(MeshRenderer))]
    public class SkyMover : MonoBehaviour
    {
        [SerializeField] private float _speed = 1;
        
        private MeshRenderer _meshRenderer;
        private Material _material;

        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            
        #if UNITY_EDITOR
            _material = new Material(_meshRenderer.sharedMaterial);
            _meshRenderer.sharedMaterial = _material;
            _meshRenderer.material = _material;
        #else
            _material = new Material(_meshRenderer.material);
            _meshRenderer.material = _material;
        #endif
            
        }

        private void Update()
        {
            _material.mainTextureOffset += Vector2.right * Time.deltaTime * _speed;
        }
    }
}