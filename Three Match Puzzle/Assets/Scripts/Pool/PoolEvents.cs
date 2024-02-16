using System;

namespace Framework
{
    public class PoolEvents : EventArgs 
    {
        public event Action<Poolable> OnCreated;
        public void NotifyOnCreated(Poolable poolable) => OnCreated?.Invoke(poolable);

        public event Action<Poolable> OnGot;
        public void NotifyOnGot(Poolable poolable) => OnGot?.Invoke(poolable);

        public event Action<Poolable> OnReleased;
        public void NotifyOnReleased(Poolable poolable) => OnReleased?.Invoke(poolable);

        public event Action<Poolable> OnDestroyed;
        public void NotifyOnDestroyed(Poolable poolable) => OnDestroyed?.Invoke(poolable);
    }
}

