namespace CliScape.Cli.Commands;

public enum ExitCode
{
    /// <summary>
    ///     The command was successfully executed.
    /// </summary>
    Success = 0,

    /// <summary>
    ///     The command parameters failed validation.
    /// </summary>
    BadRequest = 1
}