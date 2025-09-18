using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

class Program
{
    
    readonly static int numberOfGamesPerIteration = 10;
    readonly static int numberOfIterantions = 15;

    // we consider only unique positions after the first N moves of each game so as to skip the opening theory
    readonly static int numberOfMovesToSkip = 10;
    
    readonly static int numberOfTopPositionsToPresent = 10;
    static void Main()
    {

        Dictionary<string, int> frequentPositions = new Dictionary<string, int>();

        var millisecondsSinceEpoch = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero).ToUnixTimeMilliseconds();
        
        for (int i = 0; i < numberOfIterantions; i++)
        {
            var lichessGames = GetLichessGamesAsync(millisecondsSinceEpoch, numberOfGamesPerIteration).GetAwaiter().GetResult();

            Console.WriteLine("=== New batch of games ===");
            Console.WriteLine(lichessGames);
            // Record the last game's date and time to use it as the 'until' parameter in the next API call
            string pattern = @"\[UTCDate\s+""(?<date>[\d\.]+)""\]\s+\[UTCTime\s+""(?<time>[\d:]+)""\]";
            var datetimematches = Regex.Matches(lichessGames, pattern);
            if (datetimematches.Count > 0)
            {
                var datetimematch = datetimematches[datetimematches.Count - 1];
                string dateStr = datetimematch.Groups["date"].Value; // "2025.08.18"
                string timeStr = datetimematch.Groups["time"].Value; // "16:36:44"

                Console.WriteLine($"Date: {dateStr}");
                Console.WriteLine($"Time: {timeStr}");

                if (DateTime.TryParseExact(
                        dateStr + " " + timeStr,
                        "yyyy.MM.dd HH:mm:ss",
                        null,
                        System.Globalization.DateTimeStyles.AdjustToUniversal,
                        out DateTime dt))
                {
                    Console.WriteLine($"Parsed DateTime (UTC): {dt.AddYears(-1).Ticks}");
                }
                else
                {
                    break;
                }
                millisecondsSinceEpoch = new DateTimeOffset(dt, TimeSpan.Zero).ToUnixTimeMilliseconds();
            }



            string patternToRemoveMetadata = @"^.*[\[\]\(\)\{\}].*$(\r?\n)?";
            lichessGames = Regex.Replace(lichessGames, patternToRemoveMetadata, string.Empty, RegexOptions.Multiline);

            string patternFinalResult = @"\s*(1-0|0-1|1/2-1/2|\*)";
            lichessGames = Regex.Replace(lichessGames, patternFinalResult, string.Empty, RegexOptions.Multiline);

            //Console.WriteLine(lichessGames);

            foreach (var game in lichessGames.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                // filter out only the positions after the first 10 full moves of each game
                var fens = PgnToFenConverter.ConvertPgnToFen(game).Skip(2 * numberOfMovesToSkip);

                foreach (var fen in fens)
                {
                    string boardPosition = fen.Split(' ')[0];// leave only the board position out of the FEN (e.g. "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
                    //Console.WriteLine(boardPosition);
                    frequentPositions[boardPosition] = frequentPositions.GetValueOrDefault(boardPosition, 0) + 1;
                }
            }
        }
        Console.WriteLine($"overall {frequentPositions.Count} unique positions found");
        
        var sorted = frequentPositions.ToList().OrderByDescending(kv => kv.Value);
        
        foreach (var position in sorted.Take(numberOfTopPositionsToPresent))
        {
            Console.WriteLine($"{position.Key} : {position.Value}");
        }
    }
    

    static async Task<string> GetLichessGamesAsync(long millisecondsSinceEpoch, int numberOfGamesToFetch = 100)
    {

        using (var client = new HttpClient())
        {
            var token = Environment.GetEnvironmentVariable("LICHESS_API_TOKEN");
            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("LICHESS_API_TOKEN environment variable is not set");
            }
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var url = $"https://lichess.org/api/games/user/kokachernov?max={numberOfGamesToFetch}&until={millisecondsSinceEpoch}&perfType=bullet";
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