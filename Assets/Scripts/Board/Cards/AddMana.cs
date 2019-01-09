using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMana : BaseCard {

    public int manaValue;

    public void Start()
    {
        description.text = "Zyskaj " + manaValue + " pkt many";
        manaCostIndicator.text = manaCost.ToString();
    }

    public override void ActivateEffect(GameManager manager)
    {
        gameObject.GetComponent<AudioManager>().Play(this.GetType().Name);
        this.manager = manager;
        if (manager.GetPlayer().manaPool < manaCost)
        {
            print("Brak many");
            return;
        }
        print("Mana ok");

        manager.GetPlayer().manaPool += manaValue;
        manager.GetPlayer().manaPool -= manaCost;
        MoveToGraveyard();
    }

    public override void CancelCardUse()
    {
        // Nie rob nic, karta posiada jeden stopien aktywacji
    }
}
