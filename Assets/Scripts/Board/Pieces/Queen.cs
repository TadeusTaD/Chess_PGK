using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece
{
    public override List<Vector2> GetPossibleMoves(Field[,] board)
    {
        List<Vector2> possibleMoves = new List<Vector2>();
        int i = 1;

        // Ruchy w prawo
        while (currentX + i != board.GetLength(0))
        {
            if (board[currentX + i, currentY].piece == null)
            {
                possibleMoves.Add(new Vector2(currentX + i, currentY));
            }
            else
            {
                if (board[currentX + i, currentY].piece.isWhite != this.isWhite)
                {
                    possibleMoves.Add(new Vector2(currentX + i, currentY));
                }
                break;
            }
            i++;
        }

        // Ruchy w lewo
        i = 1;
        while (currentX - i != -1)
        {
            if (board[currentX - i, currentY].piece == null)
            {
                possibleMoves.Add(new Vector2(currentX - i, currentY));
            }
            else
            {
                if (board[currentX - i, currentY].piece.isWhite != this.isWhite)
                {
                    possibleMoves.Add(new Vector2(currentX - i, currentY));
                }
                break;
            }
            i++;
        }

        // Ruchy w gore
        i = 1;
        while (currentY + i != board.GetLength(0))
        {
            if (board[currentX, currentY + i].piece == null)
            {
                possibleMoves.Add(new Vector2(currentX, currentY + i));
            }
            else
            {
                if (board[currentX, currentY + i].piece.isWhite != this.isWhite)
                {
                    possibleMoves.Add(new Vector2(currentX, currentY + i));
                }
                break;
            }
            i++;
        }

        // Ruchy w dol
        i = 1;
        while (currentY - i != -1)
        {
            if (board[currentX, currentY - i].piece == null)
            {
                possibleMoves.Add(new Vector2(currentX, currentY - i));
            }
            else
            {
                if (board[currentX, currentY - i].piece.isWhite != this.isWhite)
                {
                    possibleMoves.Add(new Vector2(currentX, currentY - i));
                }
                break;
            }
            i++;
        }

        i = 1;

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
        if (currentX == destination.transform.position.x)
        {
            // Atakujemy w pionie
            if (Mathf.Abs(currentY - destination.transform.position.y) <= 2)
                return board[currentX, currentY];
            else
                return board[currentX, (int)(destination.transform.position.y + (2 * (currentY - destination.transform.position.y) / Mathf.Abs(currentY - destination.transform.position.y)))];
        }
        else if (currentY == destination.transform.position.y)
        {
            // Atakujemy w poziomie
            if (Mathf.Abs(currentX - destination.transform.position.x) <= 2)
                return board[currentX, currentY];
            else
                return board[(int)(destination.transform.position.x + (2 * (currentX - destination.transform.position.x) / Mathf.Abs(currentX - destination.transform.position.x))), (int)destination.transform.position.y];
        }
        else if (Mathf.Abs(currentX - destination.transform.position.x) <= 2)
        {
            return board[currentX, currentY];
        }
        else
        {
            return board[(int)(destination.transform.position.x + (2 * (currentX - destination.transform.position.x) / Mathf.Abs(currentX - destination.transform.position.x))), (int)(destination.transform.position.y + (2 * (currentY - destination.transform.position.y) / Mathf.Abs(currentY - destination.transform.position.y)))];
        }
    }
}
