public interface IConsole
{
    void RegisterObject(object target);
    void DeregisterObject(object target);
    void Log(object message, LogType logType = LogType.Message);
    void Submit(string input);
    void AddCommand(ConsoleCommand command);
}