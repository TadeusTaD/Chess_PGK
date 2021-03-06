﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeHeal : BaseCard {

    public int healValue;

    public void Start()
    {
        description.text = "Ulecz " + healValue + " pkt zdrowia sojuszniczym jednostkom na obszarze 3x3";
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
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                try
                {
                    if (manager.board[(int)hit.collider.transform.position.x + i, (int)hit.collider.transform.position.y + j].piece != null && manager.board[(int)hit.collider.transform.position.x + i, (int)hit.collider.transform.position.y + j].piece.isWhite == manager.whiteTurn)
                    {
                        manager.board[(int)hit.collider.transform.position.x + i, (int)hit.collider.transform.position.y + j].piece.hp += healValue;
                        
                    }
                }
                catch (System.IndexOutOfRangeException) { }
            }
        }
        manager.GetPlayer().manaPool -= manaCost;
        FindObjectOfType<AudioManager>().Play(this.GetType().Name);
        GameObject.Find("AoeHealingEffect").transform.position = hit.collider.transform.GetComponent<Field>().piece.transform.position;
        GameObject.Find("AoeHealingEffect").GetComponent<ParticleSystem>().Play();
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
                    indicators.Add(manager.CreateCubeIndicator(hit.collider.transform.position.x + i, hit.collider.transform.position.y + j, Color.green));
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
