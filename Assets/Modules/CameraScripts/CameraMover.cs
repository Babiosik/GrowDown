using Modules.Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.CameraScripts
{
    [RequireComponent(typeof(Camera))]
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _scrollSensitivity = 1f;
        [SerializeField] private float _camZoomMin = 1;
        [SerializeField] private float _camZoomMax = 5;
        [SerializeField] private float _sizePerLevel = 20;

        [Header("x=xMin;y=yMin;z=xMax;w=yMax")]
        [SerializeField] private Vector4 _borderMove;

        private InputSystem _inputSystem;
        private Camera _camera;
        [SerializeField] private Vector3 _origin;
        [SerializeField] private bool _isDrag;

        private void Start()
        {
            _inputSystem = InputService.InputSystem;
            _camera = GetComponent<Camera>();
            UnlockNewZone(0);
            OnEnable();
        }
        
        private void OnEnable()
        {
            if (_inputSystem == null) return;

            InputService.OnChangeAllow += OnChangeAllow;
            AliveService.OnLevelUp += UnlockNewZone;
            _inputSystem.All.Zoom.performed += Zoom;
            _inputSystem.All.CancelButton.started += OnMouseStart;
            _inputSystem.All.CancelButton.canceled += OnMouseEnd;
        }

        private void OnDisable()
        {
            InputService.OnChangeAllow -= OnChangeAllow;
            AliveService.OnLevelUp -= UnlockNewZone;
            _inputSystem.All.Zoom.performed -= Zoom;
            _inputSystem.All.CancelButton.started -= OnMouseStart;
            _inputSystem.All.CancelButton.canceled -= OnMouseEnd;
        }

        private void LateUpdate()
        {
            if (!_isDrag) return;
            
            var mousePos = _inputSystem.All.CoursorPosition.ReadValue<Vector2>();
            Vector3 diff = _camera.ScreenToWorldPoint(mousePos) - _camera.transform.position;
            Vector3 pos = _origin - diff;

            pos.x = Mathf.Clamp(pos.x, _borderMove.x, _borderMove.z);
            pos.y = Mathf.Clamp(pos.y, _borderMove.y, _borderMove.w);
            
            _camera.transform.position = pos;
        }

        private void Zoom(InputAction.CallbackContext ctx)
        {
            if (!InputService.AllowControl) return;

            float scroll = -ctx.ReadValue<float>() * _scrollSensitivity * Time.deltaTime;
            scroll += _camera.orthographicSize;
            _camera.orthographicSize = Mathf.Clamp(scroll, _camZoomMin, _camZoomMax);
        }

        private void UnlockNewZone(int level) =>
            _borderMove.y = -_sizePerLevel * (level + 1);

        private void OnMouseStart(InputAction.CallbackContext ctx)
        {
            if (!InputService.AllowControl) return;

            _isDrag = true;
            var mousePos = _inputSystem.All.CoursorPosition.ReadValue<Vector2>();
            _origin = _camera.ScreenToWorldPoint(mousePos);
        }

        private void OnMouseEnd(InputAction.CallbackContext ctx) =>
            _isDrag = false;
        
        private void OnChangeAllow(bool isAllow)
        {
            if (!isAllow) _isDrag = false;
        }
    }
}