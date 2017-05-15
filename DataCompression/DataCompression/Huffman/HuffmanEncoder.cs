using System.Collections.Generic;
using System.Linq;

namespace DataCompression.Huffman
{
    public class HuffmanEncoder : IEncoder
    {
        private readonly Dictionary<string, HuffmanTree> _frequencyTable = new Dictionary<string, HuffmanTree>();
        private Dictionary<string, string> _codeTable = new Dictionary<string, string>(); 
        private HuffmanTree _root;

        public void Encode(string file)
        {
            var message = MessageExtractor.ExtractMessage(file);
            GetFrequencyTable(message);

            while(_frequencyTable.Count > 1)
                AddNewLeaf();

            if (_frequencyTable.Count == 1)
            {
                var tempLeft = _frequencyTable.ElementAt(0);
                var newLeft = new HuffmanTree
                {
                    Frequency = tempLeft.Value.Frequency,
                    Chars = tempLeft.Key
                };
                var temp = _root;
                newLeft.LeftChild = temp;
                _root = newLeft;
            }

            SetCodes();

            var encodedMessage = MakeCode(message);
            MessageFileWriter.WriteEncodedMessageToFile("HuffmanEncoderOutput.txt", encodedMessage);
        }

        private void GetFrequencyTable(string message)
        {
            foreach (var ch in message)
            {
                if (_frequencyTable.ContainsKey(ch.ToString())) //!!! tostring
                    _frequencyTable[ch.ToString()].Frequency++; //!!! tostring
                else
                    _frequencyTable.Add(ch.ToString(), new HuffmanTree()); //!!! tostring
            }
            _frequencyTable.OrderByDescending(pair => pair.Value); //do something with ordering
        }

        private void AddNewLeaf()
        {
            var newLeft = new HuffmanTree();
            var tempLeft = _frequencyTable.ElementAt(0);
            newLeft.Frequency = tempLeft.Value.Frequency;
            newLeft.Chars = tempLeft.Key;
            _frequencyTable.Remove(newLeft.Chars);

            var newRight = new HuffmanTree();
            var tempRight = _frequencyTable.ElementAt(1);
            newRight.Frequency = tempRight.Value.Frequency;
            newRight.Chars = tempRight.Key;
            _frequencyTable.Remove(newRight.Chars);

            _root = new HuffmanTree(newLeft, newRight);
            _root.Frequency = newLeft.Frequency + newRight.Frequency;
            _root.Chars = newLeft.Chars + newRight.Chars;
            _frequencyTable.Add(_root.Chars, _root);
            _frequencyTable.OrderByDescending(pair => pair.Value); //do something with ordering
        }

        public void SetCodes()
        {
            _root.SetChildCodes();
            _codeTable = _root.GetChilds(_codeTable);
        }

        public string MakeCode(string message)
        {
            string code = null;
            foreach (var ch in message)
            {
                code += _codeTable[ch.ToString()]; //!!! tostring
            }
            return code;
        }
    }
}
