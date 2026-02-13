using Spectre.Console.Cli;

namespace CliScape.Cli;

/// <summary>
///     Resolves services from the built <see cref="IServiceProvider" />.
/// </summary>
public sealed class TypeResolver(IServiceProvider provider) : ITypeResolver
{
    public object? Resolve(Type? type)
    {
        return type is null ? null : provider.GetService(type);
    }
}
