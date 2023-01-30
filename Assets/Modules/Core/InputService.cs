namespace Modules.Core
{
    static public class InputService
    {
        private static InputSystem _inputSystem;
        private static bool _allowControl = true;

        static public InputSystem InputSystem
        {
            get
            {
                if (_inputSystem != null)
                    return _inputSystem;
                
                _inputSystem = new InputSystem();
                _inputSystem.Enable();
                return _inputSystem;
            }
        }

        static public bool AllowControl
        {
            get => _allowControl;
            set
            {
                if (!_allowControl && !value) return;

                _allowControl = value;
            }
        }
    }
}