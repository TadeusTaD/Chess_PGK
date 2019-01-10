using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rage : BaseCard
{
    public int damageValue;
    public void Start()
    {
        description.text = "Zadaj " + damageValue + " pkt obrazen WSZYSTKIM jednostkom sasiadujacym z wybrana sojusznicza figura";
        manaCostIndicator.text = manaCost.ToString();
    }
    public override void ActivateEffect(GameManager manager)
    {
        this.manager = manager;
        if (manager.GetPlayer().manaPool < manaCost)
        {
            print("Brak many");
            return;
        }
        print("Mana ok");
        manager.gameMode = Mode.pickField;
        manager.onBoardHit += OnFieldPick;
        manager.onBoardMove += ShowIndicator;
        manager.onVoidMove += ClearIndicator;

        DelegateCancelAdd();
    }
    public void OnFieldPick(RaycastHit hit)
    {
        if (hit.collider.transform.GetComponent<Field>().piece == null || hit.collider.transform.GetComponent<Field>().piece.isWhite != manager.whiteTurn)
        {
            CancelCardUse();
            return;
        }

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                try
                {
                    if (manager.board[(int)hit.collider.transform.position.x + i, (int)hit.collider.transform.position.y + j].piece != null && manager.board[(int)hit.collider.transform.position.x + i, (int)hit.collider.transform.position.y + j] != hit.collider.transform.GetComponent<Field>())
                    {
                        manager.board[(int)hit.collider.transform.position.x + i, (int)hit.collider.transform.position.y + j].piece.RecieveDamageFromEffect(damageValue);
                        
                    }
                }
                catch (System.IndexOutOfRangeException) { }
            }
        }
        manager.GetPlayer().manaPool -= manaCost;
        FindObjectOfType<AudioManager>().Play(this.GetType().Name);
        MoveToGraveyard();
        CancelCardUse();
    }

    public void ShowIndicator(RaycastHit hit)
    {
        ClearIndicator();
        for (int i = -1; i < 2; i++)
            for (int j = -1; j < 2; j++)
            {
                if (hit.collider.transform.position.x + i < manager.board.GetLength(0))
                    indicators.Add(manager.CreateCubeIndicator(hit.collider.transform.position.x + i, hit.collider.transform.position.y + j, new Color(255f / 255, 75f / 255, 75f / 255, 0.5f)));
            }
    }
    public override void CancelCardUse()
    {
        ClearIndicator();
        manager.gameMode = Mode.idle;
        manager.onBoardHit -= OnFieldPick;
        manager.onBoardMove -= ShowIndicator;
        manager.onVoidMove -= ClearIndicator;

        DelegateCancelClear();
    }
}
