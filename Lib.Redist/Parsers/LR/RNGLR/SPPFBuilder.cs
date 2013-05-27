using System;

namespace Hime.Redist.Parsers
{
    class SPPFBuilder
    {
        private Utils.Pool<SPPFNode> pool;

        public SPPFBuilder()
        {
            pool = new Utils.Pool<SPPFNode>(256);
        }

        public SPPFNode NewNode(Symbols.Symbol symbol, Parsers.LRTreeAction action)
        {
            SPPFNode node = pool.Acquire();
            node.Init(symbol, action);
            return node;
        }

        public void Build(SPPFNode current, SPPFNode[] children, int length)
        {
            if (current.action == Parsers.LRTreeAction.Replace)
                BuildReplaceable(current, children, length);
            else
                BuildNormal(current, children, length);
        }

        public ASTNode GetRoot(SPPFNode sub)
        {
            Expose(sub.value, sub.children, sub.count);
            return sub.value;
        }

        private void BuildReplaceable(SPPFNode current, SPPFNode[] children, int length)
        {
            int index = 0;
            for (int i = 0; i != length; i++)
            {
                SPPFNode child = children[i];
                switch (child.action)
                {
                    case Parsers.LRTreeAction.Replace:
                        System.Array.Copy(child.children, 0, current.children, index, child.count);
                        index += child.count;
                        FreeSingle(child);
                        break;
                    case Parsers.LRTreeAction.Drop:
                        FreeTree(child);
                        break;
                    default:
                        current.children[index] = child;
                        index++;
                        break;
                }
            }
            current.count = index;
        }

        private void BuildNormal(SPPFNode current, SPPFNode[] children, int length)
        {
            SPPFNode promoted = null;
            for (int i = 0; i != length; i++)
                promoted = BuildNormalOn(current, promoted, children[i]);
            if (promoted != null)
            {
                current.value = promoted.value;
                FreeSingle(promoted);
            }
        }

        private SPPFNode BuildNormalOn(SPPFNode current, SPPFNode promoted, SPPFNode child)
        {
            switch (child.action)
            {
                case Parsers.LRTreeAction.Replace:
                    for (int i = 0; i != child.count; i++)
                        promoted = BuildNormalOn(current, promoted, child.children[i]);
                    return promoted;
                case Parsers.LRTreeAction.Promote:
                    if (promoted == null)
                    {
                        if (child.count != 0)
                        {
                            Array.Copy(child.children, 0, current.children, current.count, child.count);
                            current.count += child.count;
                        }
                    }
                    else
                    {
                        Expose(promoted.value, current.children, current.count);
                        promoted.count = 0;
                        current.children[0] = promoted;
                        current.count = 1;
                        if (child.count != 0)
                        {
                            Array.Copy(child.children, 0, current.children, current.count, child.count);
                            current.count += child.count;
                        }
                    }
                    return child;
                case Parsers.LRTreeAction.Drop:
                    FreeTree(child);
                    return promoted;
                default:
                    Expose(child.value, child.children, child.count);
                    child.count = 0;
                    current.children[current.count++] = child;
                    return promoted;
            }
        }

        private void FreeSingle(SPPFNode node)
        {
            //node.value = null;
            node.count = 0;
            pool.Free(node);
        }

        private void FreeTree(SPPFNode node)
        {
            for (int i = 0; i != node.count; i++)
                FreeTree(node.children[i]);
            //node.value = null;
            node.count = 0;
            pool.Free(node);
        }

        private void Expose(ASTNode node, SPPFNode[] children, int count)
        {
            ASTNode[] content = new ASTNode[count];
            for (int i = 0; i != count; i++)
            {
                content[i] = children[i].value;
                FreeTree(children[i]);
            }
            //node.Children = new ASTNode.Family(content);
        }
    }
}
