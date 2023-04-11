namespace WpfMultiWindow.Helpers
{
    public interface IAbstractFactory<T>
    {
        T Create();
    }
}