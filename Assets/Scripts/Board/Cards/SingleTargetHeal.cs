using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetHeal : BaseCard {

    public int healValue;

    public void Start()
    {
        description.text = "Ulecz wybrana sojusznicza jednostke za " + healValue + " pkt zycia";
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
            hit.collider.transform.GetComponent<Field>().piece.hp += healValue;
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
