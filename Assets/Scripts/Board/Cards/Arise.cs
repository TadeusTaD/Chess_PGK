using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arise : BaseCard
{
    void Start()
    {
        description.text = "Wybierz karte z cmentarza i dodaj ja do swojej reki. Zmniejsz koszt many tej karty do zera.";
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

        manager.onVoidHit += CancelCardUse;
        manager.onBoardHit += CancelCardUse;
        manager.onCardHit += Ressurect;
    }

    public void Ressurect(RaycastHit hit)
    {
        BaseCard card = hit.collider.GetComponent<BaseCard>();
        if (manager.GetPlayer().graveyard.Contains(card.gameObject))
        {
            manager.GetPlayer().graveyard.Remove(card.gameObject);
            manager.GetPlayer().hand.Add(card.gameObject);
            card.manaCost = 0;

            card.transform.parent = null;
            manager.GetPlayer().RenderHand();
        }

        CancelCardUse();
    }

    public override void CancelCardUse()
    {
        manager.onVoidHit -= CancelCardUse;
        manager.onBoardHit -= CancelCardUse;
        manager.onCardHit -= Ressurect;
        DelegateCancelClear();
    }
}
