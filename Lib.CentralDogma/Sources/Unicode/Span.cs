/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.CentralDogma.Unicode
{
    class Span
    {
        protected ushort begin;
        protected ushort end;

        public ushort Begin { get { return begin; } }
        public ushort End { get { return end; } }

        public Span(ushort begin, ushort end)
        {
            this.begin = begin;
            this.end = end;
        }
    }
}