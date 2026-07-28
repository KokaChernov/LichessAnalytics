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
    
    private static string playerName = "sometimesok";

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

        Console.WriteLine($"Enter player name, current player name is {playerName}");
        // console read into the player name variable, if empty use the default player name


        var inputPlayerName = Console.ReadLine();

        if (!string.IsNullOrEmpty(inputPlayerName))
        {
            // check lichess api for the player name validity by trying to fetch one game, if the response is successful, use the new player name, otherwise keep the default one
            var isValidPlayer = GetLichessPlayerAsync(inputPlayerName).GetAwaiter().GetResult();

            if (isValidPlayer)
            {
                playerName = inputPlayerName;
            }
            else
            {
                Console.WriteLine($"Player '{inputPlayerName}' not found. Using default player '{playerName}'.");
            }
        }


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

            foreach (var game in parsedGames)
            {
                

                if (game.PGN == null) // if game was Abandoned
                {
                    continue;
                }

                var resultForThePlayer = game.White.ToLower() == playerName.ToLower() ? game.WhiteRatingDiff : game.BlackRatingDiff;

                // leave only the positions after the first N full moves of each game (to exclude the opening theory)
                var fens = PgnToFenConverter.ConvertPgnToFen(game.PGN).Skip(2 * numberOfMovesToSkip);

                foreach (var fen in fens)
                {
                    string boardPosition = string.Join(" ", fen.Split(' ')[0..2]);// leave only the board position out of the FEN (e.g. "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1" - take only the piece placement part + next move color)
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

    static async Task<bool> GetLichessPlayerAsync(string playerName)
    {
        using (var client = new HttpClient())
        {
            // var token = Environment.GetEnvironmentVariable("LICHESS_API_TOKEN");
            // if (string.IsNullOrEmpty(token))
            // {
            //     throw new InvalidOperationException("LICHESS_API_TOKEN environment variable is not set");
            // }
            // client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            //need to escape the player name in case it contains special characters
            playerName = Uri.EscapeDataString(playerName);
            var url = $"https://lichess.org/api/user/{playerName}";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) // Too Many Requests
                {
                    return false;
                }
            }
           
            return true;
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