using Unity.VisualScripting;

namespace Framework
{
    public interface IPool
    {
        Poolable Get();
        void Release(Poolable poolable);
        void Clear();
    }
}