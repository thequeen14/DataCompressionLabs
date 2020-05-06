using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCompression.FanoShennon
{
    public class FanoShennonEncoder : IEncoder
    {
        //test comment
        public void Encode(string file)
        {
            var message = MessageExtractor.ExtractMessage(file);
            var frequencyTable = SymbolFrequencyCounter.CountFrequenciesForLetters(message);
            var tree = new BinaryTree<int, string>();

            var charSequence = string.Empty;

            foreach (var c in frequencyTable)
            {
                charSequence += c.Key;
            }
            tree.Add(message.Length, charSequence);

            //отдельно выделить случай сообщения из одной повторяющейся буквы
            var currentLevelNode = tree.Root;
            while (tree.CountLeaves(tree.Root) != frequencyTable.Count)
            {
                var currentHalf = currentLevelNode.Key / 2;
                var currentSum = 0;
                var currentPositionOfDivision = 0;
                var firstNodeString = string.Empty;

                while (currentSum <= currentHalf)
                {
                    firstNodeString += frequencyTable.ElementAt(currentPositionOfDivision).Key;
                    currentSum += frequencyTable.ElementAt(currentPositionOfDivision).Value;
                    currentPositionOfDivision++;
                }

                var secondNodeString = currentLevelNode.Value.Substring(currentPositionOfDivision + 1,
                    currentLevelNode.Value.Length - currentPositionOfDivision);

                tree.Add(currentSum, firstNodeString);
                tree.Add(currentLevelNode.Key - currentSum, secondNodeString);

                if (currentLevelNode.Left.Value.Length != 1)
                {
                    currentLevelNode = currentLevelNode.Left;
                }
                else if (currentLevelNode.Right.Value.Length != 1)
                {
                    currentLevelNode = currentLevelNode.Right;
                }
            }
        }
    }
}
