using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideBomb : BaseCard
{

    public void Start()
    {
        description.text = "Zabij wybrana sojusznicza jednostke i zadaj wszystkim sasiadujacym z nia wrogom obrazenia rowne pozostalego zdrowia twijej figury";
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
        if (hit.collider.transform.GetComponent<Field>().piece != null && hit.collider.transform.GetComponent<Field>().piece.isWhite == manager.whiteTurn)
        {
            for (int i=-1;i<2;i++)
            {
                for (int j=-1;j<2;j++)
                {
                    if (manager.board[(int)hit.collider.transform.position.x + i, (int)hit.collider.transform.position.y + j].piece.isWhite != manager.whiteTurn)
                    {
                        manager.board[(int)hit.collider.transform.position.x + i, (int)hit.collider.transform.position.y + j].piece.hp -= hit.collider.transform.GetComponent<Field>().piece.hp;
                    }
                }
            }
            hit.collider.transform.GetComponent<Field>().piece.hp = 0;
            manager.GetPlayer().manaPool -= manaCost;
            MoveToGraveyard();
        }
        CancelCardUse();
    }
    public void ShowIndicator(RaycastHit hit)
    {
        ClearIndicator();
        indicators.Add(manager.CreateCubeIndicator(hit.collider.transform.position.x, hit.collider.transform.position.y, Color.green));
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

