public class ConsoleInput
{

    public readonly ConsoleCommand Command;
    public readonly object[] Parameters; // Why not objects

    public ConsoleInput(ConsoleCommand command, object[] parameters)
    {
        Command = command;
        Parameters = parameters;
    }

}
