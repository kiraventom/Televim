using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Televim.Vim.Actions;

internal class ActionFactory(IReadOnlyList<IActionDefinition> definitions, IServiceProvider sp) : IActionFactory
{
    public IReadOnlyList<IActionDefinition> Definitions => definitions;

    public IAction Build(IActionDefinition definition) => sp.GetRequiredKeyedService<IAction>(definition.ActionName);
}
