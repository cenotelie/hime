/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.Kernel.Naming
{
    public class NameCollisionException : NamingException
    {
        private Symbol container;
        private string name;

        public Symbol Container { get { return container; } }
        public string Name { get { return name; } }

        public NameCollisionException(Symbol container, string name)
            : base("Name collision in symbol " + container.CompleteName.ToString() + " for " + name)
        {
            this.container = container;
            this.name = name;
        }
    }
}