using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangerousArea : BaseCard
{
    public int explosionDamage;
    public int turnsToExplosion;
    private int _delay;
    public int delay
    {
        get { return _delay;  }
        set
        {
            _delay = value;
            canvas.transform.Find("Text").GetComponent<Text>().text = value.ToString();
        }
    }

    bool activated = false;

    public Canvas canvas;
    public SpriteRenderer blurImage; 

    void Start()
    {
        canvas.transform.parent = null;
        blurImage.transform.parent = null;
        canvas.transform.localScale = new Vector3(1, 1, 0);
        blurImage.transform.localScale = new Vector3(2, 2, 0);
        canvas.transform.position = new Vector3(3.5f, 3.5f, 10);
        blurImage.transform.position = new Vector3(3.5f, 3.5f, 10);

        delay = turnsToExplosion;
        description.text = "Po " + delay + " turach nastepuje wybuch zadajacy " + explosionDamage + " pkt obrazen wszystkim jednostkom na 4 centralnych polach.";
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

        ShowIndicator();
        ShowCounter();
        activated = true;
        PlantExpolsions();

        manager.GetPlayer().manaPool -= manaCost;
        MoveToGraveyard();
    }

    public new void OnMouseEnter()
    {
        base.OnMouseEnter();
        ShowIndicator();
    }

    public new void OnMouseExit()
    {
        base.OnMouseExit();
        if (!activated)
        { 
            HideIndicator();
        }
    }
    private void ShowIndicator()
    {
        blurImage.transform.localPosition = new Vector3(3.5f, 3.5f, -5);
    }
    private void ShowCounter()
    {
        canvas.transform.localPosition = new Vector3(3.5f, 3.5f, -5);
    }
    private void HideIndicator()
    {
        blurImage.transform.localPosition = new Vector3(3.5f, 3.5f, 10);
        canvas.transform.localPosition = new Vector3(3.5f, 3.5f, 10);
    }

    private void PlantExpolsions()
    {
        manager.onNewTurn += CountToBoom;
    }

    private void CountToBoom()
    {
        if (--delay == 0)
            DoBoom();
    }
    private void DoBoom()
    {
        manager.board[3,3].piece.RecieveDamageFromEffect(explosionDamage);
        manager.board[3,4].piece.RecieveDamageFromEffect(explosionDamage);
        manager.board[4,3].piece.RecieveDamageFromEffect(explosionDamage);
        manager.board[4,4].piece.RecieveDamageFromEffect(explosionDamage);

        HideIndicator();
    }

    public override void CancelCardUse()
    {
        // Nie rob nic, karta posiada jeden stopien aktywacji
    }
}
