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

        private void Start()
        {
            _inputSystem = InputService.InputSystem;
            _camera = GetComponent<Camera>();
            AliveService.OnLevelUp += UnlockNewZone;
            UnlockNewZone(0);
        }

        private void Update()
        {
            if (!InputService.AllowControl) return;

            Move(_inputSystem.All.Move);
            Zoom(_inputSystem.All.Zoom);
        }

        private void Move(InputAction ctx)
        {
            Vector2 move = ctx.ReadValue<Vector2>() * _speed * Time.deltaTime;
            if (transform.position.x + move.x < _borderMove.x)
                move.x = 0;
            else if (transform.position.x + move.x > _borderMove.z)
                move.x = 0;

            if (transform.position.y + move.y < _borderMove.y)
                move.y = 0;
            else if (transform.position.y + move.y > _borderMove.w)
                move.y = 0;

            transform.Translate(move);
        }

        private void Zoom(InputAction ctx)
        {
            float scroll = -ctx.ReadValue<float>() * _scrollSensitivity * Time.deltaTime;
            scroll += _camera.orthographicSize;
            _camera.orthographicSize = Mathf.Clamp(scroll, _camZoomMin, _camZoomMax);
        }
        
        private void UnlockNewZone(int level) =>
            _borderMove.y = -_sizePerLevel * (level + 1);
    }
}