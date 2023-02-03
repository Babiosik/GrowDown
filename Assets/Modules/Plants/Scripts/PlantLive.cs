using Modules.Services;
using UnityEngine;

namespace Modules.Plants.Scripts
{
    [RequireComponent(typeof(MeshRenderer))]
    public class PlantLive : MonoBehaviour
    {
        [SerializeField] private Texture[] _alivePlantGross;
        [SerializeField] private Texture[] _deadPlantGross;
        [SerializeField] private float _timeForGross;

        private MeshRenderer _mesh;
        private bool _isDead = false;
        private int _grossIndex = 0;
        private float _grossTimer = 0;
        private readonly static int MainTex = Shader.PropertyToID("_MainTex");

        private void Start() =>
            _mesh = GetComponent<MeshRenderer>();

        private void OnEnable() =>
            AliveService.OnDied += OnDied;

        private void OnDisable() =>
            AliveService.OnDied -= OnDied;

        private void Update()
        {
            if (_isDead) return;
            
            _grossTimer += Time.deltaTime;

            if (_grossTimer < _timeForGross) return;
            SetNextLevel();
        }
        
        private void SetNextLevel()
        {
            _grossTimer = 0;
            if (_grossIndex >= _alivePlantGross.Length - 1) return;
            _grossIndex++;
            SetTexture(_alivePlantGross[_grossIndex]);
        }

        private void OnDied()
        {
            _isDead = true;
            SetTexture(_deadPlantGross[_grossIndex]);
        }

        private void SetTexture(Texture texture) =>
            _mesh.material.SetTexture(MainTex, texture);
    }
}