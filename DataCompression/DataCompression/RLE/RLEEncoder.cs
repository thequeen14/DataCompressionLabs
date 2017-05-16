using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCompression.RLE
{
    public class RLEEncoder : IEncoder
    {
        public void Encode(string file)
        {
            var message = MessageExtractor.ExtractMessage(file);
            var messageAfterBW = ApplyBWTransformation(message);
            var messageAfterRLE = string.Empty;

            int increment = 0;
            for (int i = 0; i < message.Length; i += increment)
            {
                var currentCharacter = messageAfterBW.Key.Substring(i, 1);
                increment = 0;
                var nextCharacterPosition = i + increment + 1;
                if (nextCharacterPosition < message.Length)
                {
                    while (messageAfterBW.Key.Substring(nextCharacterPosition, 1) == currentCharacter)
                    {
                        increment++;
                        nextCharacterPosition++;
                        if (nextCharacterPosition >= message.Length)
                        {
                            break;
                        }
                    }
                }

                if (increment == 0)
                {
                    messageAfterRLE += currentCharacter;
                    increment++;
                }
                else
                {
                    increment++;
                    messageAfterRLE += currentCharacter + increment;
                }
            }
            MessageFileWriter.WriteEncodedMessageToFile("output1.txt", messageAfterRLE);
        }

        private KeyValuePair<string, int> ApplyBWTransformation(string text)
        {
            var table = GetBWTransformationTable(text);
            var lastCharacters = string.Empty;
            int textInTablePosition = -1;

            for (int i = 0; i < table.Length; i++)
            {
                lastCharacters += table[i].Substring(table[i].Length - 1, 1);
                if (table[i] == text)
                {
                    textInTablePosition = i;
                }
            }
            return new KeyValuePair<string, int>(lastCharacters, textInTablePosition);
        }

        private string[] GetBWTransformationTable(string text)
        {
            var BWTable = new string[text.Length];
            BWTable[0] = text;
            for (int i = 1; i < text.Length; i++)
            {
                BWTable[i] = RotateWordByOneCharacter(BWTable[i - 1]);
            }
            Array.Sort(BWTable);
            return BWTable;
        }

        private string RotateWordByOneCharacter(string word)
        {
            return word.Substring(1, word.Length - 1) + word[0];
        }
    }
}
