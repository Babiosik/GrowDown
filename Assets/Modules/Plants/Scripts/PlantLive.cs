using System;
using Modules.Services;
using UnityEngine;

namespace Modules.Plants.Scripts
{
    public class PlantLive : MonoBehaviour
    {
        [SerializeField] private float _deepMinCam;
        [SerializeField] private Camera _camera;
        [SerializeField] private PlantLevel[] _levels;

        private PlantLevel _currentLevel;
        private bool _isNeedChange;
        private int _level;

        private void Start() =>
            _currentLevel = _levels[0];
        
        private void OnEnable()
        {
            AliveService.OnDied += OnDied;
            AliveService.OnLevelUp += OnLevelUp;
        }

        private void OnDisable()
        {
            AliveService.OnDied -= OnDied;
            AliveService.OnLevelUp -= OnLevelUp;
        }

        private void Update()
        {
            if (!_isNeedChange) return;

            if (_camera.transform.position.y < _deepMinCam)
                SetLevel();
        }

        private void OnLevelUp(int level)
        {
            _level = level;
            _isNeedChange = true;
            if (_camera.transform.position.y < _deepMinCam)
                SetLevel();
        }

        private void SetLevel()
        {
            if (_currentLevel != null)
                _currentLevel.gameObject.SetActive(false);
            _currentLevel = _levels[_level];
            _currentLevel.gameObject.SetActive(true);

            _isNeedChange = false;
        }

        private void OnDied() =>
            _currentLevel.Die();
    }
}