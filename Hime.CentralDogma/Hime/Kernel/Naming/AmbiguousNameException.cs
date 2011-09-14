/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.Kernel.Naming
{
    public class AmbiguousNameException : NamingException
    {
        private Symbol origin;
        private QualifiedName name;

        public Symbol Origin { get { return origin; } }
        public QualifiedName Name { get { return name; } }

        public AmbiguousNameException(Symbol origin, QualifiedName name)
            : base("Ambiguous name " + name.ToString() + " from symbol " + origin.CompleteName.ToString())
        {
            this.origin = origin;
            this.name = name;
        }
    }
}