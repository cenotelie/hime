/*
 * Author: Laurent Wouters
 * Date: 12/03/2013
 * Time: 17:22
 * 
 */

namespace Hime.CentralDogma
{
    /// <summary>
    /// Represents a grammar's compilation mode
    /// </summary>
    public enum CompilationMode
    {
        /// <summary>
        /// Generates the source code for the lexer and parser
        /// </summary>
        Source,
        /// <summary>
        /// Generates the compiled assembly of the lexer and parser
        /// </summary>
        Assembly,
        /// <summary>
        /// Generates the source code for the lexer and parser and the compiled assembly
        /// </summary>
        SourceAndAssembly
    }
}
