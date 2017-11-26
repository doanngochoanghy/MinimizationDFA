using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimizationDFA
{
    class Program
    {
        static void InputDFA(string filename,out int[,] transition_table,out int[] final_states)
        {
            string[] lines = System.IO.File.ReadAllLines(@filename);
            string[] line = lines[0].Split(' ');
            int m = int.Parse(line[0]);
            int n = int.Parse(line[1]);
            transition_table = new int[m, n];
            for (int i = 0; i < m; i++)
            {
                line = lines[i + 1].Split(' ');
                for (int j = 0; j < n; j++)
                {
                    transition_table[i, j] = int.Parse(line[j]);
                }
            }
            line = lines[m+1].Split(' ');
            final_states = new int[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                final_states[i] = int.Parse(line[i]);
            }
        }
        static void PrintnDFA(int[,] transitionTable,int[] finalStates)
        {
            for (int i = 0; i < transitionTable.GetLength(0); i++)
            {
                Console.Write("{0} | ", i);
                for (int j = 0; j < transitionTable.GetLength(1); j++)
                {
                    Console.Write(transitionTable[i,j]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("-------");
            for (int i = 0; i < finalStates.Length; i++)
            {
                Console.Write(finalStates[i]);
                Console.Write(" ");
            }
        }
        static void Main(string[] args)
        {
            int[,] transitionTable;
            int[] finalStates;
            InputDFA("input.txt",out transitionTable,out finalStates);
            PrintnDFA(transitionTable, finalStates);
            Console.ReadKey();
        }
    }
}
