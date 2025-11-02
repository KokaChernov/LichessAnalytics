using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class PgnToFenConverter
{
    public static List<string> ConvertPgnToFen(string pgn)
    {
        string patternFinalResult = @"\s*(1-0|0-1|1/2-1/2|\*)";
        pgn = Regex.Replace(pgn, patternFinalResult, string.Empty, RegexOptions.Multiline);

        var moves = Regex.Replace(pgn, @"\d+\.", "") // remove move numbers
                         .Trim()
                         .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        var board = CreateInitialBoard();
        bool whiteToMove = true;
        string castlingRights = "KQkq";
        string enPassant = "-";
        int halfmoveClock = 0;
        int fullmoveNumber = 1;

        var fens = new List<string>();

        foreach (var move in moves)
        {
            ApplyMove(board, move, ref whiteToMove, ref castlingRights, ref enPassant, ref halfmoveClock, ref fullmoveNumber);
            fens.Add(BoardToFEN(board, whiteToMove, castlingRights, enPassant, halfmoveClock, fullmoveNumber));
        }

        return fens;
    }

    private static char[,] CreateInitialBoard()
    {
        string[] ranks = {
            "rnbqkbnr",
            "pppppppp",
            "........",
            "........",
            "........",
            "........",
            "PPPPPPPP",
            "RNBQKBNR"
        };

        var board = new char[8, 8];
        for (int r = 0; r < 8; r++)
            for (int c = 0; c < 8; c++)
                board[r, c] = ranks[r][c];
        return board;
    }

    private static void ApplyMove(char[,] board, string move, ref bool whiteToMove, ref string castlingRights, ref string enPassant, ref int halfmoveClock, ref int fullmoveNumber)
    {
        string previousEnPassant = enPassant;
        enPassant = "-"; // Reset en passant at the start of each move

        // Handle castling
        if (move == "O-O" || move == "O-O+")
        {
            if (whiteToMove)
            {
                board[7, 4] = '.';
                board[7, 6] = 'K';
                board[7, 7] = '.';
                board[7, 5] = 'R';
            }
            else
            {
                board[0, 4] = '.';
                board[0, 6] = 'k';
                board[0, 7] = '.';
                board[0, 5] = 'r';
            }
            enPassant = "-";
            halfmoveClock++;
            if (!whiteToMove) fullmoveNumber++;
            whiteToMove = !whiteToMove;
            return;
        }
        else if (move == "O-O-O" || move == "O-O-O+")
        {
            if (whiteToMove)
            {
                board[7, 4] = '.';
                board[7, 2] = 'K';
                board[7, 0] = '.';
                board[7, 3] = 'R';
            }
            else
            {
                board[0, 4] = '.';
                board[0, 2] = 'k';
                board[0, 0] = '.';
                board[0, 3] = 'r';
            }
            enPassant = "-";
            halfmoveClock++;
            if (!whiteToMove) fullmoveNumber++;
            whiteToMove = !whiteToMove;
            return;
        }

        // Detect piece type (default pawn)
        char piece = whiteToMove ? 'P' : 'p';
        int targetFile = -1, targetRank = -1;
        int? fromFile = null, fromRank = null;

        // Remove any '+' or '#' or capture 'x'
        string cleanMove = move.Replace("+", "").Replace("#", "").Replace("x", "");

        // Promotion
        char? promotionPiece = null;
        int eqIdx = cleanMove.IndexOf('=');
        if (eqIdx != -1 && eqIdx + 1 < cleanMove.Length)
        {
            promotionPiece = whiteToMove ? cleanMove[eqIdx + 1] : char.ToLower(cleanMove[eqIdx + 1]);
            cleanMove = cleanMove.Substring(0, eqIdx); // Remove =Q part for parsing target square
        }

        // Piece type
        int idx = 0;
        if (char.IsUpper(cleanMove[0]) && "KQRBN".Contains(cleanMove[0]))
        {
            piece = whiteToMove ? cleanMove[0] : char.ToLower(cleanMove[0]);
            idx = 1;
        }

        // Disambiguation: look for from-file/from-rank
        // e.g. Rda1, Kgf8, N1c3, etc.
        if (cleanMove.Length - idx == 4) // e.g. Rda1, Kgf8
        {
            if (cleanMove[idx] >= 'a' && cleanMove[idx] <= 'h') fromFile = cleanMove[idx] - 'a';
            if (cleanMove[idx] >= '1' && cleanMove[idx] <= '8') fromRank = 8 - (cleanMove[idx] - '0');
            idx++;
        }
        else if (cleanMove.Length - idx == 3) // e.g. N1c3, Rdf8
        {
            if (cleanMove[idx] >= 'a' && cleanMove[idx] <= 'h') fromFile = cleanMove[idx] - 'a';
            else if (cleanMove[idx] >= '1' && cleanMove[idx] <= '8') fromRank = 8 - (cleanMove[idx] - '0');
            idx++;
        }

        // Target square
        if (cleanMove.Length - idx >= 2)
        {
            targetFile = cleanMove[idx] - 'a';
            targetRank = 8 - (cleanMove[idx + 1] - '0');
        }

        // Find source square of piece, using disambiguation if present
        int srcFile = -1, srcRank = -1;
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                if (board[r, c] == piece &&
                    (fromFile == null || c == fromFile) &&
                    (fromRank == null || r == fromRank) &&
                    CanMoveTo(board, r, c, targetRank, targetFile, whiteToMove, previousEnPassant))
                {
                    srcRank = r;
                    srcFile = c;
                    break;
                }
            }
            if (srcRank != -1) break;
        }


        // Reset en passant unless pawn double move
        if (char.ToUpper(piece) == 'P' && Math.Abs(srcRank - targetRank) == 2)
        {
            int epRank = (srcRank + targetRank) / 2;
            enPassant = $"{(char)('a' + targetFile)}{8 - epRank}";
        }

        if (char.ToUpper(piece) == 'P' && targetFile != srcFile && board[targetRank, targetFile] == '.' && previousEnPassant != "-")
        {
            //isEnPassantCapture = true;
            // Remove the captured pawn
            int capturedPawnRank = whiteToMove ? targetRank + 1 : targetRank - 1;
            board[capturedPawnRank, targetFile] = '.';
        }

        // Move piece (including promotion)
        if (promotionPiece.HasValue && char.ToUpper(piece) == 'P' && (targetRank == 0 || targetRank == 7))
        {
            board[targetRank, targetFile] = promotionPiece.Value;
        }
        else
        {
            board[targetRank, targetFile] = piece;
        }
        board[srcRank, srcFile] = '.';


        // Update counters
        if (char.ToUpper(piece) == 'P' || board[targetRank, targetFile] != '.')
            halfmoveClock = 0;
        else
            halfmoveClock++;

        if (!whiteToMove) fullmoveNumber++;
        whiteToMove = !whiteToMove;
    }

    private static bool IsPathClear(char[,] board, int sr, int sf, int tr, int tf)
    {
        int dr = Math.Sign(tr - sr);
        int dc = Math.Sign(tf - sf);
        int r = sr + dr;
        int c = sf + dc;
        while (r != tr || c != tf)
        {
            if (board[r, c] != '.') return false;
            r += dr;
            c += dc;
        }
        return true;
    }

    private static bool IsKingInCheck(char[,] board, bool whiteKing)
    {
        // Find king position
        int kingRank = -1, kingFile = -1;
        char kingChar = whiteKing ? 'K' : 'k';
        
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                if (board[r, c] == kingChar)
                {
                    kingRank = r;
                    kingFile = c;
                    break;
                }
            }
            if (kingRank != -1) break;
        }

        // Check if any enemy piece can capture the king
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                char piece = board[r, c];
                if (piece != '.' && char.IsUpper(piece) != whiteKing) // enemy piece
                {
                    if (CanMoveTo(board, r, c, kingRank, kingFile, !whiteKing, "-", true))
                        return true;
                }
            }
        }
        return false;
    }

    // NEW signature: includes enPassant algebraic (e.g. "e6") so pawn can capture en-passant
    // Added allowKingCapture parameter to prevent infinite recursion when checking for king safety
    private static bool CanMoveTo(char[,] board, int sr, int sf, int tr, int tf, bool whiteToMove, string enPassant, bool allowKingCapture = false)
    {
        if (sr == tr && sf == tf) return false; // same square
        char moving = board[sr, sf];
        if (moving == '.') return false;

        bool movingIsWhite = char.IsUpper(moving);
        if (movingIsWhite != whiteToMove) return false; // moving wrong color

        char targetPiece = board[tr, tf];
        bool targetEmpty = (targetPiece == '.');
        // cannot capture own piece
        if (!targetEmpty && (char.IsUpper(targetPiece) == movingIsWhite)) return false;

        int dr = tr - sr;
        int dc = tf - sf;
        int adr = Math.Abs(dr);
        int adc = Math.Abs(dc);
        char up = char.ToUpper(moving);

        bool canMove = false;
        switch (up)
        {
            case 'P':
                int dir = whiteToMove ? -1 : 1; // rows decrease for white moves
                // single forward
                if (dc == 0 && dr == dir && targetEmpty) canMove = true;
                // double forward from start rank (white row 6, black row 1)
                else if (dc == 0 && dr == 2 * dir && targetEmpty)
                {
                    int betweenRank = sr + dir;
                    if (board[betweenRank, sf] == '.' &&
                        ((whiteToMove && sr == 6) || (!whiteToMove && sr == 1)))
                        canMove = true;
                }
                // capture diagonal (normal)
                else if (adc == 1 && dr == dir && !targetEmpty) canMove = true;
                // en-passant capture: target square empty but equals enPassant square
                else if (adc == 1 && dr == dir && targetEmpty && enPassant != "-")
                {
                    int epFile = enPassant[0] - 'a';
                    int epRank = 8 - (enPassant[1] - '0');
                    if (epFile == tf && epRank == tr) canMove = true;
                }
                break;

            case 'N':
                canMove = (adr == 1 && adc == 2) || (adr == 2 && adc == 1);
                break;

            case 'B':
                if (adr == adc) canMove = IsPathClear(board, sr, sf, tr, tf);
                break;

            case 'R':
                if (adr == 0 || adc == 0) canMove = IsPathClear(board, sr, sf, tr, tf);
                break;

            case 'Q':
                if (adr == adc || adr == 0 || adc == 0) canMove = IsPathClear(board, sr, sf, tr, tf);
                break;

            case 'K':
                // normal king move (castling handled separately in ApplyMove)
                canMove = Math.Max(adr, adc) == 1;
                break;
        }

        if (!canMove || allowKingCapture) return canMove;

        // Make the move temporarily to check if it leaves own king in check
        char originalTarget = board[tr, tf];
        board[tr, tf] = board[sr, sf];
        board[sr, sf] = '.';

        // Special handling for en passant capture
        int? epCaptureRank = null;
        char? epCapturePiece = null;
        if (up == 'P' && adc == 1 && dr == (whiteToMove ? -1 : 1) && targetEmpty && enPassant != "-")
        {
            int epFile = enPassant[0] - 'a';
            int epRank = 8 - (enPassant[1] - '0');
            if (epFile == tf && epRank == tr)
            {
                epCaptureRank = whiteToMove ? tr + 1 : tr - 1;
                epCapturePiece = board[epCaptureRank.Value, tf];
                board[epCaptureRank.Value, tf] = '.';
            }
        }

        bool kingInCheck = IsKingInCheck(board, whiteToMove);

        // Restore the board
        board[sr, sf] = board[tr, tf];
        board[tr, tf] = originalTarget;
        if (epCaptureRank.HasValue)
        {
            board[epCaptureRank.Value, tf] = epCapturePiece.Value;
        }

        return !kingInCheck;
    }

    private static string BoardToFEN(char[,] board, bool whiteToMove, string castlingRights, string enPassant, int halfmoveClock, int fullmoveNumber)
    {
        var fen = "";
        for (int r = 0; r < 8; r++)
        {
            int empty = 0;
            for (int c = 0; c < 8; c++)
            {
                if (board[r, c] == '.')
                {
                    empty++;
                }
                else
                {
                    if (empty > 0)
                    {
                        fen += empty;
                        empty = 0;
                    }
                    fen += board[r, c];
                }
            }
            if (empty > 0) fen += empty;
            if (r < 7) fen += "/";
        }
        fen += " " + (whiteToMove ? "w" : "b") + " " + (string.IsNullOrEmpty(castlingRights) ? "-" : castlingRights) +
               " " + enPassant + " " + halfmoveClock + " " + fullmoveNumber;
        return fen;
    }
  
}