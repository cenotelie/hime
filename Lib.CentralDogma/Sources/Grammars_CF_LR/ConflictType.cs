using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    enum ConflictType
    {
        ShiftReduce,
        ReduceReduce,
        None
    }
}