using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parenthetical_Multiplier
{
    class Program
    {
        static void Main(string[] args)
        {
            Stack<List<string>> MyStack = new Stack<List<string>>();

            foreach (List<string> set in ListGenerator("(\\bar{C} + D + E + F)(\\bar{B} + D + E)(\\bar{B}+\\bar{C} + D + \\bar{E} + F) (\\bar{A} + D)(\\bar{A} +  B + \\bar{C} + \\bar{D} + E + F)(\\bar{A} + \\bar{B} + \\bar{D} + E)(\\bar{A} + \\bar{B} + \\bar{C} + \\bar{D} + \\bar{E} + F)"))
            {
                MyStack.Push(set);
            }
            MyStack.Push(new List<string> { "" });

            while (MyStack.Count > 1)
            {
                MyStack.Push(Multiplier(MyStack.Pop(), MyStack.Pop()));
            }
            List<string> output = MyStack.Pop();
            foreach (string s in output)
            {
                Console.WriteLine(s);
            }
            Console.ReadLine();
        }

        public static List<string> Multiplier(List<string> List1, List<string> List2)
        {
            List<string> output = new List<string>();
            foreach (string s in List1)
            {
                foreach (string t in List2)
                {
                    if (string.Compare(s, t) <= 0)
                    {
                        output.Add(s + t);
                    }
                    else
                    {
                        output.Add(t + s);
                    }
                }
            }
            output.Sort(string.Compare);
            return output;
        }

        public static IEnumerable<List<string>> ListGenerator(string EntireFormula)
        {
            List<string> output = new List<string>();
            foreach (string token in GetTokens(EntireFormula))
            {
                if (token == "(")
                {
                    output = new List<string>();
                }
                else if (token == ")")
                {
                    yield return output;
                }
                else
                {
                    output.Add(token);
                }
            }
        }

        private static IEnumerable<string> GetTokens(String formula)
        {
            // My patterns
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String VariablePattern = @"[A-Z](?!})|\\bar{[A-Z]}*";
            Regex rgx = new Regex(VariablePattern + "|" + lpPattern + "|" + rpPattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (Match match in rgx.Matches(formula))
            {
                yield return match.Value;
            }

        }
    }

}
