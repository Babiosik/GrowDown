using System;

namespace Modules.Services
{
    static public class ResourcesService
    {
        static public event Action OnResourcesChange;
        static public Resource<float> Water { get; } = new Resource<float>(50);
        static public bool IsCanChangeDirection => Water.Value > ChangeDirectionResource;
        static public int ChangeDirectionResource => 1;
        static public bool IsCanStartRoot => Water.Value > StartRootResource;
        static public int StartRootResource => 2;
        
        static public void Dispose()
        {
            Water.Value = 50;
        }

        static public void Update() =>
            OnResourcesChange?.Invoke();
    }

    public class Resource<T>
    {
        private T _value;

        public event Action<T> OnChange;
        
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnChange?.Invoke(_value);
                ResourcesService.Update();
            }
        }
        
        public Resource(T init) =>
            _value = init;
    }
}