/*
 * Author: Laurent Wouters
 * Date: 07/03/2013
 * Time: 17:25
 * 
 */

namespace Hime.Redist.AST
{
    class BuildNode
    {
        private CSTNode value;
        private BuildNode[] children;
        private int action;

        public CSTNode Value { get { return value; } }

        public BuildNode(Symbols.Symbol symbol)
        {
            this.value = new CSTNode(symbol);
        }

        public void SetAction(int action)
        {
            if (action != 0)
                this.action = action;
        }

        public void Build(BuildNode[] children, int length)
        {
            if (action == Parsers.LRProduction.HeadReplace)
                BuildReplaceable(children, length);
            else
                BuildNormal(children, length);
        }

        private void BuildReplaceable(BuildNode[] children, int length)
        {
            int size = 0;
            for (int i = 0; i != length; i++)
            {
                BuildNode child = children[i];
                switch (child.action)
                {
                    case Parsers.LRProduction.HeadReplace:
                        size += child.children.Length;
                        break;
                    case Parsers.LRBytecode.PopDrop:
                        break;
                    default:
                        size++;
                        break;
                }
            }
            BuildNode[] replacement = new BuildNode[size];
            int index = 0;
            for (int i = 0; i != length; i++)
            {
                BuildNode child = children[i];
                switch (child.action)
                {
                    case Parsers.LRProduction.HeadReplace:
                        System.Array.Copy(child.children, 0, replacement, index, child.children.Length);
                        index += child.children.Length;
                        child.children = null;
                        break;
                    case Parsers.LRBytecode.PopDrop:
                        break;
                    default:
                        replacement[index] = child;
                        index++;
                        break;
                }
            }
            this.children = replacement;
        }

        private void BuildNormal(BuildNode[] children, int length)
        {
            bool firstPromote = true;
            for (int i = 0; i != length; i++)
            {
                BuildNode child = children[i];
                switch (child.action)
                {
                    case Parsers.LRProduction.HeadReplace:
                        foreach (BuildNode subchild in child.children)
                            firstPromote = ExecuteReplacement(subchild, firstPromote);
                        child.children = null;
                        break;
                    case Parsers.LRBytecode.PopDrop:
                        break;
                    case Parsers.LRBytecode.PopPromote:
                        if (firstPromote)
                        {
                            child.value.Children.InsertRange(0, this.value.Children);
                            this.value = child.value;
                            firstPromote = false;
                        }
                        else
                        {
                            child.value.Children.Insert(0, this.value);
                            this.value = child.value;
                        }
                        break;
                    default:
                        this.value.Children.Add(child.value);
                        break;
                }
            }
        }

        private bool ExecuteReplacement(BuildNode child, bool firstPromote)
        {
            if (child.action != Parsers.LRBytecode.PopPromote)
            {
                this.value.Children.Add(child.value);
                return firstPromote;
            }
            else if (firstPromote)
            {
                child.value.Children.InsertRange(0, this.value.Children);
                this.value = child.value;
            }
            else
            {
                child.value.Children.Insert(0, this.value);
                this.value = child.value;
            }
            return false;
        }
    }
}
