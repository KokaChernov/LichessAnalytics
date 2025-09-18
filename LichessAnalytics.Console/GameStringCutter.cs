using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LichessAnalytics.Console
{
    public class GameStringCutter
    {
        public List<LichessGame> Cut(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new List<LichessGame>();
            }

            var games = new List<LichessGame>();
            
            input = input.Replace("\r\n", "\n");


            var pattern = @"\n\n(?!1)"; // Pattern to match 2 newlines not followed by '1' to cut between games
            
            // Split the input string using the pattern
            string[] gameStrings = Regex.Split(input, pattern, RegexOptions.Multiline);

            foreach (var gameString in gameStrings)
            {
                games.Add(LichessGame.Parse(gameString));
            }

            return games;
        }
    }
}