using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimizationDFA
{
    class State
    {
        int index;
        State next0_state;
        State next1_state;
        bool is_final=false;
        public State(int i)
        {
            index = i;
        }
        public State()
        {
            index = -1;
            next0_state = null;
            next1_state = null;
        }
        public int Index
        {
            get {
                return index;
            }

            set {
                index = value;
            }
        }

        public State Next0_state
        {
            get {
                return next0_state;
            }

            set {
                next0_state = value;
            }
        }

        public State Next1_state
        {
            get {
                return next1_state;
            }

            set {
                next1_state = value;
            }
        }

        public bool Is_final
        {
            get {
                return is_final;
            }

            set {
                is_final = value;
            }
        }
    }
    class Program
    {
        static void InputDFA(string filename,out List<State> dfa)
        {
            string[] lines = System.IO.File.ReadAllLines(@filename);
            string[] line = lines[0].Split(' ');
            int m = int.Parse(line[0]);
            dfa = new List<State>();
            for (int i = 0; i < m; i++)
            {
                dfa.Add(new State(i));
            }
            for (int i = 0; i < m; i++)
            {
                line = lines[i + 1].Split(' ');
                dfa[i].Next0_state = dfa[int.Parse(line[0])];
                dfa[i].Next1_state = dfa[int.Parse(line[1])];
            }
            line = lines[m+1].Split(' ');
            for (int i = 0; i < line.Length; i++)
            {
                dfa[int.Parse(line[i])].Is_final = true;
            }
        }
        static void PrintnDFA(List<State> dfa)
        {
            foreach (var item in dfa)
            {
                Console.WriteLine(item.Index.ToString() + "|" + item.Next0_state.Index.ToString() + " " + item.Next1_state.Index.ToString());
            }
        }
        static void CreateMarkTable(List<State> dfa,out bool[,] markTable)
        {
            markTable = new bool[dfa.Count, dfa.Count];
            int n = dfa.Count;
            bool done = false;
            for (int i = 1; i < n; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (dfa[i].Is_final!=dfa[j].Is_final)
                    {
                        markTable[i, j] = true;
                        markTable[j, i] = true;
                    }
                }
            }
            while (!done)
            {
                done = true;
                for (int i = 1; i < n; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (!markTable[i,j])
                        {
                            if (markTable[dfa[i].Next0_state.Index,dfa[j].Next0_state.Index]||markTable[dfa[i].Next1_state.Index,dfa[j].Next1_state.Index])
                            {
                                markTable[i, j] = true;
                                markTable[j, i] = true;
                                done = false;
                            }
                        }
                    }
                }
            }
        }
        static void MinimizeDFA(List<State> dfa,bool[,] markTable,out List<State> newDFA)
        {
            
            int n = markTable.GetLength(0);
            bool addNewState;
            int numNewState = 0;
            int[] indexTable = new int[n];
            for (int i = 0; i < n; i++)
            {
                addNewState = true;
                for (int j = 0; j < i; j++)
                {
                    if (markTable[i,j]==false)
                    {
                        addNewState = false;
                        indexTable[i] = indexTable[j];
                        break;
                    } 
                }
                if (addNewState)
                {
                    indexTable[i] = numNewState;
                    numNewState += 1;
                }
            }
            newDFA = new List<State>();
            for (int i = 0; i < numNewState; i++)
            {
                newDFA.Add( new State(i));
            }
            for (int i = 0; i < n; i++)
            {
                newDFA[indexTable[i]].Next0_state = newDFA[indexTable[dfa[i].Next0_state.Index]];
                newDFA[indexTable[i]].Next1_state = newDFA[indexTable[dfa[i].Next1_state.Index]];
            }

        }
        static void Main(string[] args)
        {
            List<State> DFA;
            bool[,] MarkTable;
            InputDFA("input4.txt", out DFA);
            PrintnDFA(DFA);
            CreateMarkTable(DFA, out MarkTable);
            List<State> newDFA;
            MinimizeDFA(DFA, MarkTable, out newDFA);
            PrintnDFA(newDFA);
            Console.ReadKey();
        }
    }
}

