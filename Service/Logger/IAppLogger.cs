namespace Service.Logger
{
    public interface IAppLogger<T>
    {
        void Information(string message, params object[] args);
        void Warning(string message, params object[] args);
        void Error(string message, Exception exception);
        void Debug(string message, params object[] args);
        void Fatal(string message, Exception exception);
    }
}
