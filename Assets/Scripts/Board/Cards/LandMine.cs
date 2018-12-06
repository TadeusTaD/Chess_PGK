using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : BaseCard
{
    public int damageValue;
    private GameObject indicator;

    public void Start()
    {
        description.text = "Zaminuj wybrane pole. Gdy dowolna figura na nim stanie, zadaj jej " + damageValue + "pkt obrazen";
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
        if (hit.collider.transform.GetComponent<Field>().piece == null)
        {
            hit.collider.transform.GetComponent<Field>().onStep += ActivateMine;
            ShowMarker(hit);
            manager.gameMode = Mode.idle;
            manager.GetPlayer().manaPool -= manaCost;
            MoveToGraveyard();
        }

        CancelCardUse();
    }

    public void ActivateMine(Field field)
    {
        field.StartCoroutine(DoBoom(field));
    }

    public IEnumerator DoBoom(Field field)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(indicator);
        field.onStep -= ActivateMine;
        field.piece.RecieveDamageFromEffect(damageValue);
    }

    public void ShowIndicator(RaycastHit hit)
    {
        ClearIndicator();
        indicators.Add(manager.CreateCubeIndicator(hit.collider.transform.position.x, hit.collider.transform.position.y, new Color(255f / 255, 0f / 255, 0f / 255, 0.5f)));
    }

    public void ShowMarker(RaycastHit hit)
    {
        indicator = manager.CreateSphereIndicator(hit.collider.transform.position.x, hit.collider.transform.position.y, hit.collider.transform.position.z - 0.05f, new Color(255f / 255, 0f / 255, 0f / 255, 0.5f));
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
