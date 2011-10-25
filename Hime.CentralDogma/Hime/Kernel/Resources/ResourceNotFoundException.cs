/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.Kernel.Resources
{
    class ResourceNotFoundException : System.Exception
    {
        private string resourceName;

        public string ResourceName { get { return resourceName; } }

        public ResourceNotFoundException(string name)
            : base("No resource with name " + name + " found")
        {
            resourceName = name;
        }
		/*
		 	System.Console.WriteLine(assembly.FullName);
			foreach (string toto in assembly.GetManifestResourceNames())
			{
				System.Console.WriteLine(toto);
			}
		 */
    }
}
