using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LichessAnalytics.Console
{
    /// <summary>
    /// This class is responsible for cutting a large string containing multiple Lichess games in PGN format
    /// into individual game strings and parsing them into LichessGame objects. It uses regex to identify
    /// the boundaries between games based on the lichess format, specifically looking for two consecutive new lines as the delimiter.
    /// </summary>
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

            // take all but the last element, which is likely to be incomplete
            gameStrings = gameStrings.Length > 1 ? gameStrings[..^1] : gameStrings;
            foreach (var gameString in gameStrings)
            {
                games.Add(LichessGame.Parse(gameString));
            }

            return games;
        }
    }
}