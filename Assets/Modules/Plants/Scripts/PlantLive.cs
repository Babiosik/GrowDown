using System;
using Modules.Services;
using UnityEngine;

namespace Modules.Plants.Scripts
{
    [RequireComponent(typeof(MeshRenderer))]
    public class PlantLive : MonoBehaviour
    {
        private readonly static int MainTex = Shader.PropertyToID("_MainTex");
        
        [SerializeField] private float _deepMinCam;
        [SerializeField] private Camera _camera;

        private MeshRenderer _mesh;
        private DeepZone _zone;
        private bool _isNeedChange;

        private void Start()
        {
            _mesh = GetComponent<MeshRenderer>();
        }

        private void OnEnable() =>
            AliveService.OnDied += OnDied;

        private void OnDisable() =>
            AliveService.OnDied -= OnDied;

        private void Update()
        {
            if (!_isNeedChange) return;

            if (_camera.transform.position.y > _deepMinCam) return;
            SetTexture(_zone.AliveTexture);
            _isNeedChange = false;
        }

        public void SetZone(DeepZone zone)
        {
            if (_zone != null && _zone.Level >= zone.Level) return;

            _zone = zone;
            if (_camera.transform.position.y < _deepMinCam)
                SetTexture(_zone.AliveTexture);
            else
                _isNeedChange = true;
        }

        private void OnDied() =>
            SetTexture(_zone.DeadTexture);

        private void SetTexture(Texture texture) =>
            _mesh.material.SetTexture(MainTex, texture);
    }
}