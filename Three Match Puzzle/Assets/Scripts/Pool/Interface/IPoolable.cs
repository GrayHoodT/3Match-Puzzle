namespace Framework
{
    public interface IPoolable
    {
        IPool Pool { get; set; }
        void ReturnToPool();
    }
}
