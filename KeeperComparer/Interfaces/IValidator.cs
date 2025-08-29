namespace KeeperComparer.Interfaces
{
    public interface IValidator<T>
    {
        bool IsValid(T? value);
    }
}