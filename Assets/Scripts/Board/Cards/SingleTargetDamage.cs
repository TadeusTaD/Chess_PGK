using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetDamage : BaseCard
{
    public int damageValue;

    public void Start()
    {
        description.text = "Zadaj " + damageValue + " pkt obrazen wybranej wrogiej jednostce";
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
        if (hit.collider.transform.GetComponent<Field>().piece.isWhite != manager.whiteTurn)
        {
            hit.collider.transform.GetComponent<Field>().piece.RecieveDamageFromEffect(damageValue);

            manager.gameMode = Mode.idle;
            manager.GetPlayer().manaPool -= manaCost;
            FindObjectOfType<AudioManager>().Play(this.GetType().Name);
            MoveToGraveyard();
        }

        CancelCardUse();
    }

    public void ShowIndicator(RaycastHit hit)
    {
        ClearIndicator();
        indicators.Add(manager.CreateCubeIndicator(hit.collider.transform.position.x, hit.collider.transform.position.y, new Color(255f / 255, 75f / 255, 75f / 255, 0.5f)));
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
