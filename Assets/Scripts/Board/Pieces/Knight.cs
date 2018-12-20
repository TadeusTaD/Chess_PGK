using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override List<Vector2> GetPossibleMoves(Field[,] board)
    {
        List<Vector2> possibleMoves = new List<Vector2>();

        // Opcja 2up 1right
        if (currentY + 2 < board.GetLength(0) && currentX + 1 < board.GetLength(0))
        {
            if (board[currentX + 1, currentY + 2].piece == null || board[currentX + 1, currentY + 2].piece.isWhite != this.isWhite)
            {
                possibleMoves.Add(new Vector2(currentX + 1, currentY + 2));
            }
        }

        // Opcja 2right 1up
        if (currentX + 2 < board.GetLength(0) && currentY + 1 < board.GetLength(0))
        {
            if (board[currentX + 2, currentY + 1].piece == null || board[currentX + 2, currentY + 1].piece.isWhite != this.isWhite)
            {
                possibleMoves.Add(new Vector2(currentX + 2, currentY + 1));
            }
        }

        // Opcja 2right 1 down
        if (currentX + 2 < board.GetLength(0) && currentY - 1 >= 0)
        {
            if (board[currentX + 2, currentY - 1].piece == null || board[currentX + 2, currentY - 1].piece.isWhite != this.isWhite)
            {
                possibleMoves.Add(new Vector2(currentX + 2, currentY - 1));
            }
        }

        // Opcja 2down 1right
        if (currentY - 2 >= 0 && currentX + 1 < board.GetLength(0))
        {
            if (board[currentX + 1, currentY - 2].piece == null || board[currentX + 1, currentY - 2].piece.isWhite != this.isWhite)
            {
                possibleMoves.Add(new Vector2(currentX + 1, currentY - 2));
            }
        }

        // Opcja 2down 1left
        if (currentY - 2 >= 0 && currentX - 1 >= 0)
        {
            if (board[currentX - 1, currentY - 2].piece == null || board[currentX - 1, currentY - 2].piece.isWhite != this.isWhite)
            {
                possibleMoves.Add(new Vector2(currentX - 1, currentY - 2));
            }
        }

        // Opcja 2left 1down
        if (currentX - 2 >= 0 && currentY - 1 >= 0)
        {
            if (board[currentX - 2, currentY - 1].piece == null || board[currentX - 2, currentY - 1].piece.isWhite != this.isWhite)
            {
                possibleMoves.Add(new Vector2(currentX - 2, currentY - 1));
            }
        }

        // Opcja 2left 1up
        if (currentX - 2 >= 0 && currentY + 1 < board.GetLength(0))
        {
            if (board[currentX - 2, currentY + 1].piece == null || board[currentX - 2, currentY + 1].piece.isWhite != this.isWhite)
            {
                possibleMoves.Add(new Vector2(currentX - 2, currentY + 1));
            }
        }

        // Opcja 2up 1left
        if (currentY + 2 < board.GetLength(0) && currentX - 1 >= 0)
        {
            if (board[currentX - 1, currentY + 2].piece == null || board[currentX - 1, currentY + 2].piece.isWhite != this.isWhite)
            {
                possibleMoves.Add(new Vector2(currentX - 1, currentY + 2));
            }
        }

        return possibleMoves;
    }
    public override Field GetPositionAfterAttack(Field[,] board, Field destination)
    {
        return board[currentX, currentY];
	}

	private static readonly float[,] strategicValues =
		{
			{ -50,-40,-30,-30,-30,-30,-40,-50, },
			{ -40,-20,  0,  0,  0,  0,-20,-40, },
			{ -30,  0, 10, 15, 15, 10,  0,-30, },
			{ -30,  5, 15, 20, 20, 15,  5,-30, },
			{ -30,  0, 15, 20, 20, 15,  0,-30, },
			{ -30,  5, 10, 15, 15, 10,  5,-30, },
			{ -40,-20,  0,  5,  5,  0,-20,-40, },
			{ -50,-40,-30,-30,-30,-30,-40,-50, },
		};
	public override float GetStrategicValue(Vector2Int position, int hp, int attack)
	{
		return 32 * (10 + hp + attack) + strategicValues[position.x, isWhite ? position.y ^ 7 : position.y];
	}
}
