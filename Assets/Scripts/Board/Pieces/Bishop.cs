using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{
    public override List<Vector2> GetPossibleMoves(Field[,] board)
    {
        List<Vector2> possibleMoves = new List<Vector2>();
        int i = 1;

        // Ruchy w prawo w gore
        while (currentX + i < board.GetLength(0) && currentY + i < board.GetLength(0))
        {
            if (board[currentX + i, currentY + i].piece == null)
            {
                possibleMoves.Add(new Vector2(currentX + i, currentY + i));
            }
            else
            {
                if (board[currentX + i, currentY + i].piece.isWhite != this.isWhite)
                {
                    possibleMoves.Add(new Vector2(currentX + i, currentY + i));
                }
                break;
            }
            i++;
        }

        i = 1;
        // Ruchy w prawo w dol
        while (currentX + i < board.GetLength(0) && currentY - i >= 0)
        {
            if (board[currentX + i, currentY - i].piece == null)
            {
                possibleMoves.Add(new Vector2(currentX + i, currentY - i));
            }
            else
            {
                if (board[currentX + i, currentY - i].piece.isWhite != this.isWhite)
                {
                    possibleMoves.Add(new Vector2(currentX + i, currentY - i));
                }
                break;
            }
            i++;
        }

        i = 1;
        // Ruchy w lewo w dol
        while (currentX - i >= 0 && currentY - i >= 0)
        {
            if (board[currentX - i, currentY - i].piece == null)
            {
                possibleMoves.Add(new Vector2(currentX - i, currentY - i));
            }
            else
            {
                if (board[currentX - i, currentY - i].piece.isWhite != this.isWhite)
                {
                    possibleMoves.Add(new Vector2(currentX - i, currentY - i));
                }
                break;
            }
            i++;
        }

        i = 1;
        // Ruchy w lewo w gore
        while (currentX - i >= 0 && currentY + i < board.GetLength(0))
        {
            if (board[currentX - i, currentY + i].piece == null)
            {
                possibleMoves.Add(new Vector2(currentX - i, currentY + i));
            }
            else
            {
                if (board[currentX - i, currentY + i].piece.isWhite != this.isWhite)
                {
                    possibleMoves.Add(new Vector2(currentX - i, currentY + i));
                }
                break;
            }
            i++;
        }


        return possibleMoves;
    }
    public override Field GetPositionAfterAttack(Field[,] board, Field destination)
    {
        if (Mathf.Abs(currentX - destination.transform.position.x) <= 2)
            return board[currentX, currentY];
        else
        {
            return board[(int)(destination.transform.position.x + (2 * (currentX - destination.transform.position.x) / Mathf.Abs(currentX - destination.transform.position.x))), (int)(destination.transform.position.y + (2 * (currentY - destination.transform.position.y) / Mathf.Abs(currentY - destination.transform.position.y)))];
        }
    }
}