using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserker : BaseCard {

    public float convertRatio;

    public void Start()
    {
        description.text = "Zmniejsz zdrowie wybranej sojuszniczej jednostki do 1 i zwieksz atak tej jednostki za " + convertRatio * 100 + "% zabranego zdrowia";
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
            hit.collider.transform.GetComponent<Field>().piece.attack += (int)(hit.collider.transform.GetComponent<Field>().piece.hp * convertRatio);
            hit.collider.transform.GetComponent<Field>().piece.hp = 1;
            manager.GetPlayer().manaPool -= manaCost;
            gameObject.GetComponent<AudioManager>().Play(this.GetType().Name);
            MoveToGraveyard();
        }
        CancelCardUse();
    }
    public void ShowIndicator(RaycastHit hit)
    {
        ClearIndicator();
        indicators.Add(manager.CreateCubeIndicator(hit.collider.transform.position.x, hit.collider.transform.position.y, new Color(255f/255, 128f/255, 128f/255, 1)));
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
