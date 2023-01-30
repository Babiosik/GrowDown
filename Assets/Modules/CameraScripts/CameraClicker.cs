using Modules.Core;
using Modules.RootChange.Scripts;
using Modules.Roots.Scripts;
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
            
            _inputSystem.All.MouseLeftButton.performed += OnMouseClick;
        }

        private void OnEnable()
        {
            if (_inputSystem == null) return;
            
            _inputSystem.All.MouseLeftButton.performed += OnMouseClick;
        }

        private void OnDisable()
        {
            _inputSystem.All.MouseLeftButton.performed -= OnMouseClick;
        }
        
        private void OnMouseClick(InputAction.CallbackContext obj)
        {
            if (!InputService.AllowControl) return;

            var mousePos = _inputSystem.All.MousePosition.ReadValue<Vector2>();
            Ray ray = _camera.ScreenPointToRay(mousePos);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit, 100, _layerMask))
                return;
            
            if (TryRootHead(hit.transform)) return;
            if (TryRootSegment(hit.transform, hit.point)) return;
        }

        private bool TryRootHead(Transform obj)
        {
            var rootHead = obj.parent.GetComponent<RootHead>();
            if (rootHead == null) return false;
            
            rootHead.SetPauseGross(true);
            _changeDirectionRoot.Triggered(rootHead);           
            
            return true;
        }
        
        private bool TryRootSegment(Transform obj, Vector3 mouseWorldPos)
        {
            var rootSegment = obj.parent.parent.GetComponent<RootSegment>();
            if (rootSegment == null) return false;

            _spawnRoot.transform.position = mouseWorldPos;
            _spawnRoot.Triggered(rootSegment);
            
            return true;
        }
    }
}