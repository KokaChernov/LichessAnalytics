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
    public void ConvertPgnToFen_SimpleGame_ReturnsCorrectFens()
    {
        string pgn = "1. Nf3 Nf6 2. g3 g6 3. Bg2 c6 4. d4 d6 5. O-O Nbd7 6. Bg5 Bg7 7. Qd2 O-O 8. Nc3 Re8 9. Bh6 Nh5 10. Bxg7 Nxg7 11. h4 Nf6 12. Ng5 h6 13. Nf3 Bd7 14. Qd3 Bg4 15. Ne4 Bf5 16. Nxf6+ exf6 17. Qd2 Qd7 18. c3 Re6 19. Rae1 Rae8 20. Nh2 d5 21. f3 g5 22. hxg5 hxg5 23. Rf2 Bg6 24. Ng4 R6e7 25. Nxf6+ Kf8 26. Nxd7+ Rxd7 27. Qxg5 Rde7 28. e4 Ne6 29. Qh6+ Kg8 30. e5 Ng7 31. Bh3 f5 32. Rh2 Kf7 33. g4 fxg4 34. Bxg4 Rd8 35. Bh5 Bxh5 36. Rxh5 Nxh5 37. Qxh5+ Ke6 38. Qg6+ Kd7 39. Qd6+ Kc8 40. Qxe7 Rd7 41. Qe6 Kc7 42. Qxd7+ Kxd7 43. Rf1 Ke7 44. f4 Ke6 45. f5+ Kf7 46. f6 Ke6 47. f7 Kd7 48. f8=Q Kc7 49. Qf6 Kb6 50. Rf3 a6 51. Qf4 a5 52. Qg4 a4 53. Qg2 a3 54. Qe2 axb2 55. Qxb2+ Kc7 56. Rf2 b6 57. Qxb6+ Kxb6 58. Re2 Kb5 59. e6 Kc4 60. e7 Kxc3 61. e8=Q Kxd4 62. Qe6 Kd3 63. Qxc6 Kxe2 64. Qxd5 Ke3 65. Qb3+ Ke4 66. a4 Ke5 67. a5 Ke4 68. a6 Ke5 69. a7 Ke4 70. a8=Q+ Ke5 71. Qab7";
        var expected = new List<string>
        {
            "rnbqkbnr/pppppppp/8/8/8/5N2/PPPPPPPP/RNBQKB1R b KQkq - 0 1",
            "rnbqkb1r/pppppppp/5n2/8/8/5N2/PPPPPPPP/RNBQKB1R w KQkq - 0 2",
            "rnbqkb1r/pppppppp/5n2/8/8/5NP1/PPPPPP1P/RNBQKB1R b KQkq - 0 2",
            "rnbqkb1r/pppppp1p/5np1/8/8/5NP1/PPPPPP1P/RNBQKB1R w KQkq - 0 3",
            "rnbqkb1r/pppppp1p/5np1/8/8/5NP1/PPPPPPBP/RNBQK2R b KQkq - 0 3",
            "rnbqkb1r/pp1ppp1p/2p2np1/8/8/5NP1/PPPPPPBP/RNBQK2R w KQkq - 0 4",
            "rnbqkb1r/pp1ppp1p/2p2np1/8/3P4/5NP1/PPP1PPBP/RNBQK2R b KQkq d3 0 4",
            "rnbqkb1r/pp2pp1p/2pp1np1/8/3P4/5NP1/PPP1PPBP/RNBQK2R w KQkq - 0 5",
            "rnbqkb1r/pp2pp1p/2pp1np1/8/3P4/5NP1/PPP1PPBP/RNBQ1RK1 b KQkq - 1 5",
            "r1bqkb1r/pp1npp1p/2pp1np1/8/3P4/5NP1/PPP1PPBP/RNBQ1RK1 w KQkq - 0 6",
            "r1bqkb1r/pp1npp1p/2pp1np1/6B1/3P4/5NP1/PPP1PPBP/RN1Q1RK1 b KQkq - 0 6",
            "r1bqk2r/pp1nppbp/2pp1np1/6B1/3P4/5NP1/PPP1PPBP/RN1Q1RK1 w KQkq - 0 7",
            "r1bqk2r/pp1nppbp/2pp1np1/6B1/3P4/5NP1/PPPQPPBP/RN3RK1 b KQkq - 0 7",
            "r1bq1rk1/pp1nppbp/2pp1np1/6B1/3P4/5NP1/PPPQPPBP/RN3RK1 w KQkq - 1 8",
            "r1bq1rk1/pp1nppbp/2pp1np1/6B1/3P4/2N2NP1/PPPQPPBP/R4RK1 b KQkq - 0 8",
            "r1bqr1k1/pp1nppbp/2pp1np1/6B1/3P4/2N2NP1/PPPQPPBP/R4RK1 w KQkq - 0 9",
            "r1bqr1k1/pp1nppbp/2pp1npB/8/3P4/2N2NP1/PPPQPPBP/R4RK1 b KQkq - 0 9",
            "r1bqr1k1/pp1nppbp/2pp2pB/7n/3P4/2N2NP1/PPPQPPBP/R4RK1 w KQkq - 0 10",
            "r1bqr1k1/pp1nppBp/2pp2p1/7n/3P4/2N2NP1/PPPQPPBP/R4RK1 b KQkq - 0 10",
            "r1bqr1k1/pp1nppnp/2pp2p1/8/3P4/2N2NP1/PPPQPPBP/R4RK1 w KQkq - 0 11",
            "r1bqr1k1/pp1nppnp/2pp2p1/8/3P3P/2N2NP1/PPPQPPB1/R4RK1 b KQkq h3 0 11",
            "r1bqr1k1/pp2ppnp/2pp1np1/8/3P3P/2N2NP1/PPPQPPB1/R4RK1 w KQkq - 0 12",
            "r1bqr1k1/pp2ppnp/2pp1np1/6N1/3P3P/2N3P1/PPPQPPB1/R4RK1 b KQkq - 0 12",
            "r1bqr1k1/pp2ppn1/2pp1npp/6N1/3P3P/2N3P1/PPPQPPB1/R4RK1 w KQkq - 0 13",
            "r1bqr1k1/pp2ppn1/2pp1npp/8/3P3P/2N2NP1/PPPQPPB1/R4RK1 b KQkq - 0 13",
            "r2qr1k1/pp1bppn1/2pp1npp/8/3P3P/2N2NP1/PPPQPPB1/R4RK1 w KQkq - 0 14",
            "r2qr1k1/pp1bppn1/2pp1npp/8/3P3P/2NQ1NP1/PPP1PPB1/R4RK1 b KQkq - 0 14",
            "r2qr1k1/pp2ppn1/2pp1npp/8/3P2bP/2NQ1NP1/PPP1PPB1/R4RK1 w KQkq - 0 15",
            "r2qr1k1/pp2ppn1/2pp1npp/8/3PN1bP/3Q1NP1/PPP1PPB1/R4RK1 b KQkq - 0 15",
            "r2qr1k1/pp2ppn1/2pp1npp/5b2/3PN2P/3Q1NP1/PPP1PPB1/R4RK1 w KQkq - 0 16",
            "r2qr1k1/pp2ppn1/2pp1Npp/5b2/3P3P/3Q1NP1/PPP1PPB1/R4RK1 b KQkq - 0 16",
            "r2qr1k1/pp3pn1/2pp1ppp/5b2/3P3P/3Q1NP1/PPP1PPB1/R4RK1 w KQkq - 0 17",
            "r2qr1k1/pp3pn1/2pp1ppp/5b2/3P3P/5NP1/PPPQPPB1/R4RK1 b KQkq - 0 17",
            "r3r1k1/pp1q1pn1/2pp1ppp/5b2/3P3P/5NP1/PPPQPPB1/R4RK1 w KQkq - 0 18",
            "r3r1k1/pp1q1pn1/2pp1ppp/5b2/3P3P/2P2NP1/PP1QPPB1/R4RK1 b KQkq - 0 18",
            "r5k1/pp1q1pn1/2pprppp/5b2/3P3P/2P2NP1/PP1QPPB1/R4RK1 w KQkq - 0 19",
            "r5k1/pp1q1pn1/2pprppp/5b2/3P3P/2P2NP1/PP1QPPB1/4RRK1 b KQkq - 0 19",
            "4r1k1/pp1q1pn1/2pprppp/5b2/3P3P/2P2NP1/PP1QPPB1/4RRK1 w KQkq - 0 20",
            "4r1k1/pp1q1pn1/2pprppp/5b2/3P3P/2P3P1/PP1QPPBN/4RRK1 b KQkq - 0 20",
            "4r1k1/pp1q1pn1/2p1rppp/3p1b2/3P3P/2P3P1/PP1QPPBN/4RRK1 w KQkq - 0 21",
            "4r1k1/pp1q1pn1/2p1rppp/3p1b2/3P3P/2P2PP1/PP1QP1BN/4RRK1 b KQkq - 0 21",
            "4r1k1/pp1q1pn1/2p1rp1p/3p1bp1/3P3P/2P2PP1/PP1QP1BN/4RRK1 w KQkq - 0 22",
            "4r1k1/pp1q1pn1/2p1rp1p/3p1bP1/3P4/2P2PP1/PP1QP1BN/4RRK1 b KQkq - 0 22",
            "4r1k1/pp1q1pn1/2p1rp2/3p1bp1/3P4/2P2PP1/PP1QP1BN/4RRK1 w KQkq - 0 23",
            "4r1k1/pp1q1pn1/2p1rp2/3p1bp1/3P4/2P2PP1/PP1QPRBN/4R1K1 b KQkq - 0 23",
            "4r1k1/pp1q1pn1/2p1rpb1/3p2p1/3P4/2P2PP1/PP1QPRBN/4R1K1 w KQkq - 0 24",
            "4r1k1/pp1q1pn1/2p1rpb1/3p2p1/3P2N1/2P2PP1/PP1QPRB1/4R1K1 b KQkq - 0 24",
            "4r1k1/pp1qrpn1/2p2pb1/3p2p1/3P2N1/2P2PP1/PP1QPRB1/4R1K1 w KQkq - 0 25",
            "4r1k1/pp1qrpn1/2p2Nb1/3p2p1/3P4/2P2PP1/PP1QPRB1/4R1K1 b KQkq - 0 25",
            "4rk2/pp1qrpn1/2p2Nb1/3p2p1/3P4/2P2PP1/PP1QPRB1/4R1K1 w KQkq - 0 26",
            "4rk2/pp1Nrpn1/2p3b1/3p2p1/3P4/2P2PP1/PP1QPRB1/4R1K1 b KQkq - 0 26",
            "4rk2/pp1r1pn1/2p3b1/3p2p1/3P4/2P2PP1/PP1QPRB1/4R1K1 w KQkq - 0 27",
            "4rk2/pp1r1pn1/2p3b1/3p2Q1/3P4/2P2PP1/PP2PRB1/4R1K1 b KQkq - 0 27",
            "4rk2/pp2rpn1/2p3b1/3p2Q1/3P4/2P2PP1/PP2PRB1/4R1K1 w KQkq - 0 28",
            "4rk2/pp2rpn1/2p3b1/3p2Q1/3PP3/2P2PP1/PP3RB1/4R1K1 b KQkq e3 0 28",
            "4rk2/pp2rp2/2p1n1b1/3p2Q1/3PP3/2P2PP1/PP3RB1/4R1K1 w KQkq - 0 29",
            "4rk2/pp2rp2/2p1n1bQ/3p4/3PP3/2P2PP1/PP3RB1/4R1K1 b KQkq - 0 29",
            "4r1k1/pp2rp2/2p1n1bQ/3p4/3PP3/2P2PP1/PP3RB1/4R1K1 w KQkq - 0 30",
            "4r1k1/pp2rp2/2p1n1bQ/3pP3/3P4/2P2PP1/PP3RB1/4R1K1 b KQkq - 0 30",
            "4r1k1/pp2rpn1/2p3bQ/3pP3/3P4/2P2PP1/PP3RB1/4R1K1 w KQkq - 0 31",
            "4r1k1/pp2rpn1/2p3bQ/3pP3/3P4/2P2PPB/PP3R2/4R1K1 b KQkq - 0 31",
            "4r1k1/pp2r1n1/2p3bQ/3pPp2/3P4/2P2PPB/PP3R2/4R1K1 w KQkq f6 0 32",
            "4r1k1/pp2r1n1/2p3bQ/3pPp2/3P4/2P2PPB/PP5R/4R1K1 b KQkq - 0 32",
            "4r3/pp2rkn1/2p3bQ/3pPp2/3P4/2P2PPB/PP5R/4R1K1 w KQkq - 0 33",
            "4r3/pp2rkn1/2p3bQ/3pPp2/3P2P1/2P2P1B/PP5R/4R1K1 b KQkq - 0 33",
            "4r3/pp2rkn1/2p3bQ/3pP3/3P2p1/2P2P1B/PP5R/4R1K1 w KQkq - 0 34",
            "4r3/pp2rkn1/2p3bQ/3pP3/3P2B1/2P2P2/PP5R/4R1K1 b KQkq - 0 34",
            "3r4/pp2rkn1/2p3bQ/3pP3/3P2B1/2P2P2/PP5R/4R1K1 w KQkq - 0 35",
            "3r4/pp2rkn1/2p3bQ/3pP2B/3P4/2P2P2/PP5R/4R1K1 b KQkq - 0 35",
            "3r4/pp2rkn1/2p4Q/3pP2b/3P4/2P2P2/PP5R/4R1K1 w KQkq - 0 36",
            "3r4/pp2rkn1/2p4Q/3pP2R/3P4/2P2P2/PP6/4R1K1 b KQkq - 0 36",
            "3r4/pp2rk2/2p4Q/3pP2n/3P4/2P2P2/PP6/4R1K1 w KQkq - 0 37",
            "3r4/pp2rk2/2p5/3pP2Q/3P4/2P2P2/PP6/4R1K1 b KQkq - 0 37",
            "3r4/pp2r3/2p1k3/3pP2Q/3P4/2P2P2/PP6/4R1K1 w KQkq - 0 38",
            "3r4/pp2r3/2p1k1Q1/3pP3/3P4/2P2P2/PP6/4R1K1 b KQkq - 0 38",
            "3r4/pp1kr3/2p3Q1/3pP3/3P4/2P2P2/PP6/4R1K1 w KQkq - 0 39",
            "3r4/pp1kr3/2pQ4/3pP3/3P4/2P2P2/PP6/4R1K1 b KQkq - 0 39",
            "2kr4/pp2r3/2pQ4/3pP3/3P4/2P2P2/PP6/4R1K1 w KQkq - 0 40",
            "2kr4/pp2Q3/2p5/3pP3/3P4/2P2P2/PP6/4R1K1 b KQkq - 0 40",
            "2k5/pp1rQ3/2p5/3pP3/3P4/2P2P2/PP6/4R1K1 w KQkq - 0 41",
            "2k5/pp1r4/2p1Q3/3pP3/3P4/2P2P2/PP6/4R1K1 b KQkq - 0 41",
            "8/ppkr4/2p1Q3/3pP3/3P4/2P2P2/PP6/4R1K1 w KQkq - 0 42",
            "8/ppkQ4/2p5/3pP3/3P4/2P2P2/PP6/4R1K1 b KQkq - 0 42",
            "8/pp1k4/2p5/3pP3/3P4/2P2P2/PP6/4R1K1 w KQkq - 0 43",
            "8/pp1k4/2p5/3pP3/3P4/2P2P2/PP6/5RK1 b KQkq - 0 43",
            "8/pp2k3/2p5/3pP3/3P4/2P2P2/PP6/5RK1 w KQkq - 0 44",
            "8/pp2k3/2p5/3pP3/3P1P2/2P5/PP6/5RK1 b KQkq - 0 44",
            "8/pp6/2p1k3/3pP3/3P1P2/2P5/PP6/5RK1 w KQkq - 0 45",
            "8/pp6/2p1k3/3pPP2/3P4/2P5/PP6/5RK1 b KQkq - 0 45",
            "8/pp3k2/2p5/3pPP2/3P4/2P5/PP6/5RK1 w KQkq - 0 46",
            "8/pp3k2/2p2P2/3pP3/3P4/2P5/PP6/5RK1 b KQkq - 0 46",
            "8/pp6/2p1kP2/3pP3/3P4/2P5/PP6/5RK1 w KQkq - 0 47",
            "8/pp3P2/2p1k3/3pP3/3P4/2P5/PP6/5RK1 b KQkq - 0 47",
            "8/pp1k1P2/2p5/3pP3/3P4/2P5/PP6/5RK1 w KQkq - 0 48",
            "5Q2/pp1k4/2p5/3pP3/3P4/2P5/PP6/5RK1 b KQkq - 0 48",
            "5Q2/ppk5/2p5/3pP3/3P4/2P5/PP6/5RK1 w KQkq - 0 49",
            "8/ppk5/2p2Q2/3pP3/3P4/2P5/PP6/5RK1 b KQkq - 0 49",
            "8/pp6/1kp2Q2/3pP3/3P4/2P5/PP6/5RK1 w KQkq - 0 50",
            "8/pp6/1kp2Q2/3pP3/3P4/2P2R2/PP6/6K1 b KQkq - 0 50",
            "8/1p6/pkp2Q2/3pP3/3P4/2P2R2/PP6/6K1 w KQkq - 0 51",
            "8/1p6/pkp5/3pP3/3P1Q2/2P2R2/PP6/6K1 b KQkq - 0 51",
            "8/1p6/1kp5/p2pP3/3P1Q2/2P2R2/PP6/6K1 w KQkq - 0 52",
            "8/1p6/1kp5/p2pP3/3P2Q1/2P2R2/PP6/6K1 b KQkq - 0 52",
            "8/1p6/1kp5/3pP3/p2P2Q1/2P2R2/PP6/6K1 w KQkq - 0 53",
            "8/1p6/1kp5/3pP3/p2P4/2P2R2/PP4Q1/6K1 b KQkq - 0 53",
            "8/1p6/1kp5/3pP3/3P4/p1P2R2/PP4Q1/6K1 w KQkq - 0 54",
            "8/1p6/1kp5/3pP3/3P4/p1P2R2/PP2Q3/6K1 b KQkq - 0 54",
            "8/1p6/1kp5/3pP3/3P4/2P2R2/Pp2Q3/6K1 w KQkq - 0 55",
            "8/1p6/1kp5/3pP3/3P4/2P2R2/PQ6/6K1 b KQkq - 0 55",
            "8/1pk5/2p5/3pP3/3P4/2P2R2/PQ6/6K1 w KQkq - 0 56",
            "8/1pk5/2p5/3pP3/3P4/2P5/PQ3R2/6K1 b KQkq - 0 56",
            "8/2k5/1pp5/3pP3/3P4/2P5/PQ3R2/6K1 w KQkq - 0 57",
            "8/2k5/1Qp5/3pP3/3P4/2P5/P4R2/6K1 b KQkq - 0 57",
            "8/8/1kp5/3pP3/3P4/2P5/P4R2/6K1 w KQkq - 0 58",
            "8/8/1kp5/3pP3/3P4/2P5/P3R3/6K1 b KQkq - 0 58",
            "8/8/2p5/1k1pP3/3P4/2P5/P3R3/6K1 w KQkq - 0 59",
            "8/8/2p1P3/1k1p4/3P4/2P5/P3R3/6K1 b KQkq - 0 59",
            "8/8/2p1P3/3p4/2kP4/2P5/P3R3/6K1 w KQkq - 0 60",
            "8/4P3/2p5/3p4/2kP4/2P5/P3R3/6K1 b KQkq - 0 60",
            "8/4P3/2p5/3p4/3P4/2k5/P3R3/6K1 w KQkq - 0 61",
            "4Q3/8/2p5/3p4/3P4/2k5/P3R3/6K1 b KQkq - 0 61",
            "4Q3/8/2p5/3p4/3k4/8/P3R3/6K1 w KQkq - 0 62",
            "8/8/2p1Q3/3p4/3k4/8/P3R3/6K1 b KQkq - 0 62",
            "8/8/2p1Q3/3p4/8/3k4/P3R3/6K1 w KQkq - 0 63",
            "8/8/2Q5/3p4/8/3k4/P3R3/6K1 b KQkq - 0 63",
            "8/8/2Q5/3p4/8/8/P3k3/6K1 w KQkq - 0 64",
            "8/8/8/3Q4/8/8/P3k3/6K1 b KQkq - 0 64",
            "8/8/8/3Q4/8/4k3/P7/6K1 w KQkq - 0 65",
            "8/8/8/8/8/1Q2k3/P7/6K1 b KQkq - 0 65",
            "8/8/8/8/4k3/1Q6/P7/6K1 w KQkq - 0 66",
            "8/8/8/8/P3k3/1Q6/8/6K1 b KQkq a3 0 66",
            "8/8/8/4k3/P7/1Q6/8/6K1 w KQkq - 0 67",
            "8/8/8/P3k3/8/1Q6/8/6K1 b KQkq - 0 67",
            "8/8/8/P7/4k3/1Q6/8/6K1 w KQkq - 0 68",
            "8/8/P7/8/4k3/1Q6/8/6K1 b KQkq - 0 68",
            "8/8/P7/4k3/8/1Q6/8/6K1 w KQkq - 0 69",
            "8/P7/8/4k3/8/1Q6/8/6K1 b KQkq - 0 69",
            "8/P7/8/8/4k3/1Q6/8/6K1 w KQkq - 0 70",
            "Q7/8/8/8/4k3/1Q6/8/6K1 b KQkq - 0 70",
            "Q7/8/8/4k3/8/1Q6/8/6K1 w KQkq - 0 71",
            "8/1Q6/8/4k3/8/1Q6/8/6K1 b KQkq - 0 71",
        };
        var actual = PgnToFenConverter.ConvertPgnToFen(pgn);
        foreach (var fen in actual)
        {
            output.WriteLine("\""+fen+"\",");
        }
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ConvertPgnToFen_EnPassant_PawnMoves_ReturnsCorrectFens()
    {
        string pgn = "1. e4 Nc6 2. d4 e6 3. d5 exd5 4. exd5 Nce7 5. Nf3 d6 6. Be2 g6 7. c4 Bg7 8. Nc3 Nf6 9. Qc2 O-O 10. b3 Re8 11. Bb2 Nexd5 12. cxd5 b6 13. O-O Bd7 14. Rfe1 c5 15. dxc6 Bxc6 16. Bb5 Bxb5 17. Nxb5 Rxe1+ 18. Rxe1 Qd7 19. a4 Rc8 20. Qd2 Nh5 21. Bxg7 Nxg7 22. Qxd6 Rd8 23. Qe5 Ne6 24. h3 Nc5 25. b4 Nxa4 26. Nfd4 Re8 27. Qxe8+ Kg7 28. Qxd7";
        var expected = new List<string>
        {
            "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1",
            "r1bqkbnr/pppppppp/2n5/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 2",
            "r1bqkbnr/pppppppp/2n5/8/3PP3/8/PPP2PPP/RNBQKBNR b KQkq d3 0 2",
            "r1bqkbnr/pppp1ppp/2n1p3/8/3PP3/8/PPP2PPP/RNBQKBNR w KQkq - 0 3",
            "r1bqkbnr/pppp1ppp/2n1p3/3P4/4P3/8/PPP2PPP/RNBQKBNR b KQkq - 0 3",
            "r1bqkbnr/pppp1ppp/2n5/3p4/4P3/8/PPP2PPP/RNBQKBNR w KQkq - 0 4",
            "r1bqkbnr/pppp1ppp/2n5/3P4/8/8/PPP2PPP/RNBQKBNR b KQkq - 0 4",
            "r1bqkbnr/ppppnppp/8/3P4/8/8/PPP2PPP/RNBQKBNR w KQkq - 0 5",
            "r1bqkbnr/ppppnppp/8/3P4/8/5N2/PPP2PPP/RNBQKB1R b KQkq - 0 5",
            "r1bqkbnr/ppp1nppp/3p4/3P4/8/5N2/PPP2PPP/RNBQKB1R w KQkq - 0 6",
            "r1bqkbnr/ppp1nppp/3p4/3P4/8/5N2/PPP1BPPP/RNBQK2R b KQkq - 0 6",
            "r1bqkbnr/ppp1np1p/3p2p1/3P4/8/5N2/PPP1BPPP/RNBQK2R w KQkq - 0 7",
            "r1bqkbnr/ppp1np1p/3p2p1/3P4/2P5/5N2/PP2BPPP/RNBQK2R b KQkq c3 0 7",
            "r1bqk1nr/ppp1npbp/3p2p1/3P4/2P5/5N2/PP2BPPP/RNBQK2R w KQkq - 0 8",
            "r1bqk1nr/ppp1npbp/3p2p1/3P4/2P5/2N2N2/PP2BPPP/R1BQK2R b KQkq - 0 8",
            "r1bqk2r/ppp1npbp/3p1np1/3P4/2P5/2N2N2/PP2BPPP/R1BQK2R w KQkq - 0 9",
            "r1bqk2r/ppp1npbp/3p1np1/3P4/2P5/2N2N2/PPQ1BPPP/R1B1K2R b KQkq - 0 9",
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
            "2r3k1/p2q1pbp/1p1p1np1/1N6/P7/1P3N2/1BQ2PPP/4R1K1 w KQkq - 0 20",
            "2r3k1/p2q1pbp/1p1p1np1/1N6/P7/1P3N2/1B1Q1PPP/4R1K1 b KQkq - 0 20",
            "2r3k1/p2q1pbp/1p1p2p1/1N5n/P7/1P3N2/1B1Q1PPP/4R1K1 w KQkq - 0 21",
            "2r3k1/p2q1pBp/1p1p2p1/1N5n/P7/1P3N2/3Q1PPP/4R1K1 b KQkq - 0 21",
            "2r3k1/p2q1pnp/1p1p2p1/1N6/P7/1P3N2/3Q1PPP/4R1K1 w KQkq - 0 22",
            "2r3k1/p2q1pnp/1p1Q2p1/1N6/P7/1P3N2/5PPP/4R1K1 b KQkq - 0 22",
            "3r2k1/p2q1pnp/1p1Q2p1/1N6/P7/1P3N2/5PPP/4R1K1 w KQkq - 0 23",
            "3r2k1/p2q1pnp/1p4p1/1N2Q3/P7/1P3N2/5PPP/4R1K1 b KQkq - 0 23",
            "3r2k1/p2q1p1p/1p2n1p1/1N2Q3/P7/1P3N2/5PPP/4R1K1 w KQkq - 0 24",
            "3r2k1/p2q1p1p/1p2n1p1/1N2Q3/P7/1P3N1P/5PP1/4R1K1 b KQkq - 0 24",
            "3r2k1/p2q1p1p/1p4p1/1Nn1Q3/P7/1P3N1P/5PP1/4R1K1 w KQkq - 0 25",
            "3r2k1/p2q1p1p/1p4p1/1Nn1Q3/PP6/5N1P/5PP1/4R1K1 b KQkq - 0 25",
            "3r2k1/p2q1p1p/1p4p1/1N2Q3/nP6/5N1P/5PP1/4R1K1 w KQkq - 0 26",
            "3r2k1/p2q1p1p/1p4p1/1N2Q3/nP1N4/7P/5PP1/4R1K1 b KQkq - 0 26",
            "4r1k1/p2q1p1p/1p4p1/1N2Q3/nP1N4/7P/5PP1/4R1K1 w KQkq - 0 27",
            "4Q1k1/p2q1p1p/1p4p1/1N6/nP1N4/7P/5PP1/4R1K1 b KQkq - 0 27",
            "4Q3/p2q1pkp/1p4p1/1N6/nP1N4/7P/5PP1/4R1K1 w KQkq - 0 28",
            "8/p2Q1pkp/1p4p1/1N6/nP1N4/7P/5PP1/4R1K1 b KQkq - 0 28"
        };
        
        var actual = PgnToFenConverter.ConvertPgnToFen(pgn);
        foreach (var fen in actual)
        {
            output.WriteLine("\""+fen+"\",");
        }
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ConvertPgnToFen_PinnedRook_ReturnsCorrectFens()
    {
        string pgn = "1. e4 Nc6 2. d4 e6 3. f4 g6 4. Nf3 Bb4+ 5. c3 Bf8 6. Be3 Bg7 7. Bd3 Nge7 8. e5 d5 9. O-O b6 10. Nbd2 Bb7 11. Rf2 Qc8 12. Rb1 Ba6 13. Nf1 Bxd3 14. Qxd3 Nf5 15. Qb5 Qd7 16. Kh1 a6 17. Qb3 Na5 18. Qc2 Nc4 19. Bc1 Bh6 20. b3 Na5 21. g3 Bg7 22. Ng5 h6 23. Nf3 Ne7 24. Be3 Nf5 25. Qe2 Nxe3 26. Qxe3 Rc8 27. Qe2 Ra8 28. a4 Nb7 29. Nh4 a5 30. Qc2 Qe7 31. Nxg6 fxg6 32. Qxg6+ Qf7 33. Qg4 h5 34. Qh3 Ke7 35. Qh4+ Kd7 36. Qh3 Rag8 37. Ne3 Bf8 38. Ng2 Be7 39. Nh4 Bxh4 40. Qxh4 Rg4 41. Qh3 Qf5 42. Qg2 h4 43. Rbf1 hxg3 44. Rf3 Rxh2+ 45. Qxh2 gxh2 46. Kxh2 Qh5+ 47. Rh3 Rh4 48. Rf3 Rxh3+ 49. Rxh3 Qe2+ 50. Kg3 Qe3+ 51. Kg4 Qg1+ 52. Rg3 Qd1+ 53. Kg5 Qxb3 54. Rg4";
        var expected = new List<string>
        {
            "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1",
            "r1bqkbnr/pppppppp/2n5/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 2",
            "r1bqkbnr/pppppppp/2n5/8/3PP3/8/PPP2PPP/RNBQKBNR b KQkq d3 0 2",
            "r1bqkbnr/pppp1ppp/2n1p3/8/3PP3/8/PPP2PPP/RNBQKBNR w KQkq - 0 3",
            "r1bqkbnr/pppp1ppp/2n1p3/8/3PPP2/8/PPP3PP/RNBQKBNR b KQkq f3 0 3",
            "r1bqkbnr/pppp1p1p/2n1p1p1/8/3PPP2/8/PPP3PP/RNBQKBNR w KQkq - 0 4",
            "r1bqkbnr/pppp1p1p/2n1p1p1/8/3PPP2/5N2/PPP3PP/RNBQKB1R b KQkq - 0 4",
            "r1bqk1nr/pppp1p1p/2n1p1p1/8/1b1PPP2/5N2/PPP3PP/RNBQKB1R w KQkq - 0 5",
            "r1bqk1nr/pppp1p1p/2n1p1p1/8/1b1PPP2/2P2N2/PP4PP/RNBQKB1R b KQkq - 0 5",
            "r1bqkbnr/pppp1p1p/2n1p1p1/8/3PPP2/2P2N2/PP4PP/RNBQKB1R w KQkq - 0 6",
            "r1bqkbnr/pppp1p1p/2n1p1p1/8/3PPP2/2P1BN2/PP4PP/RN1QKB1R b KQkq - 0 6",
            "r1bqk1nr/pppp1pbp/2n1p1p1/8/3PPP2/2P1BN2/PP4PP/RN1QKB1R w KQkq - 0 7",
            "r1bqk1nr/pppp1pbp/2n1p1p1/8/3PPP2/2PBBN2/PP4PP/RN1QK2R b KQkq - 0 7",
            "r1bqk2r/ppppnpbp/2n1p1p1/8/3PPP2/2PBBN2/PP4PP/RN1QK2R w KQkq - 0 8",
            "r1bqk2r/ppppnpbp/2n1p1p1/4P3/3P1P2/2PBBN2/PP4PP/RN1QK2R b KQkq - 0 8",
            "r1bqk2r/ppp1npbp/2n1p1p1/3pP3/3P1P2/2PBBN2/PP4PP/RN1QK2R w KQkq d6 0 9",
            "r1bqk2r/ppp1npbp/2n1p1p1/3pP3/3P1P2/2PBBN2/PP4PP/RN1Q1RK1 b KQkq - 1 9",
            "r1bqk2r/p1p1npbp/1pn1p1p1/3pP3/3P1P2/2PBBN2/PP4PP/RN1Q1RK1 w KQkq - 0 10",
            "r1bqk2r/p1p1npbp/1pn1p1p1/3pP3/3P1P2/2PBBN2/PP1N2PP/R2Q1RK1 b KQkq - 0 10",
            "r2qk2r/pbp1npbp/1pn1p1p1/3pP3/3P1P2/2PBBN2/PP1N2PP/R2Q1RK1 w KQkq - 0 11",
            "r2qk2r/pbp1npbp/1pn1p1p1/3pP3/3P1P2/2PBBN2/PP1N1RPP/R2Q2K1 b KQkq - 0 11",
            "r1q1k2r/pbp1npbp/1pn1p1p1/3pP3/3P1P2/2PBBN2/PP1N1RPP/R2Q2K1 w KQkq - 0 12",
            "r1q1k2r/pbp1npbp/1pn1p1p1/3pP3/3P1P2/2PBBN2/PP1N1RPP/1R1Q2K1 b KQkq - 0 12",
            "r1q1k2r/p1p1npbp/bpn1p1p1/3pP3/3P1P2/2PBBN2/PP1N1RPP/1R1Q2K1 w KQkq - 0 13",
            "r1q1k2r/p1p1npbp/bpn1p1p1/3pP3/3P1P2/2PBBN2/PP3RPP/1R1Q1NK1 b KQkq - 0 13",
            "r1q1k2r/p1p1npbp/1pn1p1p1/3pP3/3P1P2/2PbBN2/PP3RPP/1R1Q1NK1 w KQkq - 0 14",
            "r1q1k2r/p1p1npbp/1pn1p1p1/3pP3/3P1P2/2PQBN2/PP3RPP/1R3NK1 b KQkq - 0 14",
            "r1q1k2r/p1p2pbp/1pn1p1p1/3pPn2/3P1P2/2PQBN2/PP3RPP/1R3NK1 w KQkq - 0 15",
            "r1q1k2r/p1p2pbp/1pn1p1p1/1Q1pPn2/3P1P2/2P1BN2/PP3RPP/1R3NK1 b KQkq - 0 15",
            "r3k2r/p1pq1pbp/1pn1p1p1/1Q1pPn2/3P1P2/2P1BN2/PP3RPP/1R3NK1 w KQkq - 0 16",
            "r3k2r/p1pq1pbp/1pn1p1p1/1Q1pPn2/3P1P2/2P1BN2/PP3RPP/1R3N1K b KQkq - 0 16",
            "r3k2r/2pq1pbp/ppn1p1p1/1Q1pPn2/3P1P2/2P1BN2/PP3RPP/1R3N1K w KQkq - 0 17",
            "r3k2r/2pq1pbp/ppn1p1p1/3pPn2/3P1P2/1QP1BN2/PP3RPP/1R3N1K b KQkq - 0 17",
            "r3k2r/2pq1pbp/pp2p1p1/n2pPn2/3P1P2/1QP1BN2/PP3RPP/1R3N1K w KQkq - 0 18",
            "r3k2r/2pq1pbp/pp2p1p1/n2pPn2/3P1P2/2P1BN2/PPQ2RPP/1R3N1K b KQkq - 0 18",
            "r3k2r/2pq1pbp/pp2p1p1/3pPn2/2nP1P2/2P1BN2/PPQ2RPP/1R3N1K w KQkq - 0 19",
            "r3k2r/2pq1pbp/pp2p1p1/3pPn2/2nP1P2/2P2N2/PPQ2RPP/1RB2N1K b KQkq - 0 19",
            "r3k2r/2pq1p1p/pp2p1pb/3pPn2/2nP1P2/2P2N2/PPQ2RPP/1RB2N1K w KQkq - 0 20",
            "r3k2r/2pq1p1p/pp2p1pb/3pPn2/2nP1P2/1PP2N2/P1Q2RPP/1RB2N1K b KQkq - 0 20",
            "r3k2r/2pq1p1p/pp2p1pb/n2pPn2/3P1P2/1PP2N2/P1Q2RPP/1RB2N1K w KQkq - 0 21",
            "r3k2r/2pq1p1p/pp2p1pb/n2pPn2/3P1P2/1PP2NP1/P1Q2R1P/1RB2N1K b KQkq - 0 21",
            "r3k2r/2pq1pbp/pp2p1p1/n2pPn2/3P1P2/1PP2NP1/P1Q2R1P/1RB2N1K w KQkq - 0 22",
            "r3k2r/2pq1pbp/pp2p1p1/n2pPnN1/3P1P2/1PP3P1/P1Q2R1P/1RB2N1K b KQkq - 0 22",
            "r3k2r/2pq1pb1/pp2p1pp/n2pPnN1/3P1P2/1PP3P1/P1Q2R1P/1RB2N1K w KQkq - 0 23",
            "r3k2r/2pq1pb1/pp2p1pp/n2pPn2/3P1P2/1PP2NP1/P1Q2R1P/1RB2N1K b KQkq - 0 23",
            "r3k2r/2pqnpb1/pp2p1pp/n2pP3/3P1P2/1PP2NP1/P1Q2R1P/1RB2N1K w KQkq - 0 24",
            "r3k2r/2pqnpb1/pp2p1pp/n2pP3/3P1P2/1PP1BNP1/P1Q2R1P/1R3N1K b KQkq - 0 24",
            "r3k2r/2pq1pb1/pp2p1pp/n2pPn2/3P1P2/1PP1BNP1/P1Q2R1P/1R3N1K w KQkq - 0 25",
            "r3k2r/2pq1pb1/pp2p1pp/n2pPn2/3P1P2/1PP1BNP1/P3QR1P/1R3N1K b KQkq - 0 25",
            "r3k2r/2pq1pb1/pp2p1pp/n2pP3/3P1P2/1PP1nNP1/P3QR1P/1R3N1K w KQkq - 0 26",
            "r3k2r/2pq1pb1/pp2p1pp/n2pP3/3P1P2/1PP1QNP1/P4R1P/1R3N1K b KQkq - 0 26",
            "2r1k2r/2pq1pb1/pp2p1pp/n2pP3/3P1P2/1PP1QNP1/P4R1P/1R3N1K w KQkq - 0 27",
            "2r1k2r/2pq1pb1/pp2p1pp/n2pP3/3P1P2/1PP2NP1/P3QR1P/1R3N1K b KQkq - 0 27",
            "r3k2r/2pq1pb1/pp2p1pp/n2pP3/3P1P2/1PP2NP1/P3QR1P/1R3N1K w KQkq - 0 28",
            "r3k2r/2pq1pb1/pp2p1pp/n2pP3/P2P1P2/1PP2NP1/4QR1P/1R3N1K b KQkq a3 0 28",
            "r3k2r/1npq1pb1/pp2p1pp/3pP3/P2P1P2/1PP2NP1/4QR1P/1R3N1K w KQkq - 0 29",
            "r3k2r/1npq1pb1/pp2p1pp/3pP3/P2P1P1N/1PP3P1/4QR1P/1R3N1K b KQkq - 0 29",
            "r3k2r/1npq1pb1/1p2p1pp/p2pP3/P2P1P1N/1PP3P1/4QR1P/1R3N1K w KQkq - 0 30",
            "r3k2r/1npq1pb1/1p2p1pp/p2pP3/P2P1P1N/1PP3P1/2Q2R1P/1R3N1K b KQkq - 0 30",
            "r3k2r/1np1qpb1/1p2p1pp/p2pP3/P2P1P1N/1PP3P1/2Q2R1P/1R3N1K w KQkq - 0 31",
            "r3k2r/1np1qpb1/1p2p1Np/p2pP3/P2P1P2/1PP3P1/2Q2R1P/1R3N1K b KQkq - 0 31",
            "r3k2r/1np1q1b1/1p2p1pp/p2pP3/P2P1P2/1PP3P1/2Q2R1P/1R3N1K w KQkq - 0 32",
            "r3k2r/1np1q1b1/1p2p1Qp/p2pP3/P2P1P2/1PP3P1/5R1P/1R3N1K b KQkq - 0 32",
            "r3k2r/1np2qb1/1p2p1Qp/p2pP3/P2P1P2/1PP3P1/5R1P/1R3N1K w KQkq - 0 33",
            "r3k2r/1np2qb1/1p2p2p/p2pP3/P2P1PQ1/1PP3P1/5R1P/1R3N1K b KQkq - 0 33",
            "r3k2r/1np2qb1/1p2p3/p2pP2p/P2P1PQ1/1PP3P1/5R1P/1R3N1K w KQkq - 0 34",
            "r3k2r/1np2qb1/1p2p3/p2pP2p/P2P1P2/1PP3PQ/5R1P/1R3N1K b KQkq - 0 34",
            "r6r/1np1kqb1/1p2p3/p2pP2p/P2P1P2/1PP3PQ/5R1P/1R3N1K w KQkq - 0 35",
            "r6r/1np1kqb1/1p2p3/p2pP2p/P2P1P1Q/1PP3P1/5R1P/1R3N1K b KQkq - 0 35",
            "r6r/1npk1qb1/1p2p3/p2pP2p/P2P1P1Q/1PP3P1/5R1P/1R3N1K w KQkq - 0 36",
            "r6r/1npk1qb1/1p2p3/p2pP2p/P2P1P2/1PP3PQ/5R1P/1R3N1K b KQkq - 0 36",
            "6rr/1npk1qb1/1p2p3/p2pP2p/P2P1P2/1PP3PQ/5R1P/1R3N1K w KQkq - 0 37",
            "6rr/1npk1qb1/1p2p3/p2pP2p/P2P1P2/1PP1N1PQ/5R1P/1R5K b KQkq - 0 37",
            "5brr/1npk1q2/1p2p3/p2pP2p/P2P1P2/1PP1N1PQ/5R1P/1R5K w KQkq - 0 38",
            "5brr/1npk1q2/1p2p3/p2pP2p/P2P1P2/1PP3PQ/5RNP/1R5K b KQkq - 0 38",
            "6rr/1npkbq2/1p2p3/p2pP2p/P2P1P2/1PP3PQ/5RNP/1R5K w KQkq - 0 39",
            "6rr/1npkbq2/1p2p3/p2pP2p/P2P1P1N/1PP3PQ/5R1P/1R5K b KQkq - 0 39",
            "6rr/1npk1q2/1p2p3/p2pP2p/P2P1P1b/1PP3PQ/5R1P/1R5K w KQkq - 0 40",
            "6rr/1npk1q2/1p2p3/p2pP2p/P2P1P1Q/1PP3P1/5R1P/1R5K b KQkq - 0 40",
            "7r/1npk1q2/1p2p3/p2pP2p/P2P1PrQ/1PP3P1/5R1P/1R5K w KQkq - 0 41",
            "7r/1npk1q2/1p2p3/p2pP2p/P2P1Pr1/1PP3PQ/5R1P/1R5K b KQkq - 0 41",
            "7r/1npk4/1p2p3/p2pPq1p/P2P1Pr1/1PP3PQ/5R1P/1R5K w KQkq - 0 42",
            "7r/1npk4/1p2p3/p2pPq1p/P2P1Pr1/1PP3P1/5RQP/1R5K b KQkq - 0 42",
            "7r/1npk4/1p2p3/p2pPq2/P2P1Prp/1PP3P1/5RQP/1R5K w KQkq - 0 43",
            "7r/1npk4/1p2p3/p2pPq2/P2P1Prp/1PP3P1/5RQP/5R1K b KQkq - 0 43",
            "7r/1npk4/1p2p3/p2pPq2/P2P1Pr1/1PP3p1/5RQP/5R1K w KQkq - 0 44",
            "7r/1npk4/1p2p3/p2pPq2/P2P1Pr1/1PP2Rp1/6QP/5R1K b KQkq - 0 44",
            "8/1npk4/1p2p3/p2pPq2/P2P1Pr1/1PP2Rp1/6Qr/5R1K w KQkq - 0 45",
            "8/1npk4/1p2p3/p2pPq2/P2P1Pr1/1PP2Rp1/7Q/5R1K b KQkq - 0 45",
            "8/1npk4/1p2p3/p2pPq2/P2P1Pr1/1PP2R2/7p/5R1K w KQkq - 0 46",
            "8/1npk4/1p2p3/p2pPq2/P2P1Pr1/1PP2R2/7K/5R2 b KQkq - 0 46",
            "8/1npk4/1p2p3/p2pP2q/P2P1Pr1/1PP2R2/7K/5R2 w KQkq - 0 47",
            "8/1npk4/1p2p3/p2pP2q/P2P1Pr1/1PP4R/7K/5R2 b KQkq - 0 47",
            "8/1npk4/1p2p3/p2pP2q/P2P1P1r/1PP4R/7K/5R2 w KQkq - 0 48",
            "8/1npk4/1p2p3/p2pP2q/P2P1P1r/1PP2R1R/7K/8 b KQkq - 0 48",
            "8/1npk4/1p2p3/p2pP2q/P2P1P2/1PP2R1r/7K/8 w KQkq - 0 49",
            "8/1npk4/1p2p3/p2pP2q/P2P1P2/1PP4R/7K/8 b KQkq - 0 49",
            "8/1npk4/1p2p3/p2pP3/P2P1P2/1PP4R/4q2K/8 w KQkq - 0 50",
            "8/1npk4/1p2p3/p2pP3/P2P1P2/1PP3KR/4q3/8 b KQkq - 0 50",
            "8/1npk4/1p2p3/p2pP3/P2P1P2/1PP1q1KR/8/8 w KQkq - 0 51",
            "8/1npk4/1p2p3/p2pP3/P2P1PK1/1PP1q2R/8/8 b KQkq - 0 51",
            "8/1npk4/1p2p3/p2pP3/P2P1PK1/1PP4R/8/6q1 w KQkq - 0 52",
            "8/1npk4/1p2p3/p2pP3/P2P1PK1/1PP3R1/8/6q1 b KQkq - 0 52",
            "8/1npk4/1p2p3/p2pP3/P2P1PK1/1PP3R1/8/3q4 w KQkq - 0 53",
            "8/1npk4/1p2p3/p2pP1K1/P2P1P2/1PP3R1/8/3q4 b KQkq - 0 53",
            "8/1npk4/1p2p3/p2pP1K1/P2P1P2/1qP3R1/8/8 w KQkq - 0 54",
            "8/1npk4/1p2p3/p2pP1K1/P2P1PR1/1qP5/8/8 b KQkq - 0 54"
        };
        var actual = PgnToFenConverter.ConvertPgnToFen(pgn);
        foreach (var fen in actual)
        {
            output.WriteLine("\""+fen+"\",");
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
