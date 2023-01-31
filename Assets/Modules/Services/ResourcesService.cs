using System;

namespace Modules.Services
{
    static public class ResourcesService
    {
        static public Resource<float> Water { get; } = new Resource<float>(100);
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
            }
        }
        
        public Resource(T init) =>
            _value = init;
    }
}