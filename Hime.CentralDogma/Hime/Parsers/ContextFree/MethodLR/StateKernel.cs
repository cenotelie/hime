/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree.LR
{
    public class StateKernel
    {
        private Dictionary<CFRule, Dictionary<int, List<Item>>> dictItems;
        private List<Item> items;

        public int Size { get { return items.Count; } }
        public ICollection<Item> Items { get { return items; } }

        public StateKernel()
        {
            dictItems = new Dictionary<CFRule, Dictionary<int, List<Item>>>(CFRule.Comparer.Instance);
            items = new List<Item>();
        }

        public void AddItem(Item item)
        {
            if (!dictItems.ContainsKey(item.BaseRule))
                dictItems.Add(item.BaseRule, new Dictionary<int, List<Item>>());
            Dictionary<int, List<Item>> sub = dictItems[item.BaseRule];
            if (!sub.ContainsKey(item.DotPosition))
                sub.Add(item.DotPosition, new List<Item>());
            sub[item.DotPosition].Add(item);
            items.Add(item);
        }
        public bool ContainsItem(Item item)
        {
            if (!dictItems.ContainsKey(item.BaseRule))
                return false;
            Dictionary<int, List<Item>> sub = dictItems[item.BaseRule];
            if (!sub.ContainsKey(item.DotPosition))
                return false;
            return sub[item.DotPosition].Contains(item);
        }

        public State GetClosure()
        {
            // The set's items
            Dictionary<CFRule, Dictionary<int, List<Item>>> map = new Dictionary<CFRule, Dictionary<int, List<Item>>>(CFRule.Comparer.Instance);
            foreach (CFRule rule in dictItems.Keys)
            {
                Dictionary<int, List<Item>> clone = new Dictionary<int, List<Item>>();
                Dictionary<int, List<Item>> original = dictItems[rule];
                map.Add(rule, clone);
                foreach (int position in original.Keys)
                {
                    List<Item> list = new List<Item>(original[position]);
                    clone.Add(position, list);
                }
            }
            List<Item> closure = new List<Item>(items);
            // Close the set
            for (int i = 0; i != closure.Count; i++)
                closure[i].CloseTo(closure, map);
            return new State(this, closure);
        }

        public bool Equals(StateKernel kernel)
        {
            if (this.items.Count != kernel.items.Count)
                return false;
            if (this.dictItems.Count != kernel.dictItems.Count)
                return false;
            foreach (CFRule rule in this.dictItems.Keys)
            {
                if (!kernel.dictItems.ContainsKey(rule))
                    return false;
                Dictionary<int, List<Item>> left = this.dictItems[rule];
                Dictionary<int, List<Item>> right = kernel.dictItems[rule];
                if (left.Count != right.Count)
                    return false;
                foreach (int position in left.Keys)
                {
                    if (!right.ContainsKey(position))
                        return false;
                    List<Item> l1 = left[position];
                    List<Item> l2 = right[position];
                    if (l1.Count != l2.Count)
                        return false;
                    foreach (Item item in l1)
                        if (!l2.Contains(item))
                            return false;
                }
            }
            return true;
        }
        public override bool Equals(object obj) { return Equals(obj as StateKernel); }
        public override int GetHashCode() { return base.GetHashCode(); }
    }
}