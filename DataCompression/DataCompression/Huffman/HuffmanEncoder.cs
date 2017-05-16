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
            
            SetCodes();

            var encodedMessage = MakeCode(message);
            MessageFileWriter.WriteEncodedMessageToFile("HuffmanEncoderOutput.txt", encodedMessage);
        }

        private void GetFrequencyTable(string message)
        {
            foreach (var ch in message)
            {
                if (_frequencyTable.ContainsKey(ch.ToString()))
                    _frequencyTable[ch.ToString()].Frequency++;
                else
                    _frequencyTable.Add(ch.ToString(), new HuffmanTree());
            }
        }

        private void AddNewLeaf()
        {
            var minPair = FindMinInFrequencyTable();
            var newLeft = minPair.Value;
            newLeft.Chars = minPair.Key;
            _frequencyTable.Remove(newLeft.Chars);

            minPair = FindMinInFrequencyTable();
            var newRight = minPair.Value;
            newRight.Chars = minPair.Key;
            _frequencyTable.Remove(newRight.Chars);

            _root = new HuffmanTree(newLeft, newRight);
            _root.Frequency = newLeft.Frequency + newRight.Frequency;
            _root.Chars = newLeft.Chars + newRight.Chars;
            _frequencyTable.Add(_root.Chars, _root);
        }

        private KeyValuePair<string, HuffmanTree> FindMinInFrequencyTable()
        {
            var min = _frequencyTable.ElementAt(0);
            foreach (var pair in _frequencyTable)
            {
                if (pair.Value.Frequency < min.Value.Frequency)
                    min = pair;
            }
            return min;
        } 

        private void SetCodes()
        {
            var maxCodeLength = _root.SetChildCodes();
            _root.FormatChildCodes(maxCodeLength);
            _codeTable = _root.GetChilds(_codeTable);
        }

        private string MakeCode(string message)
        {
            string code = null;
            foreach (var ch in message)
            {
                code += _codeTable[ch.ToString()];
            }
            return code;
        }
    }
}
