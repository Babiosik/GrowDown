using Modules.Core;
using Modules.Roots.Scripts;
using Modules.Singletones.Factories;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.SpawnRoot.Scripts
{
    public class SpawnRoot : MonoBehaviour
    {
        private const float AngleMultiple = -6;

        [SerializeField] private GameObject[] _angleLines = new GameObject[3];
        [SerializeField] private float _rotationStart;
        [SerializeField] private float _rotationEnd;
        [SerializeField] private bool _isFirst = false;

        private InputSystem _inputSystem;
        private bool _isActive = false;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            _inputSystem = InputService.InputSystem;

            SetActive(false);
            if (_isFirst) Triggered();
        }

        private void Update()
        {
            if (!_isActive) return;

            var positionMouse = _inputSystem.All.MousePosition.ReadValue<Vector2>();
            Vector3 worldMouse = _camera.ScreenToWorldPoint(positionMouse);
            Quaternion rotation = Quaternion.LookRotation(transform.position - worldMouse);
            float angle = rotation.eulerAngles.y;
            if (angle > 180) angle -= 360;
            angle = Mathf.Clamp(angle * AngleMultiple, _rotationStart, _rotationEnd);
            _angleLines[2].transform.localRotation = Quaternion.Euler(0, 0, angle);
        }

        private void Triggered()
        {
            if (!InputService.AllowControl) return;
            
            SetActive(true);
            _angleLines[0].transform.localRotation = Quaternion.Euler(0, 0, _rotationStart);
            _angleLines[1].transform.localRotation = Quaternion.Euler(0, 0, _rotationEnd);

            InputService.AllowControl = false;
            _inputSystem.All.MouseLeftButton.performed += OnApply;
        }

        private void OnApply(InputAction.CallbackContext obj)
        {
            _inputSystem.All.MouseLeftButton.performed -= OnApply;
            InputService.AllowControl = true;

            SetActive(false);
            RootHead head = RootFactory.CreateRootHead();
            RootSegment segment = RootFactory.CreateRootSegment();
            RootFactory.CreateRootSegmentMesh(segment);
            
            segment.Init(head, transform.position, _angleLines[2].transform.rotation);
        }

        private void SetActive(bool active)
        {
            _isActive = active;
            foreach (GameObject line in _angleLines)
                line.SetActive(active);
        }

    }
}