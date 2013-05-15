using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    interface StateAction
    {
        ItemAction ActionType { get; }
        Symbol OnSymbol { get; }
    }
}