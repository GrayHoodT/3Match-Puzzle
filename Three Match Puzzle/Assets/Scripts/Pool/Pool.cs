using UnityEngine;
using UnityEngine.Pool;

namespace Framework
{
    public class Pool : IPool
    {
        private Poolable prefab;
        private int defaultCapacity;
        private int maxCapacity;
        private IObjectPool<Poolable> pool;
        private PoolEvents events;

        public Pool(Poolable prefab, int defaultCapacity, int maxCapacity, out PoolEvents events)
        {
            this.prefab = prefab;
            this.defaultCapacity = defaultCapacity;
            this.maxCapacity = maxCapacity;
            pool = new ObjectPool<Poolable>(Create, OnGot, OnReleased, OnDestroyed, true, this.defaultCapacity, this.maxCapacity);
            
            this.events = new PoolEvents();
            events = this.events;
        }

        public Poolable Get() => pool.Get();
        public void Release(Poolable poolable) => pool.Release(poolable as Poolable);
        public void Clear() => pool.Clear();

        #region Callback Method

        private Poolable Create()
        {
            var poolable = GameObject.Instantiate<Poolable>(prefab);
            poolable.Pool = this;
            events.NotifyOnCreated(poolable);

            return poolable;
        }

        private void OnGot(Poolable poolable)
        {
            poolable.gameObject.SetActive(true);
            events.NotifyOnGot(poolable);
        }
        private void OnReleased(Poolable poolable)
        {
            poolable.gameObject.SetActive(false);
            events.NotifyOnReleased(poolable);
        }
        private void OnDestroyed(Poolable poolable)
        {
            GameObject.Destroy(poolable);
            events.NotifyOnDestroyed(poolable);
        }

        #endregion
    }
}
