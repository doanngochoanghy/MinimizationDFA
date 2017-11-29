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
        int next0_state;
        int next1_state;
        bool is_final=false;
        public State(int i,int s0,int s1)
        {
            index = i;
            next0_state = s0;
            next1_state = s1;
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

        public int Next0_state
        {
            get {
                return next0_state;
            }

            set {
                next0_state = value;
            }
        }

        public int Next1_state
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
            dfa = new List<State>();
            string[] lines = System.IO.File.ReadAllLines(@filename);
            string[] line = lines[0].Split(' ');
            int m = int.Parse(line[0]);
            for (int i = 0; i < m; i++)
            {
                line = lines[i + 1].Split(' ');
                State s = new State(i,int.Parse(line[0]),int.Parse(line[1]));
                dfa.Add(s);
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
                Console.WriteLine(item.Index.ToString() + "|" + item.Next0_state.ToString() + " " + item.Next1_state.ToString());
            }
        }
        static void CreateMarkTable(out int[,] markTable)
        {

        }
        static void Main(string[] args)
        {
            List<State> DFA = new List<State>();
            InputDFA("input.txt",out DFA);
            PrintnDFA(DFA);
            Console.ReadKey();
            
        }
    }
}
