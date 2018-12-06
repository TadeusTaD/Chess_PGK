using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class King : ChessPiece
{
    public override List<Vector2> GetPossibleMoves(Field[,] board)
    {
        List<Vector2> possibleMoves = new List<Vector2>();

        // gora
        if (currentY + 1 < board.GetLength(0))
        {
            if (board[currentX, currentY + 1].piece == null || board[currentX, currentY + 1].piece.isWhite != this.isWhite)
            {
                possibleMoves.Add(new Vector2(currentX, currentY + 1));
            }
        }

        // gora prawo
        if (currentX + 1 < board.GetLength(0) && currentY + 1 < board.GetLength(0))
        {
            if (board[currentX + 1, currentY + 1].piece == null || board[currentX + 1, currentY + 1].piece.isWhite != this.isWhite)
            {
                possibleMoves.Add(new Vector2(currentX + 1, currentY + 1));
            }
        }

        // prawo
        if (currentX + 1 < board.GetLength(0))
        {
            if (board[currentX + 1, currentY].piece == null || board[currentX + 1, currentY].piece.isWhite != this.isWhite)
            {
                possibleMoves.Add(new Vector2(currentX + 1, currentY));
            }
        }

        // dol prawo
        if (currentY - 1 >= 0 && currentX + 1 < board.GetLength(0))
        {
            if (board[currentX + 1, currentY - 1].piece == null || board[currentX + 1, currentY - 1].piece.isWhite != this.isWhite)
            {
                possibleMoves.Add(new Vector2(currentX + 1, currentY - 1));
            }
        }

        // dol
        if (currentY - 1 >= 0)
        {
            if (board[currentX, currentY - 1].piece == null || board[currentX, currentY - 1].piece.isWhite != this.isWhite)
            {
                possibleMoves.Add(new Vector2(currentX, currentY - 1));
            }
        }

        // dol lewo
        if (currentY - 1 >= 0 && currentX - 1 >= 0)
        {
            if (board[currentX - 1, currentY - 1].piece == null || board[currentX - 1, currentY - 1].piece.isWhite != this.isWhite)
            {
                possibleMoves.Add(new Vector2(currentX - 1, currentY - 1));
            }
        }

        // lewo
        if (currentX - 1 >= 0)
        {
            if (board[currentX - 1, currentY].piece == null || board[currentX - 1, currentY].piece.isWhite != this.isWhite)
            {
                possibleMoves.Add(new Vector2(currentX - 1, currentY));
            }
        }

        // lewo gora
        if (currentX - 1 >= 0 && currentY + 1 < board.GetLength(0))
        {
            if (board[currentX - 1, currentY + 1].piece == null || board[currentX - 1, currentY + 1].piece.isWhite != this.isWhite)
            {
                possibleMoves.Add(new Vector2(currentX - 1, currentY + 1));
            }
        }

        return possibleMoves;
    }
    public override Field GetPositionAfterAttack(Field[,] board, Field destination)
    {
        return board[currentX, currentY];
    }
    public override void DestroyPiece()
    {
        Destroy(this.gameObject);
        SceneManager.LoadScene("MainMenu");
    }
}
