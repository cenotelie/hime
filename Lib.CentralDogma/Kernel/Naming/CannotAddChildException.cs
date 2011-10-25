/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.Kernel.Naming
{
    public class CannotAddChildException : NamingException
    {
        private Symbol parent;
        private Symbol child;

        public Symbol ParentSymbol { get { return parent; } }
        public Symbol ChildSymbol { get { return child; } }

        public CannotAddChildException(Symbol parent, Symbol child)
            : base("Cannot add " + child.LocalName + " in symbol " + parent.CompleteName.ToString())
        {
            this.parent = parent;
            this.child = child;
        }
    }
}