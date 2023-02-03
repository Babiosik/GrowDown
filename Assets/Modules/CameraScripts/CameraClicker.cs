using Modules.RootChange.Scripts;
using Modules.Roots.Scripts;
using Modules.Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.CameraScripts
{
    public class CameraClicker : MonoBehaviour
    {
        [SerializeField] private ChangeDirectionRoot _changeDirectionRoot;
        [SerializeField] private SpawnRoot _spawnRoot;
        [SerializeField] private LayerMask _layerMask;
        
        private InputSystem _inputSystem;
        private Camera _camera;

        private void Start()
        {
            _inputSystem = InputService.InputSystem;
            _camera = Camera.main;

            OnEnable();
        }

        private void OnEnable()
        {
            if (_inputSystem == null) return;

            RootHead.OnClick += OnRootHeadClick;
            RootSegment.OnClick += OnRootSegmentClick;
        }

        private void OnDisable()
        {
            RootHead.OnClick -= OnRootHeadClick;
            RootSegment.OnClick -= OnRootSegmentClick;
        }

        private void OnRootHeadClick(RootHead rootHead) =>
            _changeDirectionRoot.Triggered(rootHead);

        private void OnRootSegmentClick(RootSegment rootSegment)
        {
            var mousePos = _inputSystem.All.CoursorPosition.ReadValue<Vector2>();
            
            var pos = _camera.ScreenToWorldPoint(mousePos);
            pos.z = 0;
            _spawnRoot.transform.position = pos;
            _spawnRoot.Triggered(rootSegment);
        }
    }
}