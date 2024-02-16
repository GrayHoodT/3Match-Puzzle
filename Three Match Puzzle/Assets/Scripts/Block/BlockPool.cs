using System;
using System.Collections;
using UnityEngine;
using Framework;
using Anipang.Common;

namespace Anipang
{
    public class BlockPool : MonoBehaviour
    {
        [field: SerializeField]
        public Block Prefab { get; private set; }
        [field: SerializeField]
        public int DefaultCapacity { get; private set; }
        [field: SerializeField]
        public int MaxSize { get; private set; }
        
        private IPool blockPool;

        private PoolEvents events;
        public PoolEvents Events => events;

        public void Initialize(Block prefab, int defaultCapacity, int maxSize, Action<PoolEvents> events)
        {
            Prefab = prefab;
            DefaultCapacity = defaultCapacity;
            MaxSize = maxSize;
            blockPool = new Pool(Prefab, DefaultCapacity, MaxSize, out this.events);

            events.Invoke(this.events);
        }

        public Block Get() => blockPool.Get() as Block;
        public void Release(Block poolable) => blockPool.Release(poolable as Block);
        public void Clear() => blockPool.Clear();
    }
}
