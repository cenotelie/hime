/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Reflection;
using System.Text;

namespace Hime.Kernel.Resources
{
    class ResourceNotFoundException : System.Exception
    {
        private string resourceName;
		private Assembly assembly;

        public ResourceNotFoundException(string name, Assembly assembly)
        {
            this.resourceName = name;
			this.assembly = assembly;
        }
		
		public override string Message {
			get {
				StringBuilder result = new StringBuilder();
				result.AppendLine("No resource with name " + this.resourceName + " found in assembly " + this.assembly.FullName + ".");
				result.AppendLine("Available resources:");
				foreach (string resource in this.assembly.GetManifestResourceNames())
				{
					result.AppendLine("\t" + resource);
				}
				return result.ToString();
			}
		}
    }
}
