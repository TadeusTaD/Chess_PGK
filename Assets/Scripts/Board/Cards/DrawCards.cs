using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCards : BaseCard {

    public int cardsAmount;

    public void Start()
    {
        description.text = "Dobierz " + cardsAmount;
        if (cardsAmount < 5)
            description.text += " karty";
        else
            description.text += " kart";
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

        for (int i = 0; i < cardsAmount; i++)
            manager.GetPlayer().DrawCard();
        manager.GetPlayer().manaPool -= manaCost;
        FindObjectOfType<AudioManager>().Play(this.GetType().Name);
        MoveToGraveyard();
    }

    public override void CancelCardUse()
    {
        // Nie rob nic, karta posiada jeden stopien aktywacji
    }
}
