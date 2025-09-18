using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using LichessAnalytics.Console;


public class LichessGameTests
{
    [Fact]
    public void Parse_ValidLichessGame_ReturnsCorrectlyPopulatedObject()
    {
        // Arrange
        string gameString = @"[Event ""Rated Blitz game""]
[Site ""https://lichess.org/abc123def""]
[Date ""2023.10.15""]
[White ""Player1""]
[Black ""Player2""]
[Result ""1-0""]
[GameId ""abc123def""]
[UTCDate ""2023.10.15""]
[UTCTime ""14:23:45""]
[WhiteElo ""1854""]
[BlackElo ""1823""]
[WhiteRatingDiff ""+7""]
[BlackRatingDiff ""-7""]
[BlackTitle ""FM""]
[BlackFideId ""12345678""]
[Variant ""Standard""]
[TimeControl ""3+2""]
[ECO ""B20""]
[Termination ""Normal""]

1. b3 e6 2. Bb2 d6 3. e3 c6 4. a3 f6 5. d4 g6 6. Nf3 Nh6 7. Be2 Nf7 8. c4 Bg7 9. b4 O-O 10. Nc3 Na6 11. O-O Nc7 12. a4 b6 13. b5 Bb7 14. bxc6 Bxc6 15. Na2 Qd7 16. Nb4 Bb7 17. Nd3 d5 18. cxd5 Bxd5 19. Nb4 Bb7 20. Rc1 Rac8 21. Qd2 Nd6 22. Rc2 Ne4 23. Qd3 Na8 24. Rfc1 Rxc2 25. Rxc2 Rc8 26. Rxc8+ Qxc8 27. h3 Qd7 28. Na6 Qxa4 29. Bc1 Qa1 30. Qd2 Bxa6 31. Qc2 Bxe2 32. Qxe2 Qxc1+ 33. Kh2 Nc3 34. Qd3 Nc7 35. Nd2 f5 36. Nc4 N7d5 37. Ne5 Bxe5+ 38. dxe5 Ne4 39. Qe2 Qc7 40. f3 Qxe5+ 41. f4 Qc7 42. g3 Qc3 43. Qa2 Qxe3 44. Qxa7 Qxg3+ 45. Kh1 Nf2# 0-1";

        // Act
        var game = LichessGame.Parse(gameString);

        // Assert

        Assert.Equal("Rated Blitz game", game.Event);
        Assert.Equal("https://lichess.org/abc123def", game.Site);
        Assert.Equal(new DateTime(2023, 10, 15), game.Date);
        Assert.Equal("Player1", game.White);
        Assert.Equal("Player2", game.Black);
        Assert.Equal("1-0", game.Result);
        Assert.Equal("abc123def", game.GameId);
        Assert.Equal(new DateTime(2023, 10, 15), game.UTCDate);
        Assert.Equal("14:23:45", game.UTCTime);
        Assert.Equal(1854, game.WhiteElo);
        Assert.Equal(1823, game.BlackElo);
        Assert.Equal(7, game.WhiteRatingDiff);
        Assert.Equal(-7, game.BlackRatingDiff);
        Assert.Equal("FM", game.BlackTitle);
        Assert.Equal("12345678", game.BlackFideId);
        Assert.Equal("Standard", game.Variant);
        Assert.Equal("3+2", game.TimeControl);
        Assert.Equal("B20", game.ECO);
        Assert.Equal("Normal", game.Termination);
        Assert.Equal("1. b3 e6 2. Bb2 d6 3. e3 c6 4. a3 f6 5. d4 g6 6. Nf3 Nh6 7. Be2 Nf7 8. c4 Bg7 9. b4 O-O 10. Nc3 Na6 11. O-O Nc7 12. a4 b6 13. b5 Bb7 14. bxc6 Bxc6 15. Na2 Qd7 16. Nb4 Bb7 17. Nd3 d5 18. cxd5 Bxd5 19. Nb4 Bb7 20. Rc1 Rac8 21. Qd2 Nd6 22. Rc2 Ne4 23. Qd3 Na8 24. Rfc1 Rxc2 25. Rxc2 Rc8 26. Rxc8+ Qxc8 27. h3 Qd7 28. Na6 Qxa4 29. Bc1 Qa1 30. Qd2 Bxa6 31. Qc2 Bxe2 32. Qxe2 Qxc1+ 33. Kh2 Nc3 34. Qd3 Nc7 35. Nd2 f5 36. Nc4 N7d5 37. Ne5 Bxe5+ 38. dxe5 Ne4 39. Qe2 Qc7 40. f3 Qxe5+ 41. f4 Qc7 42. g3 Qc3 43. Qa2 Qxe3 44. Qxa7 Qxg3+ 45. Kh1 Nf2# 0-1", game.PGN);
    }

        [Fact]
    public void Cut_10ValidLichessGames_Into_ArrayOfObjects()
    {
        // Arrange
        string gameString = @"[Event ""casual bullet game""]
[Site ""https://lichess.org/5UNdPqxo""]
[Date ""2025.09.17""]
[White ""kokachernov""]
[Black ""pipi_pamperz""]
[Result ""0-1""]
[GameId ""5UNdPqxo""]
[UTCDate ""2025.09.17""]
[UTCTime ""07:16:01""]
[WhiteElo ""1930""]
[BlackElo ""1910""]
[Variant ""Standard""]
[TimeControl ""60+0""]
[ECO ""A01""]
[Termination ""Time forfeit""]

1. b3 c5 2. Bb2 d6 3. e3 Nf6 4. Nf3 e6 5. d4 Be7 6. dxc5 O-O 7. a3 dxc5 8. b4 cxb4 9. axb4 a6 10. c3 b5 11. Nbd2 Bb7 12. Nb3 Nc6 13. Qxd8 Rfxd8 14. Be2 Rd7 15. O-O Rad8 16. Rfd1 h6 17. Rxd7 Rxd7 18. Rd1 g5 19. Rxd7 Nxd7 20. Nbd4 Nxd4 21. Nxd4 Nb6 22. Nb3 Nc4 23. Bxc4 bxc4 24. Nd4 e5 25. Nc2 Kg7 26. Bc1 f5 27. f3 Kf6 28. g3 e4 29. f4 Bd6 30. Bd2 Bc7 31. Nd4 a5 32. bxa5 Bxa5 33. Nb5 Ba6 34. Nd6 Ke6 35. Ne8 Bb5 36. Ng7+ Kf6 37. fxg5+ Kxg7 38. gxh6+ Kxh6 39. h4 Kh5 40. Kg2 Kg4 41. Kf2 Bc7 42. Bc1 Bxg3+ 43. Kf1 Bxh4 44. Ba3 Bg5 45. Bd6 Kf3 46. Bh2 Bxe3 47. Bd6 Bd2 48. Bc5 Bxc3 49. Bf2 Bd2 50. Bb6 Be3 51. Bxe3 Kxe3 52. Ke1 c3 53. Kd1 f4 54. Kc1 f3 55. Kc2 f2 56. Kb3 f1=Q 57. Kxc3 Qd3+ 58. Kb4 Kd2 59. Kc5 e3 60. Kb6 e2 61. Ka5 e1=Q 62. Kb6 Bc4 63. Ka5 Bd5 64. Kb6 Bf3 65. Kc5 Ke2 66. Kb6 Kf2 67. Kc5 Qdd2 0-1

[Event ""rated bullet game""]
[Site ""https://lichess.org/0sD0ybyN""]
[Date ""2025.09.17""]
[White ""kokachernov""]
[Black ""TortySaar""]
[Result ""0-1""]
[GameId ""0sD0ybyN""]
[UTCDate ""2025.09.17""]
[UTCTime ""07:10:21""]
[WhiteElo ""1936""]
[BlackElo ""1911""]
[WhiteRatingDiff ""-6""]
[BlackRatingDiff ""+6""]
[Variant ""Standard""]
[TimeControl ""60+0""]
[ECO ""A01""]
[Termination ""Normal""]

1. b3 g6 2. Bb2 Nf6 3. e3 Bg7 4. d4 O-O 5. a3 d5 6. c4 c6 7. c5 Nbd7 8. b4 Re8 9. Nf3 Qc7 10. Be2 e5 11. O-O e4 12. Nfd2 Nf8 13. Nc3 Ne6 14. b5 Nd7 15. bxc6 bxc6 16. a4 Rb8 17. Rb1 Rf8 18. a5 Qxa5 19. Qc2 f5 20. Nd1 f4 21. Bc3 Qc7 22. Rxb8 Qxb8 23. exf4 Nxf4 24. Nb1 Nxe2+ 25. Qxe2 Qxb1 26. Ne3 Qd3 27. Re1 Qxc3 28. g3 Bxd4 29. Kg2 Bxc5 30. Ng4 Nf6 31. Ne5 Qxe5 32. Qa2 Ng4 33. h3 Rxf2+ 34. Qxf2 Nxf2 35. Rf1 Nd3 36. h4 Qe6 37. h5 Qh3# 0-1

[Event ""rated bullet game""]
[Site ""https://lichess.org/Xy7UbifU""]
[Date ""2025.09.17""]
[White ""MohanishV""]
[Black ""kokachernov""]
[Result ""0-1""]
[GameId ""Xy7UbifU""]
[UTCDate ""2025.09.17""]
[UTCTime ""07:07:48""]
[WhiteElo ""1881""]
[BlackElo ""1931""]
[WhiteRatingDiff ""-4""]
[BlackRatingDiff ""+5""]
[Variant ""Standard""]
[TimeControl ""60+0""]
[ECO ""E60""]
[Termination ""Time forfeit""]

1. d4 Nf6 2. c4 g6 3. e3 d6 4. Nf3 Bg7 5. Be2 O-O 6. O-O Re8 7. Nc3 e6 8. b3 Nbd7 9. Bb2 b6 10. Rc1 Bb7 11. Re1 e5 12. dxe5 dxe5 13. Nd2 e4 14. Qc2 Nc5 15. Ba3 Nd3 16. Bxd3 exd3 17. Qd1 c5 18. Nb5 Ne4 19. Nxe4 Rxe4 20. Qd2 Re8 21. Rcd1 Be4 22. Nc3 Bf5 23. e4 Bg4 24. f3 Be6 25. Qxd3 Qxd3 26. Rxd3 Rad8 27. Rxd8 Rxd8 28. Bb2 Bd4+ 29. Kf1 f6 30. Na4 Bc8 31. Bxd4 Rxd4 32. Ke2 Bd7 33. Rd1 Bxa4 34. Rxd4 cxd4 35. bxa4 Kf7 36. Kd3 Ke6 37. Kxd4 Kd6 38. g4 h6 39. h4 a6 40. f4 a5 41. a3 h5 42. gxh5 gxh5 43. e5+ fxe5+ 44. fxe5+ Ke6 45. Ke4 Ke7 46. Kf5 Kd7 47. e6+ Ke7 48. Ke5 Ke8 49. Kf6 Kf8 50. e7+ Ke8 51. Ke6 b5 52. Kd5 bxa4 53. Kc5 Kxe7 54. Kb5 Kf6 55. Kxa5 Kf5 56. Kxa4 Kg4 57. Kb5 Kxh4 58. c5 Kg3 59. c6 h4 60. c7 h3 0-1

[Event ""rated bullet game""]
[Site ""https://lichess.org/3S9z9v7d""]
[Date ""2025.09.17""]
[White ""kokachernov""]
[Black ""shiftsayan""]
[Result ""0-1""]
[GameId ""3S9z9v7d""]
[UTCDate ""2025.09.17""]
[UTCTime ""07:06:14""]
[WhiteElo ""1936""]
[BlackElo ""1981""]
[WhiteRatingDiff ""-5""]
[BlackRatingDiff ""+5""]
[Variant ""Standard""]
[TimeControl ""60+0""]
[ECO ""A01""]
[Termination ""Normal""]

1. b3 e5 2. Bb2 Bc5 3. e3 Nf6 4. a3 a6 5. d4 exd4 6. exd4 Ba7 7. c4 O-O 8. Nf3 d5 9. Be2 dxc4 10. bxc4 Bg4 11. O-O Bxf3 12. Bxf3 Nc6 13. d5 Ne5 14. Be2 Ng6 15. Kh1 c6 16. dxc6 Qxd1 17. Rxd1 bxc6 18. Bf3 Rac8 19. Nc3 Rfd8 20. Ne4 Nxe4 21. Bxe4 Rxd1+ 22. Rxd1 c5 23. Bb7 Rb8 24. Bxa6 Rxb2 25. h3 h6 26. Bb5 Kh7 27. a4 Bb8 28. Rd8 Be5 29. Rc8 Rb1# 0-1

[Event ""rated bullet game""]
[Site ""https://lichess.org/Gp63BRwE""]
[Date ""2025.09.17""]
[White ""Nyondoh""]
[Black ""kokachernov""]
[Result ""0-1""]
[GameId ""Gp63BRwE""]
[UTCDate ""2025.09.17""]
[UTCTime ""07:04:13""]
[WhiteElo ""1916""]
[BlackElo ""1931""]
[WhiteRatingDiff ""-6""]
[BlackRatingDiff ""+5""]
[Variant ""Standard""]
[TimeControl ""60+0""]
[ECO ""A45""]
[Termination ""Time forfeit""]

1. d4 Nf6 2. g3 g6 3. Bg2 d6 4. Nf3 Bg7 5. O-O O-O 6. a3 Re8 7. b4 Nbd7 8. Bb2 Rb8 9. Nbd2 b6 10. c4 Bb7 11. Qb3 c5 12. dxc5 dxc5 13. b5 e6 14. a4 e5 15. Rad1 e4 16. Ne1 Nh5 17. e3 Bxb2 18. Qxb2 Ndf6 19. h3 Ng7 20. g4 h5 21. g5 Nh7 22. h4 Nf5 23. f3 Nxh4 24. fxe4 Bxe4 25. Nxe4 Qe7 26. Nf6+ Nxf6 27. gxf6 Qe6 28. e4 Rbd8 29. Rxd8 Rxd8 0-1

[Event ""rated bullet game""]
[Site ""https://lichess.org/SWPNsDSz""]
[Date ""2025.09.17""]
[White ""gekon16""]
[Black ""kokachernov""]
[Result ""1-0""]
[GameId ""SWPNsDSz""]
[UTCDate ""2025.09.17""]
[UTCTime ""07:03:02""]
[WhiteElo ""1989""]
[BlackElo ""1936""]
[WhiteRatingDiff ""+5""]
[BlackRatingDiff ""-5""]
[Variant ""Standard""]
[TimeControl ""60+0""]
[ECO ""B00""]
[Termination ""Normal""]

1. e4 Nc6 2. Nf3 e6 3. d4 g6 4. d5 exd5 5. exd5 Nce7 6. Qe2 d6 7. Bg5 f6 8. Be3 g5 9. Nc3 h5 10. Nd4 Bg4 11. Qb5+ Bd7 12. Qxb7 Bh6 13. Ncb5 c6 14. dxc6 Nxc6 15. Nxc6 Bxc6 16. Qxc6+ Qd7 17. Qxa8+ Kf7 18. Qxa7 Qxa7 19. Bxa7 Ne7 20. Nxd6+ Kg6 21. Ne4 Re8 22. O-O-O Nf5 23. Bd3 Nd4 24. Nd6+ Kg7 25. Nxe8+ 1-0

[Event ""rated bullet game""]
[Site ""https://lichess.org/bfZRNrbG""]
[Date ""2025.09.17""]
[White ""kokachernov""]
[Black ""Sectusempraaaa""]
[Result ""1-0""]
[GameId ""bfZRNrbG""]
[UTCDate ""2025.09.17""]
[UTCTime ""06:08:54""]
[WhiteElo ""1931""]
[BlackElo ""1887""]
[WhiteRatingDiff ""+5""]
[BlackRatingDiff ""-27""]
[Variant ""Standard""]
[TimeControl ""60+0""]
[ECO ""A01""]
[Termination ""Time forfeit""]

1. b3 d5 2. Bb2 Nc6 3. e3 e5 4. a3 Nf6 5. d4 e4 6. Ne2 Bg4 7. c4 dxc4 8. bxc4 Bd6 9. Qc2 O-O 10. h3 Bxe2 11. Bxe2 Be7 12. Nd2 Na5 13. Nxe4 Nxe4 14. Qxe4 Re8 15. Qc2 Bf6 16. O-O c5 17. d5 Bxb2 18. Qxb2 b6 19. Bf3 Nb7 20. d6 Nxd6 21. Bxa8 Nxc4 22. Qc3 Qxa8 23. Qxc4 Qc6 24. Qf4 Re4 25. Qf3 Qf6 26. Qxf6 gxf6 27. Rfd1 Ra4 28. Rd8+ Kg7 29. Rd7 c4 30. Rd4 b5 31. Kh2 Ra6 32. a4 Rc6 33. axb5 Rc5 34. Rc1 c3 35. Rd3 c2 36. Rd2 Rxb5 37. Rdxc2 a5 38. Ra2 Rc5 39. Rca1 a4 40. Rxa4 Rc4 41. Rxc4 1-0

[Event ""rated bullet game""]
[Site ""https://lichess.org/9SmNxZg9""]
[Date ""2025.09.17""]
[White ""TaroonVACA""]
[Black ""kokachernov""]
[Result ""0-1""]
[GameId ""9SmNxZg9""]
[UTCDate ""2025.09.17""]
[UTCTime ""06:06:40""]
[WhiteElo ""1875""]
[BlackElo ""1926""]
[WhiteRatingDiff ""-10""]
[BlackRatingDiff ""+5""]
[Variant ""Standard""]
[TimeControl ""60+0""]
[ECO ""B00""]
[Termination ""Time forfeit""]

1. e4 Nc6 2. Nf3 e6 3. d3 g6 4. Nbd2 Bg7 5. g3 Nge7 6. Bg2 O-O 7. c3 Re8 8. O-O b6 9. Re1 Bb7 10. d4 d6 11. e5 d5 12. Nf1 Kf8 13. h4 Ng8 14. N1h2 Nce7 15. Ng4 Nf5 16. Bg5 Qd7 17. Qd2 h6 18. Bf6 h5 19. Bxg7+ Kxg7 20. Nf6 Nxf6 21. exf6+ Kxf6 22. Qg5+ Kg7 23. Ne5 Qe7 24. Qd2 Nd6 25. g4 Ne4 26. Qe3 Qf6 27. gxh5 Qxh4 0-1

[Event ""rated bullet game""]
[Site ""https://lichess.org/7dOpqCM0""]
[Date ""2025.09.17""]
[White ""kokachernov""]
[Black ""TYKING""]
[Result ""0-1""]
[GameId ""7dOpqCM0""]
[UTCDate ""2025.09.17""]
[UTCTime ""06:05:26""]
[WhiteElo ""1931""]
[BlackElo ""1944""]
[WhiteRatingDiff ""-5""]
[BlackRatingDiff ""+6""]
[Variant ""Standard""]
[TimeControl ""60+0""]
[ECO ""A01""]
[Termination ""Normal""]

1. b3 d5 2. Bb2 Bf5 3. e3 Bg6 4. Nf3 Nf6 5. a3 e6 6. d4 Bd6 7. Bd3 c6 8. Bxg6 hxg6 9. c4 Qc7 10. c5 Be7 11. b4 Ne4 12. Nbd2 f5 13. Nxe4 dxe4 14. Ne5 Bf6 15. Qc2 Bxe5 16. dxe5 Nd7 17. f4 exf3 18. gxf3 Nxe5 19. f4 Nf3+ 20. Kf2 Rh3 21. Bxg7 O-O-O 22. Qc3 Rd2+ 23. Kf1 Nxh2+ 24. Kg1 Nf3+ 25. Kf1 Rxh1# 0-1

[Event ""casual bullet game""]
[Site ""https://lichess.org/kI9wEZ4l""]
[Date ""2025.09.17""]
[White ""JD_Lister""]
[Black ""kokachernov""]
[Result ""0-1""]
[GameId ""kI9wEZ4l""]
[UTCDate ""2025.09.17""]
[UTCTime ""05:56:09""]
[WhiteElo ""1801""]
[BlackElo ""1931""]
[Variant ""Standard""]
[TimeControl ""60+0""]
[ECO ""B02""]
[Termination ""Time forfeit""]

1. e4 Nf6 2. Nc3 Ng8 3. Nf3 d6 4. g3 e5 5. Bg2 Nf6 6. O-O g6 7. d3 Bg7 8. Ne2 Nbd7 9. Ng5 O-O 10. Nh3 h6 11. Rb1 Nb6 12. f4 exf4 13. Nhxf4 Nh7 14. c3 g5 15. Nh5 Bd7 16. Nxg7 Kxg7 17. Nd4 f6 18. Nf5+ Bxf5 19. Rxf5 Nd7 20. Qb3 Ne5 21. Qxb7 Qb8 22. Qd5 Qb6+ 23. d4 Nd3 24. b3 Nxc1 25. Rxc1 a5 26. Rcf1 a4 27. h3 axb3 28. axb3 Qa5 29. Qxa5 Rxa5 30. Rxa5 Rf7 31. Ra7 Nf8 32. b4 Ne6 33. Rfa1 h5 34. Rb7 h4 35. g4 Nf4 0-1";

        // Act
        var gameCutter = new GameStringCutter();
        var games = gameCutter.Cut(gameString);
        Assert.Equal(10, games.Count); // Ensure we have 10 games

        var game = games[1];
        // Assert
        Assert.Equal("rated bullet game", game.Event);
        Assert.Equal("https://lichess.org/0sD0ybyN", game.Site);
        Assert.Equal(new DateTime(2025, 09, 17), game.Date);
        Assert.Equal("kokachernov", game.White);
        Assert.Equal("TortySaar", game.Black);
        Assert.Equal("0-1", game.Result);
        Assert.Equal("0sD0ybyN", game.GameId);
        Assert.Equal(new DateTime(2025, 09, 17), game.UTCDate);
        Assert.Equal("07:10:21", game.UTCTime);
        Assert.Equal(1936, game.WhiteElo);
        Assert.Equal(1911, game.BlackElo);
        Assert.Equal(-6, game.WhiteRatingDiff);
        Assert.Equal(6, game.BlackRatingDiff);
        Assert.Equal("Standard", game.Variant);
        Assert.Equal("60+0", game.TimeControl);
        Assert.Equal("A01", game.ECO);
        Assert.Equal("Normal", game.Termination);
       
        Assert.Equal("1. b3 g6 2. Bb2 Nf6 3. e3 Bg7 4. d4 O-O 5. a3 d5 6. c4 c6 7. c5 Nbd7 8. b4 Re8 9. Nf3 Qc7 10. Be2 e5 11. O-O e4 12. Nfd2 Nf8 13. Nc3 Ne6 14. b5 Nd7 15. bxc6 bxc6 16. a4 Rb8 17. Rb1 Rf8 18. a5 Qxa5 19. Qc2 f5 20. Nd1 f4 21. Bc3 Qc7 22. Rxb8 Qxb8 23. exf4 Nxf4 24. Nb1 Nxe2+ 25. Qxe2 Qxb1 26. Ne3 Qd3 27. Re1 Qxc3 28. g3 Bxd4 29. Kg2 Bxc5 30. Ng4 Nf6 31. Ne5 Qxe5 32. Qa2 Ng4 33. h3 Rxf2+ 34. Qxf2 Nxf2 35. Rf1 Nd3 36. h4 Qe6 37. h5 Qh3# 0-1", game.PGN);
    }

}
