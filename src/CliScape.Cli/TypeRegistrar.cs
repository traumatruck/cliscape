using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace CliScape.Cli;

/// <summary>
///     Adapts <see cref="IServiceCollection" /> to Spectre.Console.Cli's
///     <see cref="ITypeRegistrar" /> so that commands receive constructor-injected dependencies.
/// </summary>
public sealed class TypeRegistrar(IServiceCollection services) : ITypeRegistrar
{
    public ITypeResolver Build()
    {
        return new TypeResolver(services.BuildServiceProvider());
    }

    public void Register(Type service, Type implementation)
    {
        services.AddSingleton(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        services.AddSingleton(service, implementation);
    }

    public void RegisterLazy(Type service, Func<object> factory)
    {
        services.AddSingleton(service, _ => factory());
    }
}
