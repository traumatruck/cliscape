namespace CliScape.Cli.Commands;

public enum ExitCode
{
    /// <summary>
    ///     The command was successfully executed.
    /// </summary>
    Success = 0,

    /// <summary>
    ///     The command failed to execute.
    /// </summary>
    Failure = 1
}