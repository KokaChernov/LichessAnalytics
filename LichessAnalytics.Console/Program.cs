using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using LichessAnalytics.Console;

class Program
{
    
    readonly static int numberOfGamesPerIteration = 1000;
    readonly static int numberOfIterantions = 150;

    // we consider only unique positions after the first N moves of each game so as to skip the opening theory
    readonly static int numberOfMovesToSkip = 7;
    
    readonly static int numberOfTopPositionsToPresent = 20;
    
    readonly static string playerName = "kokachernov";

    static void Main()
    {
        // count number of each unique position encountered in all games
        Dictionary<string, int> frequentPositions = new Dictionary<string, int>();
        // record which games each position appeared in
        Dictionary<string, List<string>> postitionsToGameReference = new Dictionary<string, List<string>>();
        // cumulative result (sum of rating changes) for each position
        Dictionary<string, int> positionCumulativeResult = new Dictionary<string, int>();
        //var millisecondsSinceEpoch
        // var ticks = 638756064000000000;
        //var utcDateTime = new DateTime(ticks, DateTimeKind.Utc);
        var millisecondsSinceEpoch = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero).ToUnixTimeMilliseconds();
        //var millisecondsSinceEpoch = 1738372104000;

        for (int i = 0; i < numberOfIterantions; i++)
        {
            var lichessGamesString = GetLichessGamesAsync(millisecondsSinceEpoch, numberOfGamesPerIteration).GetAwaiter().GetResult();

            Console.WriteLine("=== New batch of games ===");
            Console.WriteLine($"{lichessGamesString}");

            var parsedGames = new GameStringCutter().Cut(lichessGamesString);

            if (parsedGames.Count == 0)
            {
                Console.WriteLine("No more games found, exiting.");
                break;
            }

            var cutoffDate = parsedGames[parsedGames.Count - 1].UTCDate;
            var cutoffTime = parsedGames[parsedGames.Count - 1].UTCTime;
            millisecondsSinceEpoch = new DateTimeOffset(cutoffDate.Add(TimeSpan.Parse(cutoffTime)), TimeSpan.Zero).ToUnixTimeMilliseconds();

            //Console.WriteLine(lichessGamesString);
            // Record the last game's date and time to use it as the 'until' parameter in the next API call
            // string pattern = @"\[UTCDate\s+""(?<date>[\d\.]+)""\]\s+\[UTCTime\s+""(?<time>[\d:]+)""\]";
            // var datetimematches = Regex.Matches(lichessGamesString, pattern);
            // if (datetimematches.Count > 0)
            // {
            //     var datetimematch = datetimematches[datetimematches.Count - 1];
            //     string dateStr = datetimematch.Groups["date"].Value; // "2025.08.18"
            //     string timeStr = datetimematch.Groups["time"].Value; // "16:36:44"

            //     Console.WriteLine($"Date: {dateStr}");
            //     Console.WriteLine($"Time: {timeStr}");

            //     if (DateTime.TryParseExact(
            //             dateStr + " " + timeStr,
            //             "yyyy.MM.dd HH:mm:ss",
            //             null,
            //             System.Globalization.DateTimeStyles.AdjustToUniversal,
            //             out DateTime dt))
            //     {
            //         Console.WriteLine($"Parsed DateTime (UTC): {dt.AddYears(-1).Ticks}");
            //     }
            //     else
            //     {
            //         break;
            //     }
            //     millisecondsSinceEpoch = new DateTimeOffset(dt, TimeSpan.Zero).ToUnixTimeMilliseconds();
            // }

            foreach (var game in parsedGames)
            {
                var resultForThePlayer = game.White.ToLower() == playerName.ToLower() ? game.WhiteRatingDiff : game.BlackRatingDiff;


                // leave only the positions after the first N full moves of each game (to exclude the opening theory)
                var fens = PgnToFenConverter.ConvertPgnToFen(game.PGN).Skip(2 * numberOfMovesToSkip);

                foreach (var fen in fens)
                {
                    string boardPosition = fen.Split(' ')[0];// leave only the board position out of the FEN (e.g. "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1" - take only the piece placement part)
                    //Console.WriteLine(boardPosition);
                    frequentPositions[boardPosition] = frequentPositions.GetValueOrDefault(boardPosition, 0) + 1;
                    if (!postitionsToGameReference.ContainsKey(boardPosition))
                    {
                        postitionsToGameReference[boardPosition] = new List<string>();
                    }
                    postitionsToGameReference[boardPosition].Add(game.GameId);

                    positionCumulativeResult[boardPosition] = positionCumulativeResult.GetValueOrDefault(boardPosition, 0) + resultForThePlayer;
                }
            }
        }
        Console.WriteLine($"overall {frequentPositions.Count} unique positions found");

        var sorted = frequentPositions.ToList().OrderByDescending(kv => kv.Value);

        foreach (var position in sorted.Take(numberOfTopPositionsToPresent))
        {
            Console.WriteLine($"{position.Key} : {position.Value}: cumulative result {positionCumulativeResult[position.Key]} in games ");
            foreach (var gameId in postitionsToGameReference[position.Key])
            {
                Console.Write($"{gameId} ");
            }
            Console.WriteLine();
        }
    }
    

    static async Task<string> GetLichessGamesAsync(long millisecondsSinceEpoch, int numberOfGamesToFetch = 100)
    {

        using (var client = new HttpClient())
        {
            // var token = Environment.GetEnvironmentVariable("LICHESS_API_TOKEN");
            // if (string.IsNullOrEmpty(token))
            // {
            //     throw new InvalidOperationException("LICHESS_API_TOKEN environment variable is not set");
            // }
            // client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var url = $"https://lichess.org/api/games/user/{playerName}?max={numberOfGamesToFetch}&until={millisecondsSinceEpoch}&perfType=bullet";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests) // Too Many Requests
                {
                    Thread.Sleep(1000); // Wait for 1 second before retrying
                    return await GetLichessGamesAsync(millisecondsSinceEpoch);
                }
            }
            Thread.Sleep(50);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }

}