using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingGrounds : BaseCard {

    public int healAmount;
    public int maxTurns;
    private List<GameObject> plusesIndicators = new List<GameObject>();
    private List<Field> healingGrounds = new List<Field>();
    private int turns = 0;

    public void Start ()
    {
        if (maxTurns <= 4)
            description.text = "Wybierz obszar 5x5, przez nastepne " + maxTurns + " tury ulecz WSZYSTKIE figury na tym obszarze za " + healAmount + " pkt obrazen";
        else
            description.text = "Wybierz obszar 5x5, przez nastepne " + maxTurns + " tur ulecz WSZYSTKIE figury na tym obszarze za " + healAmount + " pkt obrazen";

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
        for (int i = -2; i < 3; i++)
            for (int j = -2; j < 3; j++)
            {
                try
                {
                    healingGrounds.Add(manager.board[(int)(hit.collider.transform.position.x + i), (int)(hit.collider.transform.position.y + j)]);
                }
                catch (System.IndexOutOfRangeException)
                {
                    print("Exception");
                }
            }
        manager.GetPlayer().manaPool -= manaCost;
        FindObjectOfType<AudioManager>().Play(this.GetType().Name);
        GameObject.Find("HealingGroundEffect").transform.position = hit.collider.transform.GetComponent<Field>().piece.transform.position;
        GameObject.Find("HealingGroundEffect").GetComponent<ParticleSystem>().Play();
        MoveToGraveyard();
        CancelCardUse();
        manager.onNewTurn += Heal;
        //ShowMarkers();
    }
    private void Heal()
    {
        turns++;
        foreach (Field f in healingGrounds)
        {
            try
            {
                f.piece.hp += healAmount;
            }
            catch (System.NullReferenceException) { }
        }

        if (turns == maxTurns)
        {
            ClearPluses();
        }
    }

    private void ClearPluses()
    {
        manager.onNewTurn -= Heal;

        //foreach (GameObject obj in plusesIndicators)
        //{
        //    Destroy(obj);
        //}
        GameObject.Find("HealingGroundEffect").GetComponent<ParticleSystem>().Stop();
    }

    public void ShowMarkers()
    {
        foreach (Field f in healingGrounds)
        {
            GameObject obj1 = manager.CreateCubeIndicator(f.transform.position.x, f.transform.position.y, new Color(100f / 255, 255f / 255, 150f / 255, 1));
            GameObject obj2 = manager.CreateCubeIndicator(f.transform.position.x, f.transform.position.y, new Color(100f / 255, 255f / 255, 150f / 255, 1));
            obj1.transform.localScale = new Vector3(0.1f, 0.9f, 1);
            obj2.transform.localScale = new Vector3(0.9f, 0.1f, 1);
            plusesIndicators.Add(obj1);
            plusesIndicators.Add(obj2);
        }
    }

    public void ShowIndicator(RaycastHit hit)
    {
        ClearIndicator();
        for (int i = -2; i < 3; i++)
            for (int j = -2; j < 3; j++)
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
