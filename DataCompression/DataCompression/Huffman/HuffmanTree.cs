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
            LeftChild = right;
            RightChild = left;
        }

        public int SetChildCodes()
        {
            int maxLength = 0;

            if (LeftChild != null)
            {
                LeftChild.Code = Code + "0";
                maxLength = LeftChild.SetChildCodes() + 1;
            }

            if (RightChild != null)
            {
                RightChild.Code = Code + "1";
                var tempLength = RightChild.SetChildCodes() + 1;
                if (tempLength > maxLength)
                    maxLength = tempLength;
            }

            return maxLength;
        }

        public void FormatChildCodes(int max)
        {
            if (LeftChild != null && LeftChild.Code.Length < max)
                    LeftChild.Code = new string('0', max - LeftChild.Code.Length) + LeftChild.Code;

            if (RightChild != null && RightChild.Code.Length < max)
                RightChild.Code = new string('0', max - RightChild.Code.Length) + RightChild.Code;
        }

        public Dictionary<string, string> GetChilds(Dictionary<string, string> list)
        {
            if (LeftChild != null)
                list = LeftChild.GetChilds(list);

            if (RightChild != null)
                list = RightChild.GetChilds(list);

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
