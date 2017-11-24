using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimizationDFA
{
    class Program
    {
        static void InputDFA(string filename,out int[,] table)
        {
            string[] lines = System.IO.File.ReadAllLines(@filename);
            string[] line = lines[0].Split(' ');
            int m = int.Parse(line[0]);
            int n = int.Parse(line[1]);
            table = new int[m, n];
            for (int i = 0; i < m; i++)
            {
                line = lines[i + 1].Split(' ');
                for (int j = 0; j < n; j++)
                {
                    table[i, j] = int.Parse(line[j]);
                }
            }
        }
        static void Main(string[] args)
        {
            int[,] transitionTable;
            InputDFA("input.txt",out transitionTable);
            foreach (var item in transitionTable)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
}
