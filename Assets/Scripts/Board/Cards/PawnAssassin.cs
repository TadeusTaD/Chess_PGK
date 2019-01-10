using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnAssassin : BaseCard
{

    public int attackBoost;

    public void Start()
    {
        description.text = "Wybrany sojuszniczy PION zyskuje " + attackBoost + " pkt ataku";
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
            if (hit.collider.transform.GetComponent<Field>().piece is Pawn)
            {
                hit.collider.transform.GetComponent<Field>().piece.attack += attackBoost;
                manager.GetPlayer().manaPool -= manaCost;
                FindObjectOfType<AudioManager>().Play(this.GetType().Name);
                MoveToGraveyard();
            }
        }
        CancelCardUse();
    }
    public void ShowIndicator(RaycastHit hit)
    {
        ClearIndicator();
        indicators.Add(manager.CreateCubeIndicator(hit.collider.transform.position.x, hit.collider.transform.position.y, new Color(128f/ 255, 128f/ 255, 255f/255, 1)));
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
