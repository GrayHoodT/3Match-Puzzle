using UnityEngine;

namespace Framework
{
    public abstract class Poolable : MonoBehaviour, IPoolable
    {
        public IPool Pool { get; set; }
        public virtual void ReturnToPool() => Pool.Release(this);
    }
}

