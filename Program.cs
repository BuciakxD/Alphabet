using System;
using System.Collections.Generic;
using System.IO;

namespace Prog_2_Alphabet
{

    struct Coordinates
    {
        public int x;
        public int y;
        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    class Program
    {

        static string read_file()
        {
            StreamReader sr = new StreamReader("C:\\Users\\llama\\Desktop\\programmig shit\\C# shit\\Prog_2_KEYBOARD\\text.txt");
            return sr.ReadLine();
        }
        static State find_path(Coordinates curr_possition, State[] states)
        {
            int curr_path;
            int last_path = int.MaxValue;
            State state;
            State final_state = new State(curr_possition, last_path);
            for (int i = 0; i < states.Length; i++)
            {
                state = states[i];
                curr_path = state.move_count + Math.Abs(curr_possition.x-state.possition.x) + Math.Abs(curr_possition.y - state.possition.y);
                if (curr_path < last_path)
                {
                    final_state = new State(curr_possition, curr_path + 1);
                    last_path = curr_path;
                }
            }
            return final_state;
        }
        static int shortest_path(string sentence, Dictionary<char, Letter> alphabet)
        {
            State[] last_states = new State[] { new State(new Coordinates(0, 0), 0) };
            State[] curr_states = new State[] { new State(new Coordinates(0, 0), 0) };
            char curr_char;
            List<Coordinates> char_possitions;

            for (int i = 0; i < sentence.Length; i++)
            {
                curr_char = sentence[i];
                if (alphabet.ContainsKey(curr_char))
                {
                    char_possitions = alphabet[curr_char].return_possitions();
                    curr_states = new State[char_possitions.Count];

                    for (int j = 0; j < char_possitions.Count; j++)
                        curr_states[j] = find_path(char_possitions[j], last_states);
                    last_states = curr_states;
                }
            }

            
            int final_moves_count = curr_states[0].move_count;

            for (int i = 1; i < curr_states.Length; i++)  
            {
                if (final_moves_count > curr_states[i].move_count)
                    final_moves_count = curr_states[i].move_count;
            }

            return final_moves_count;   

        }
        static void Main(string[] args)
        {
            int x = int.Parse(Console.ReadLine());
            int y = int.Parse(Console.ReadLine());
            Dictionary<char, Letter> alphabet = create_dict(x, y);
            string sentence = read_file();
            Console.Write(shortest_path(sentence, alphabet));

        }
        static Dictionary<char, Letter> create_dict(int x, int y)
        {
            Dictionary<char, Letter> dict = new Dictionary<char, Letter>();
            string alphabet = Console.ReadLine();
            int k = 0;
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    char character = alphabet[k];  
                    if (dict.ContainsKey(character))
                    {
                        Letter letter = dict[character];
                        letter.add_new_possition(new Coordinates(i,j));
                        dict[character] = letter;
                    }
                    else
                    {
                        Letter letter = new Letter(character);
                        letter.add_new_possition(new Coordinates(i,j));
                        dict.Add(character, letter);
                    }
                    k++;
                }
            }
            return dict;
        }
    }
    struct State
    {
        public Coordinates possition;
        public int move_count;

        public State(Coordinates possition, int move_count)
        {
            this.possition = possition;
            this.move_count = move_count;
        }
    }
    class Letter
    {
        List<Coordinates> possitions = new List<Coordinates>();
        char letter;

        public Letter(char letter)
        {
            this.letter = letter;
        }

        public void add_new_possition(Coordinates possition)
        {
            possitions.Add(possition);
        }
        public List<Coordinates> return_possitions()
        {
            return possitions;
        }
    }
}