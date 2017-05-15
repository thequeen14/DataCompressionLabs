using System;
using System.Collections.Generic;

namespace DataCompression.Huffman
{


    internal class HuffmanTree : IComparable
    {
        public HuffmanTree LeftChild { get; set; }
        public HuffmanTree RightChild { get; set; }
        public int Frequency { get; set; }
        public string Chars { get; set; }
        public string Code { get; set; }

        public HuffmanTree()
        {
            Frequency = 1;
            LeftChild = null;
            RightChild = null;
        }

        public HuffmanTree(HuffmanTree left, HuffmanTree right)
        {
            Frequency = 1;
            LeftChild = left;
            RightChild = right;
        }

        public void SetChildCodes()
        {
            if (LeftChild != null)
            {
                LeftChild.Code = Code + "0";
                LeftChild.SetChildCodes();
            }

            if (RightChild != null)
            {
                RightChild.Code = Code + "1";
                RightChild.SetChildCodes();
            }
        }

        public Dictionary<string, string> GetChilds(Dictionary<string, string> list)
        {
            if (LeftChild != null)
                LeftChild.GetChilds(list);

            if (RightChild != null)
                RightChild.GetChilds(list);

            if (LeftChild == null && RightChild == null)
                list.Add(Chars, Code);
            return list;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return -2;
            var other = obj as HuffmanTree;
            if (Frequency > other.Frequency)
                return 1;
            if (Frequency < other.Frequency)
                return -1;
            return 0;
        }
    }
}
