using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBT
{
    public class TBTTreeNode
    {
        private const int defaultChildCount = -1;
        private List<TBTTreeNode> childrenList;
        private int maxChildCount;

        public TBTTreeNode(int maxChildCount = -1)
        {
            childrenList = new List<TBTTreeNode>();
            if (maxChildCount > 0)
                childrenList.Capacity = maxChildCount;
            this.maxChildCount = maxChildCount;
        }

        public TBTTreeNode() : this(defaultChildCount)
        {
        }

        ~TBTTreeNode()
        {
            childrenList = null;
        }

        public TBTTreeNode AddChild(TBTTreeNode node)
        {
            if (maxChildCount >= 0 && childrenList.Count >= maxChildCount)
            {
                UnityEngine.Debug.LogError("BTTree Child Count Error!");
                return this;
            }
            childrenList.Add(node);
            return this;
        }

        public int GetChildCount()
        {
            return childrenList.Count;
        }

        public bool IsIndexValid(int index)
        {
            return index >= 0 && index < childrenList.Count;
        }

        public T GetChild<T>(int index) where T : TBTTreeNode
        {
            if (index < 0 || index >= childrenList.Count)
                return null;
            return (T)childrenList[index];
        }
    }
}
