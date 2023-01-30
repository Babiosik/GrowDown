using Modules.Core;
using UnityEngine;

namespace Modules.CameraScripts
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        
        [Header("x=xMin;y=yMin;z=xMax;w=yMax")]
        [SerializeField] private Vector4 _borderMove;
        
        private InputSystem _inputSystem;

        private void Awake()
        {
            _inputSystem = InputService.InputSystem;
        }

        private void Update()
        {
            if (!InputService.AllowControl) return;
            
            Vector2 move = _inputSystem.All.Move.ReadValue<Vector2>() * _speed * Time.deltaTime;
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
    }
}