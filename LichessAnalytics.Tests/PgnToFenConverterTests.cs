using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

public class PgnToFenConverterTests
{
    private readonly ITestOutputHelper output;
    public PgnToFenConverterTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void ConvertPgnToFen_EnPassant_PawnMoves_ReturnsCorrectFens()
    {
        string pgn = "1. e4 Nc6 2. d4 e6 3. d5 exd5 4. exd5 Nce7 5. Nf3 d6 6. Be2 g6 7. c4 Bg7 8. Nc3 Nf6 9. Qc2 O-O 10. b3 Re8 11. Bb2 Nexd5 12. cxd5 b6 13. O-O Bd7 14. Rfe1 c5 15. dxc6 Bxc6 16. Bb5 Bxb5 17. Nxb5 Rxe1+ 18. Rxe1 Qd7 19. a4 Rc8 20. Qd2 Nh5 21. Bxg7 Nxg7 22. Qxd6 Rd8 23. Qe5 Ne6 24. h3 Nc5 25. b4 Nxa4 26. Nfd4 Re8 27. Qxe8+ Kg7 28. Qxd7";
        var expected = new List<string>
        {
            "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1",
            "r1bqkbnr/pppppppp/2n5/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq e3 0 2",
            "r1bqkbnr/pppppppp/2n5/8/3PP3/8/PPP2PPP/RNBQKBNR b KQkq d3 0 2",
            "r1bqkbnr/pppp1ppp/2n1p3/8/3PP3/8/PPP2PPP/RNBQKBNR w KQkq d3 0 3",
            "r1bqkbnr/pppp1ppp/2n1p3/3P4/4P3/8/PPP2PPP/RNBQKBNR b KQkq d3 0 3",
            "r1bqkbnr/pppp1ppp/2n5/3p4/4P3/8/PPP2PPP/RNBQKBNR w KQkq d3 0 4",
            "r1bqkbnr/pppp1ppp/2n5/3P4/8/8/PPP2PPP/RNBQKBNR b KQkq d3 0 4",
            "r1bqkbnr/ppppnppp/8/3P4/8/8/PPP2PPP/RNBQKBNR w KQkq d3 0 5",
            "r1bqkbnr/ppppnppp/8/3P4/8/5N2/PPP2PPP/RNBQKB1R b KQkq d3 0 5",
            "r1bqkbnr/ppp1nppp/3p4/3P4/8/5N2/PPP2PPP/RNBQKB1R w KQkq d3 0 6",
            "r1bqkbnr/ppp1nppp/3p4/3P4/8/5N2/PPP1BPPP/RNBQK2R b KQkq d3 0 6",
            "r1bqkbnr/ppp1np1p/3p2p1/3P4/8/5N2/PPP1BPPP/RNBQK2R w KQkq d3 0 7",
            "r1bqkbnr/ppp1np1p/3p2p1/3P4/2P5/5N2/PP2BPPP/RNBQK2R b KQkq c3 0 7",
            "r1bqk1nr/ppp1npbp/3p2p1/3P4/2P5/5N2/PP2BPPP/RNBQK2R w KQkq c3 0 8",
            "r1bqk1nr/ppp1npbp/3p2p1/3P4/2P5/2N2N2/PP2BPPP/R1BQK2R b KQkq c3 0 8",
            "r1bqk2r/ppp1npbp/3p1np1/3P4/2P5/2N2N2/PP2BPPP/R1BQK2R w KQkq c3 0 9",
            "r1bqk2r/ppp1npbp/3p1np1/3P4/2P5/2N2N2/PPQ1BPPP/R1B1K2R b KQkq c3 0 9",
            "r1bq1rk1/ppp1npbp/3p1np1/3P4/2P5/2N2N2/PPQ1BPPP/R1B1K2R w KQkq - 1 10",
            "r1bq1rk1/ppp1npbp/3p1np1/3P4/2P5/1PN2N2/P1Q1BPPP/R1B1K2R b KQkq - 0 10",
            "r1bqr1k1/ppp1npbp/3p1np1/3P4/2P5/1PN2N2/P1Q1BPPP/R1B1K2R w KQkq - 0 11",
            "r1bqr1k1/ppp1npbp/3p1np1/3P4/2P5/1PN2N2/PBQ1BPPP/R3K2R b KQkq - 0 11",
            "r1bqr1k1/ppp2pbp/3p1np1/3n4/2P5/1PN2N2/PBQ1BPPP/R3K2R w KQkq - 0 12",
            "r1bqr1k1/ppp2pbp/3p1np1/3P4/8/1PN2N2/PBQ1BPPP/R3K2R b KQkq - 0 12",
            "r1bqr1k1/p1p2pbp/1p1p1np1/3P4/8/1PN2N2/PBQ1BPPP/R3K2R w KQkq - 0 13",
            "r1bqr1k1/p1p2pbp/1p1p1np1/3P4/8/1PN2N2/PBQ1BPPP/R4RK1 b KQkq - 1 13",
            "r2qr1k1/p1pb1pbp/1p1p1np1/3P4/8/1PN2N2/PBQ1BPPP/R4RK1 w KQkq - 0 14",
            "r2qr1k1/p1pb1pbp/1p1p1np1/3P4/8/1PN2N2/PBQ1BPPP/R3R1K1 b KQkq - 0 14",
            "r2qr1k1/p2b1pbp/1p1p1np1/2pP4/8/1PN2N2/PBQ1BPPP/R3R1K1 w KQkq c6 0 15",
            "r2qr1k1/p2b1pbp/1pPp1np1/8/8/1PN2N2/PBQ1BPPP/R3R1K1 b KQkq - 0 15",
            "r2qr1k1/p4pbp/1pbp1np1/8/8/1PN2N2/PBQ1BPPP/R3R1K1 w KQkq - 0 16",
            "r2qr1k1/p4pbp/1pbp1np1/1B6/8/1PN2N2/PBQ2PPP/R3R1K1 b KQkq - 0 16",
            "r2qr1k1/p4pbp/1p1p1np1/1b6/8/1PN2N2/PBQ2PPP/R3R1K1 w KQkq - 0 17",
            "r2qr1k1/p4pbp/1p1p1np1/1N6/8/1P3N2/PBQ2PPP/R3R1K1 b KQkq - 0 17",
            "r2q2k1/p4pbp/1p1p1np1/1N6/8/1P3N2/PBQ2PPP/R3r1K1 w KQkq - 0 18",
            "r2q2k1/p4pbp/1p1p1np1/1N6/8/1P3N2/PBQ2PPP/4R1K1 b KQkq - 0 18",
            "r5k1/p2q1pbp/1p1p1np1/1N6/8/1P3N2/PBQ2PPP/4R1K1 w KQkq - 0 19",
            "r5k1/p2q1pbp/1p1p1np1/1N6/P7/1P3N2/1BQ2PPP/4R1K1 b KQkq a3 0 19",
            "2r3k1/p2q1pbp/1p1p1np1/1N6/P7/1P3N2/1BQ2PPP/4R1K1 w KQkq a3 0 20",
            "2r3k1/p2q1pbp/1p1p1np1/1N6/P7/1P3N2/1B1Q1PPP/4R1K1 b KQkq a3 0 20",
            "2r3k1/p2q1pbp/1p1p2p1/1N5n/P7/1P3N2/1B1Q1PPP/4R1K1 w KQkq a3 0 21",
            "2r3k1/p2q1pBp/1p1p2p1/1N5n/P7/1P3N2/3Q1PPP/4R1K1 b KQkq a3 0 21",
            "2r3k1/p2q1pnp/1p1p2p1/1N6/P7/1P3N2/3Q1PPP/4R1K1 w KQkq a3 0 22",
            "2r3k1/p2q1pnp/1p1Q2p1/1N6/P7/1P3N2/5PPP/4R1K1 b KQkq a3 0 22",
            "3r2k1/p2q1pnp/1p1Q2p1/1N6/P7/1P3N2/5PPP/4R1K1 w KQkq a3 0 23",
            "3r2k1/p2q1pnp/1p4p1/1N2Q3/P7/1P3N2/5PPP/4R1K1 b KQkq a3 0 23",
            "3r2k1/p2q1p1p/1p2n1p1/1N2Q3/P7/1P3N2/5PPP/4R1K1 w KQkq a3 0 24",
            "3r2k1/p2q1p1p/1p2n1p1/1N2Q3/P7/1P3N1P/5PP1/4R1K1 b KQkq a3 0 24",
            "3r2k1/p2q1p1p/1p4p1/1Nn1Q3/P7/1P3N1P/5PP1/4R1K1 w KQkq a3 0 25",
            "3r2k1/p2q1p1p/1p4p1/1Nn1Q3/PP6/5N1P/5PP1/4R1K1 b KQkq a3 0 25",
            "3r2k1/p2q1p1p/1p4p1/1N2Q3/nP6/5N1P/5PP1/4R1K1 w KQkq a3 0 26",
            "3r2k1/p2q1p1p/1p4p1/1N2Q3/nP1N4/7P/5PP1/4R1K1 b KQkq a3 0 26",
            "4r1k1/p2q1p1p/1p4p1/1N2Q3/nP1N4/7P/5PP1/4R1K1 w KQkq a3 0 27",
            "4Q1k1/p2q1p1p/1p4p1/1N6/nP1N4/7P/5PP1/4R1K1 b KQkq a3 0 27",
            "4Q3/p2q1pkp/1p4p1/1N6/nP1N4/7P/5PP1/4R1K1 w KQkq a3 0 28",
            "8/p2Q1pkp/1p4p1/1N6/nP1N4/7P/5PP1/4R1K1 b KQkq a3 0 28"
        };
        var actual = PgnToFenConverter.ConvertPgnToFen(pgn);
        foreach (var fen in actual)
        {
            output.WriteLine(fen);
        }
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void  ConvertPgnToFen_PawnMoves_ReturnsCorrectFens()
    {
        string pgn = "1. e4 Nc6 2. d4 e6 3. f4 g6 4. Nf3 Bb4+ 5. c3 Bf8 6. Be3 Bg7 7. Bd3 Nge7 8. e5 d5 9. O-O b6 10. Nbd2 Bb7 11. Rf2 Qc8 12. Rb1 Ba6 13. Nf1 Bxd3 14. Qxd3 Nf5 15. Qb5 Qd7 16. Kh1 a6 17. Qb3 Na5 18. Qc2 Nc4 19. Bc1 Bh6 20. b3 Na5 21. g3 Bg7 22. Ng5 h6 23. Nf3 Ne7 24. Be3 Nf5 25. Qe2 Nxe3 26. Qxe3 Rc8 27. Qe2 Ra8 28. a4 Nb7 29. Nh4 a5 30. Qc2 Qe7 31. Nxg6 fxg6 32. Qxg6+ Qf7 33. Qg4 h5 34. Qh3 Ke7 35. Qh4+ Kd7 36. Qh3 Rag8 37. Ne3 Bf8 38. Ng2 Be7 39. Nh4 Bxh4 40. Qxh4 Rg4 41. Qh3 Qf5 42. Qg2 h4 43. Rbf1 hxg3 44. Rf3 Rxh2+ 45. Qxh2 gxh2 46. Kxh2 Qh5+ 47. Rh3 Rh4 48. Rf3 Rxh3+ 49. Rxh3 Qe2+ 50. Kg3 Qe3+ 51. Kg4 Qg1+ 52. Rg3 Qd1+ 53. Kg5 Qxb3 54. Rg4";
        var expected = new List<string>
        {

        };
        var actual = PgnToFenConverter.ConvertPgnToFen(pgn);
        foreach (var fen in actual)
        {
            output.WriteLine(fen);
        }
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void ConvertPgnToFen_Chess960_ReturnsCorrectFens()
    {
        string pgn = "1. e3 Ng6 2. Bd3 f6 3. Bxg6 hxg6 4. Ng3 Be6 5. Ne2 g5 6. h3 g6 7. b3 Bg7 8. c4 f5 9. Nbc3 f4 10. exf4 gxf4 11. Nxf4 Bf5 12. Ncd5 d6 13. Qa3 Nc6 14. Qa4 Qd7 15. Bh2 g5 16. O-O-O gxf4 17. Bxf4 Rh8 18. Re2 Rg8 19. Rde1 Be5 20. Bxe5 Nxe5 21. Rxe5 dxe5 22. Rxe5 Qxa4 23. bxa4 Be6 24. Nxe7 Kxe7 25. f4";
        var expected = new List<string>
        {
          
        };
        var actual = PgnToFenConverter.ConvertPgnToFen(pgn);
        foreach (var fen in actual)
        {
            output.WriteLine(fen);
        }
        Assert.Equal(expected, actual);
    }
}
