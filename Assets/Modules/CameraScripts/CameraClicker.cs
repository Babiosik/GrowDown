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
            
            _inputSystem.All.DoButton.performed += OnMouseClick;
        }

        private void OnDisable()
        {
            _inputSystem.All.DoButton.performed -= OnMouseClick;
        }

        private void OnMouseClick(InputAction.CallbackContext obj)
        {
            if (!InputService.AllowControl) return;

            var mousePos = _inputSystem.All.CoursorPosition.ReadValue<Vector2>();
            Ray ray = _camera.ScreenPointToRay(mousePos);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit, 100, _layerMask))
                return;

            if (TryRootHead(hit.transform)) return;
            if (TryRootSegment(hit.transform, hit.point)) return;
        }

        private bool TryRootHead(Transform obj)
        {
            if (!obj.CompareTag("RootHead")) return false;
            
            var rootHead = obj.parent.GetComponent<RootHead>();
            if (rootHead == null) return false;
            if (rootHead.IsDied || !ResourcesService.IsCanChangeDirection) return true;
            
            _changeDirectionRoot.Triggered(rootHead);           
            
            return true;
        }

        private bool TryRootSegment(Transform obj, Vector3 mouseWorldPos)
        {
            if (!obj.CompareTag("RootSegment")) return false;
            
            var rootSegment = obj.parent.parent.GetComponent<RootSegment>();
            if (rootSegment == null) return false;
            if (rootSegment.IsDied || !ResourcesService.IsCanStartRoot) return true;

            _spawnRoot.transform.position = mouseWorldPos;
            _spawnRoot.Triggered(rootSegment);
            
            return true;
        }
    }
}