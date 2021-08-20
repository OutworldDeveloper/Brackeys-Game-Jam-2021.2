public class CommandDescriptionsGenerator
{

    private readonly ConsoleColors _colors;

    public CommandDescriptionsGenerator(ConsoleColors colors)
    {
        _colors = colors;
    }

    public string GenerateDescription(ConsoleCommand command)
    {
        string description = $"<color=white>{command.Alias}</color>";
        var parameters = command.Parameters;

        if (parameters.Length > 0)
        {
            string colorHex = _colors.SuggestionParametersHEX;
            description += $" <color=#{colorHex}><";

            for (int i = 0; i < parameters.Length; i++)
            {
                description += parameters[i].ParameterType.Name.ToLower();
                if (i < parameters.Length - 1)
                {
                    description += ", ";
                }
            }

            description += "></color>";
        }

        description += $" - <color=#{_colors.SuggestionDescriptionHEX}>{command.Description.ToLower()}</color>";

        return description;
    }

}