using System;
using System.Text.RegularExpressions;


namespace LichessAnalytics.Console
{
    public class LichessGame
    {
        public string Event { get; set; }
        public string Site { get; set; }
        public DateTime Date { get; set; }
        public string White { get; set; }
        public string Black { get; set; }
        public string Result { get; set; }
        public string GameId { get; set; }
        public DateTime UTCDate { get; set; }
        public string UTCTime { get; set; }
        public int WhiteElo { get; set; }
        public int BlackElo { get; set; }
        public int WhiteRatingDiff { get; set; }
        public int BlackRatingDiff { get; set; }
        public string BlackTitle { get; set; }
        public string BlackFideId { get; set; }
        public string Variant { get; set; }
        public string TimeControl { get; set; }
        public string ECO { get; set; }
        public string Termination { get; set; }

        public string PGN { get; set; }

        private static readonly Regex TagPattern = new Regex(@"\[(\w+)\s+""(.+?)""\]");
        private static readonly Regex PGNPattern = new Regex(@"1\..+");

        public static LichessGame Parse(string lichessGameString)
        {
            var game = new LichessGame();
            var matches = TagPattern.Matches(lichessGameString);

            foreach (Match match in matches)
            {
                var tagName = match.Groups[1].Value;
                var tagValue = match.Groups[2].Value;

                switch (tagName)
                {
                    case "Event": game.Event = tagValue; break;
                    case "Site": game.Site = tagValue; break;
                    case "Date": game.Date = DateTime.Parse(tagValue.Replace(".", "-")); break;
                    case "White": game.White = tagValue; break;
                    case "Black": game.Black = tagValue; break;
                    case "Result": game.Result = tagValue; break;
                    case "GameId": game.GameId = tagValue; break;
                    case "UTCDate": game.UTCDate = DateTime.Parse(tagValue.Replace(".", "-")); break;
                    case "UTCTime": game.UTCTime = tagValue; break;
                    case "WhiteElo": game.WhiteElo = int.Parse(tagValue); break;
                    case "BlackElo": game.BlackElo = int.Parse(tagValue); break;
                    case "WhiteRatingDiff": game.WhiteRatingDiff = int.Parse(tagValue); break;
                    case "BlackRatingDiff": game.BlackRatingDiff = int.Parse(tagValue.TrimStart('+')); break;
                    case "BlackTitle": game.BlackTitle = tagValue; break;
                    case "BlackFideId": game.BlackFideId = tagValue; break;
                    case "Variant": game.Variant = tagValue; break;
                    case "TimeControl": game.TimeControl = tagValue; break;
                    case "ECO": game.ECO = tagValue; break;
                    case "Termination": game.Termination = tagValue; break;
                }
            }
            var pgnMatch = PGNPattern.Match(lichessGameString);

            if (pgnMatch.Success)
            {
                game.PGN = pgnMatch.Value;
                return game;
            }
            
            return game;
        }
    }
}