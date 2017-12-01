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
        /// <summary>
        /// Hàm nhập vào DFA từ file.
        /// </summary>
        /// <param name="filename">Đường dẫn đến file input</param>
        /// <param name="dfa">DFA output từ file </param>
        static void InputDFA(string filename,out List<State> dfa)
        {
            /*
             * File DFA
             * Dòng 1 số m là số trạng thái của DFA
             * m dòng tiếp theo dòng thứ i chứa 2 chỉ số trạng thái của DFA sau khi nhập vào kí tự 0 và 1 khi DFA ở trạng thái i.
             * dòng m+2 chỉ số các trạng thái kết thúc của DFA
             */
            string[] lines = System.IO.File.ReadAllLines(@filename);
            string[] line = lines[0].Split(' ');
            int m = int.Parse(line[0]);//Số trạng thái DFA
            dfa = new List<State>();//List trạng thái DFA
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
            for (int i = 0; i < line.Length; i++)//Nhập các trạng thái kết thúc DFA
            {
                dfa[int.Parse(line[i])].Is_final = true;
            }
        }
        /// <summary>
        /// In DFA ra thành bảng chuyển trạng thái
        /// </summary>
        /// <param name="dfa"></param>
        static void PrintnDFA(List<State> dfa)
        {
            Console.WriteLine(" |0 1");
            Console.WriteLine("_____");
            foreach (State s in dfa)
            {
                Console.WriteLine(s.Index.ToString() + "|" + s.Next0_state.Index.ToString() + " " + s.Next1_state.Index.ToString());
            }
        }
        /// <summary>
        /// Tạo bảng đánh dấu các trạng thái tương đương. markTable[i,j] = False <=> 2 trạng thái i,j tương đương
        /// </summary>
        /// <param name="dfa">DFA</param>
        /// <param name="markTable">MarkTable</param>
        static void CreateMarkTable(List<State> dfa,out bool[,] markTable)
        {
            markTable = new bool[dfa.Count, dfa.Count];//Tạo mảng 2 chiều giá trị False
            int n = dfa.Count;
            bool done = false;
            for (int i = 1; i < n; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (dfa[i].Is_final!=dfa[j].Is_final)//Đánh dấu các trạng thái không cùng kết thúc.
                    {
                        markTable[i, j] = true;
                        markTable[j, i] = true;
                    }
                }
            }
            while (!done)//Lặp lại các bước đánh dấu bảng DFA cho đến khi bảng đánh dấu không còn thay đổi
            {
                done = true;
                for (int i = 1; i < n; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (!markTable[i,j])//Nếu 2 trạng thái i,j là đang tương đương
                        {
                            //Kiểm tra trạng thái tiếp theo của i,j khi nhập vào 1 kí tự 0|1 có tương được hay ko
                            if (markTable[dfa[i].Next0_state.Index,dfa[j].Next0_state.Index]||markTable[dfa[i].Next1_state.Index,dfa[j].Next1_state.Index])
                            {
                                markTable[i, j] = true;//Đánh dấu true cho i,j là ko tương đương
                                markTable[j, i] = true;
                                done = false;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Cực tiểu hóa DFA dựa vào DFA và bảng đánh dấu các trạng thái tương đương.
        /// </summary>
        /// <param name="dfa">DFA</param>
        /// <param name="markTable">MarkTable</param>
        /// <param name="newDFA">New DFA</param>
        static void MinimizeDFA(List<State> dfa,out List<State> newDFA)
        {
            bool[,] markTable;
            CreateMarkTable(dfa, out markTable);
            int n = markTable.GetLength(0);
            bool addNewState;
            int numNewState = 0;//Số trạng thái của DFA mới.
            int[] indexTable = new int[n];//Ánh xạ chỉ số DFA cũ sang DFA mới.
            for (int i = 0; i < n; i++)
            {
                addNewState = true;
                for (int j = 0; j < i; j++)
                {
                    if (markTable[i,j]==false)
                    {
                        addNewState = false;//Nếu i,j tương đương thì không thêm trạng thái i cho DFA vì đã có trạng thái j được thêm vào từ trước vì f<i
                        indexTable[i] = indexTable[j];
                        break;
                    } 
                }
                if (addNewState)//Nếu i,j ko tương đương thì thêm trạng thái i vào DFA mới với chỉ số bằng độ dài DFA mới hiện tại.
                {
                    indexTable[i] = numNewState;
                    numNewState += 1;
                }
            }
            newDFA = new List<State>();
            for (int i = 0; i < numNewState; i++)//Tạo DFA mới với số trạng thái mới.
            {
                newDFA.Add( new State(i));
            }
            for (int i = 0; i < n; i++)//Duyệt tất cả DFA cũ, ánh xạ các chuyển trạng thái của DFA cũ sang trạng thái của DFA mới.
            {
                newDFA[indexTable[i]].Next0_state = newDFA[indexTable[dfa[i].Next0_state.Index]];
                newDFA[indexTable[i]].Next1_state = newDFA[indexTable[dfa[i].Next1_state.Index]];
            }

        }
        static void Main(string[] args)
        {
            List<State> DFA;
            InputDFA("input5.txt", out DFA);//Nhập DFA
            Console.WriteLine("Input DFA:");
            PrintnDFA(DFA);
            List<State> newDFA;
            MinimizeDFA(DFA, out newDFA);//Cực tiểu hóa DFA
            Console.WriteLine("Output DFA:");
            PrintnDFA(newDFA);
            Console.ReadKey();
        }
    }
}

