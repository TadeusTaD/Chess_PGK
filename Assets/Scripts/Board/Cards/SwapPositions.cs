using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapPositions : BaseCard {

    private Field field1, field2;
    private bool picked1, picked2;
    private GameObject obj1, obj2;

    public void Start()
    {
        description.text = "Zamien dwie sojusznicze jednostki miejscami";
        manaCostIndicator.text = manaCost.ToString();

        picked1 = false;
        picked2 = false;
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
        manager.onBoardHit = OnFieldPick1;
        manager.onBoardMove += ShowIndicator;
        manager.onVoidMove += ClearIndicator;

        DelegateCancelAdd();
    }

    public void OnFieldPick1(RaycastHit hit)
    {
        if (hit.collider.transform.GetComponent<Field>().piece != null && hit.collider.transform.GetComponent<Field>().piece.isWhite == manager.whiteTurn)
        {
            field1 = hit.collider.transform.GetComponent<Field>();
            picked1 = true;
            manager.onBoardHit -= OnFieldPick1;
            manager.onBoardHit += OnFieldPick2;
        } else
        {
            CancelCardUse();
        }
    }
    public void OnFieldPick2(RaycastHit hit)
    {
        if (hit.collider.transform.GetComponent<Field>().piece != null && hit.collider.transform.GetComponent<Field>().piece.isWhite == manager.whiteTurn)
        {
            field2 = hit.collider.transform.GetComponent<Field>();
            picked1 = false;
            picked2 = true;

            manager.gameMode = Mode.idle;

            ChessPiece tmp = field1.piece;
            field1.piece = field2.piece;
            field2.piece = tmp;

            field1.piece.transform.position = new Vector3(field1.transform.position.x, field1.transform.position.y, 0);
            field2.piece.transform.position = new Vector3(field2.transform.position.x, field2.transform.position.y, 0);
            field1.piece.currentX = (int)field1.transform.position.x;
            field1.piece.currentY = (int)field1.transform.position.y;
            field2.piece.currentX = (int)field2.transform.position.x;
            field2.piece.currentY = (int)field2.transform.position.y;


            manager.GetPlayer().manaPool -= manaCost;
            gameObject.GetComponent<AudioManager>().Play(this.GetType().Name);
            MoveToGraveyard();
        }
        CancelCardUse();
    }

    public void ShowIndicator(RaycastHit hit)
    {
        if (!picked1 && !picked2)
        {
            ClearOneIndicator(0);
            obj1 = manager.CreateCubeIndicator(hit.collider.transform.position.x, hit.collider.transform.position.y, new Color(255f / 255, 175f / 255, 75f / 255, 0.5f));
        }
        if (picked1 && !picked2)
        {
            ClearOneIndicator(1);
            obj2 = manager.CreateCubeIndicator(hit.collider.transform.position.x, hit.collider.transform.position.y, new Color(10f / 255, 155f / 255, 210f / 255, 0.5f));
        }
    }

    public void ClearOneIndicator(int nr)
    {
        if (nr == 0)
            Destroy(obj1);
        else if (nr == 1)
            Destroy(obj2);
    }
    public override void ClearIndicator()
    {
        if (!picked1)
            ClearOneIndicator(0);
        if (!picked2)
            ClearOneIndicator(1);
    }

    public override void CancelCardUse()
    {
        Destroy(obj1);
        Destroy(obj2);
        manager.gameMode = Mode.idle;
        manager.onBoardHit -= OnFieldPick1;
        manager.onBoardHit -= OnFieldPick2;
        manager.onBoardMove -= ShowIndicator;
        manager.onVoidMove -= ClearIndicator;

        DelegateCancelClear();

        picked1 = false;
        picked2 = false;
        field1 = null;
        field2 = null;
    }
}
