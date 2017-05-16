using DataCompression.Huffman;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var huffman = new HuffmanEncoder();
            huffman.Encode("input.txt");
        }
    }
}
