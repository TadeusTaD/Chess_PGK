using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAI
{
	public IEnumerator MakeMove(GameManager manager)
	{
		yield return new WaitForSeconds(4);

		bool isWhite = manager.GetPlayer().isWhite;
		List<Field> validSelections = manager.GetAllFields().FindAll(
			field => field.piece != null && field.piece.isWhite == isWhite
		);

		Field decisionSource = null;
		Field decisionDestination = null;
		float bestScore = float.NegativeInfinity;

		foreach (Field selection in validSelections)
		{
			List<Vector2> possibleMoves = selection.piece.GetPossibleMoves(manager.board);
			foreach (var move in possibleMoves)
			{
				Field target = manager.board[(int)move.x, (int)move.y];

				float score = GetMoveScore(selection, target, manager.board);
				if (score > bestScore)
				{
					bestScore = score;
					decisionSource = selection;
					decisionDestination = target;
				}
			}
		}

		manager.ProcessSelectField(decisionSource);
		manager.ProcessTryToMovePiece(decisionDestination);

		yield return new WaitForSeconds(0);
	}

	private float GetMoveScore(Field source, Field destination, Field[,] board)
	{
		float result = -source.piece.GetCurrentStrategicValue();
		if (destination.piece != null)
		{
			result += destination.piece.GetCurrentStrategicValue();

			int newHP = destination.piece.hp - source.piece.attack;
			if (newHP > 0)
			{
				Vector2Int position = new Vector2Int(destination.piece.currentX, destination.piece.currentY);
				result -= destination.piece.GetStrategicValue(position, newHP, destination.piece.attack);
				destination = source.piece.GetPositionAfterAttack(board, destination);
			}
		}
		Vector2Int newPosition = new Vector2Int((int)destination.transform.position.x, (int)destination.transform.position.y);
		result += source.piece.GetStrategicValue(newPosition, source.piece.hp, source.piece.attack);

		return result;
	}
}
