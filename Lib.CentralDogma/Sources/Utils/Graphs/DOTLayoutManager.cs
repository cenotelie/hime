/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.Utils.Graphs
{
    interface DOTLayoutManager
    {
        void Render(string dotFile, string svgFile);
    }
}