using Modules.Core;
using Modules.Roots.Scripts;
using Modules.SpawnRoot.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.CameraScripts
{
    public class CameraClicker : MonoBehaviour
    {
        [SerializeField] private ChangeDirectionRoot _changeDirectionRoot;
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
        }

        private bool TryRootHead(Transform obj)
        {
            var rootHead = obj.parent.GetComponent<RootHead>();
            if (rootHead == null) return false;
            
            rootHead.SetPauseGross(true);
            _changeDirectionRoot.Triggered(rootHead);           
            
            return true;
        }
    }
}