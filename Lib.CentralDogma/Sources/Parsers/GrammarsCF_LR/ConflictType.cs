/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree.LR
{
    enum ConflictType
    {
        ShiftReduce,
        ReduceReduce,
        None
    }
}