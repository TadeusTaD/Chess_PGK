using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public delegate void OnStep(Field field);
    public OnStep onStep;
    public bool isWhite;
    private GameManager manager;
    private ChessPiece _piece;
    public ChessPiece piece
    {
        get
        {
            return _piece;
        }
        set
        {
            _piece = value;
            if (value != null)
                ActivateEffect(_piece);
        }
    }

    public void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if ((int)(transform.position.x + transform.position.y) % 2 == 0)
            isWhite = false;
        else
            isWhite = true;


    }
    public void ActivateEffect(ChessPiece piece)
    {
        //print("Aktywowano efekt pola " + transform.position.x + ", " + transform.position.y);
        this._piece = piece;
        try
        {
            onStep(this);
        }
        catch (System.NullReferenceException) { }
    }
}