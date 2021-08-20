public struct ConsoleLog
{

    public readonly string Message;
    public readonly LogType LogType;

    public ConsoleLog(string message, LogType type)
    {
        Message = message;
        LogType = type;
    }

}