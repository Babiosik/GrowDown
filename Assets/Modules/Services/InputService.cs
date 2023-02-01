namespace Modules.Services
{
    public class InputService
    {
        #region Static

        private static InputService Inst => _self ??= new InputService();
        private static InputService _self;

        static public InputSystem InputSystem => Inst._inputSystem;
        static public bool AllowControl
        {
            get => Inst._allowControl;
            set => Inst.SetAllowControl(value);
        }

        static public void Dispose() =>
            _self = null;

        #endregion
        #region Instance

        private readonly InputSystem _inputSystem;
        private bool _allowControl = true;

        private InputService()
        {
            _inputSystem = new InputSystem();
            _inputSystem.Enable();
        }

        private void SetAllowControl(bool value)
        {
            if (!_allowControl && !value) return;
            _allowControl = value;
        }

        #endregion
    }
}