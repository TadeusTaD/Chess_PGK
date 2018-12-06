using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override List<Vector2> GetPossibleMoves(Field[,] board)
    {
        List<Vector2> possibleMoves = new List<Vector2>();
        int directionModifier;

        if (isWhite)
        {
            directionModifier = 1;
        }
        else
        {
            directionModifier = -1;
        }

        print("ZAZNACZONO");
        if (currentY != board.GetLength(0) - 1)
        {
            print("BOARD: " + board.GetLength(0));
            if (board[currentX, currentY + 1 * directionModifier].piece == null)
            {
                possibleMoves.Add(new Vector2(currentX, currentY + 1 * directionModifier));
            }
            if (/*currentY != board.GetLength(0) - 2 && */board[currentX, currentY + 2 * directionModifier].piece == null && ((currentY == 1 && isWhite) || (currentY == board.GetLength(0) - 2 && !isWhite)))
            {
                possibleMoves.Add(new Vector2(currentX, currentY + 2 * directionModifier));
            }
            if (currentX != 0 && board[currentX - 1, currentY + 1 * directionModifier].piece != null &&
                board[currentX - 1, currentY + 1 * directionModifier].piece.isWhite != this.isWhite)
            {
                possibleMoves.Add(new Vector2(currentX - 1, currentY + 1 * directionModifier));
            }
            if (currentX != board.GetLength(0) - 1 && board[currentX + 1, currentY + 1 * directionModifier].piece != null &&
                board[currentX + 1, currentY + 1 * directionModifier].piece.isWhite != this.isWhite)
            {
                possibleMoves.Add(new Vector2(currentX + 1, currentY + 1 * directionModifier));
            }
        }
        return possibleMoves;

        // TODO - bicie w przelocie
    }

    public override Field GetPositionAfterAttack(Field[,] board, Field destination)
    {
        return board[currentX, currentY];
    }
}
