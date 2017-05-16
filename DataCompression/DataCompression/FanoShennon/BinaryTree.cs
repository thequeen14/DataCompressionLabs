using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCompression.FanoShennon
{
    public class Node<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public Node<TKey, TValue> Left { get; set; }
        public Node<TKey, TValue> Right { get; set; }
        public Node<TKey, TValue> Parent { get; set; }

        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Left = null; //вообще говоря, они и по умолчанию null
            Right = null;
            Parent = null;
        }
    }

    public class BinaryTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        public int Count { get; private set; }
        public Node<TKey, TValue> Root;

        public BinaryTree()
        {
            Count = 0;
            Root = null;
        }

        public void Add(TKey key, TValue value)
        {
            var node = new Node<TKey, TValue>(key, value);
            if (Root == null)
            {
                Root = node;
                Count = 1;
            }
            else
            {
                var tempNode = Root;
                Node<TKey, TValue> parent = null;

                while (tempNode != null)
                {
                    if (tempNode.Key.CompareTo(key) == 0)
                    {
                        throw new ArgumentException("Key must be unique.");
                    }
                    parent = tempNode;
                    if (tempNode.Key.CompareTo(key) > 0)
                    {
                        tempNode = tempNode.Left;
                    }
                    else
                    {
                        tempNode = tempNode.Right;
                    }
                }

                if (parent.Key.CompareTo(key) > 0)
                {
                    parent.Left = node;
                }
                else
                {
                    parent.Right = node;
                }
                node.Parent = parent;
                Count++;
            }
        }

        public int CountLeaves(Node<TKey, TValue> currentNode)
        {
            int count = 0;
            if (currentNode.Left == null && currentNode.Right == null)
            {
                count++;
            }
            else
            {
                if (currentNode.Left != null)
                {
                    CountLeaves(currentNode.Left);
                }
                if (currentNode.Right != null)
                {
                    CountLeaves(currentNode.Right);
                }
            }
            return count;
        }
    }
}
