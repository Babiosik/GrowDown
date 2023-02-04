using Modules.Roots.Scripts;
using Modules.Services;
using UnityEngine;

namespace Modules.Plants.Scripts
{
    public class PlantLive : MonoBehaviour
    {
        [SerializeField] private float _deepMinCam;
        [SerializeField] private Camera _camera;
        [SerializeField] private PlantLevel[] _levels;
        [SerializeField] private PlantLevel _finishLevel;

        private PlantLevel _currentLevel;
        private bool _isNeedChange;
        private bool _isStarted;
        private int _level;

        private void Start() =>
            _currentLevel = _levels[0];
        
        private void OnEnable()
        {
            AliveService.OnDied += OnDied;
            AliveService.OnLevelUp += OnLevelUp;
            AliveService.OnFinish += OnFinish;
            AliveService.OnStart += OnStarted;
        }

        private void OnDisable()
        {
            AliveService.OnDied -= OnDied;
            AliveService.OnLevelUp -= OnLevelUp;
            AliveService.OnFinish += OnFinish;
            AliveService.OnStart -= OnStarted;
        }

        private void Update()
        {
            if (_isStarted)
                _currentLevel.Drink(Time.deltaTime);
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

        private void OnFinish(RootHead rootHead)
        {
            _currentLevel.gameObject.SetActive(false);
            _finishLevel.gameObject.SetActive(true);
            _finishLevel.enabled = false;
        }

        private void OnStarted() =>
            _isStarted = true;
    }
}