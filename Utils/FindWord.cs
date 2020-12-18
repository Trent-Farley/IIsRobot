using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IIsRobot.Utils
{
    public class FindWord
    {
        private string _script;
        public FindWord(string script)
        {
            _script = script;
        }
        public string GetThreeWords()
        {
            var words = new List<string>();
            var choice = new Random();
            var allWords = GetWords(_script);
            return allWords[choice.Next(1, allWords.Count())];
        }
        private static List<string> GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w']*\b");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select TrimSuffix(m.Value);

            return words.ToList();
        }

        private static string TrimSuffix(string word)
        {
            int apostropheLocation = word.IndexOf('\'');
            if (apostropheLocation != -1)
            {
                word = word.Substring(0, apostropheLocation);
            }

            return word;
        }
    }
}
