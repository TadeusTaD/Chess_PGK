using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum Mode { blocked, idle, readyToMove, pickField }
public class Manager : MonoBehaviour
{

    /*private Vector2 activeField;
    private GameObject fieldMark;
    private List<GameObject> moveMarks = new List<GameObject>();

    public List<GameObject> cardPrefabs = new List<GameObject>();
    public bool whiteTurn = false;
    public delegate void OnHit(RaycastHit hit);
    public delegate void OnNewTurn();
    public ChessPiece[,] board;
    public Player whitePlayer;
    public Player blackPlayer;
    public OnHit onBoardHit;
    public OnHit onCardHit;
    public OnNewTurn onNewTurn;
    public GameObject turnIndicator;
    public Mode gameMode = Mode.blocked;
    public int turn = 0;

    void Start()
    {
        PreparePieces();
        PreparePlayers();
        onNewTurn += EndTurn;
        SwitchTurns();

    }

    void Update()
    {
        GetRaycastHitOnClick();
    }

    private void PreparePlayers()
    {
        whitePlayer.isWhite = true;
        blackPlayer.isWhite = false;

        whitePlayer.PrepareCards(cardPrefabs);
        blackPlayer.PrepareCards(cardPrefabs);

        whitePlayer.maxMana = 0;
        blackPlayer.maxMana = 1;
    }
    private void PreparePieces()
    {
        GameObject pieces = GameObject.Find("Pieces");

        // Tablica z figurami podzielonymi na kolory
        Transform[] piecesByColor = new Transform[2];

        // Podzial figur na kolory
        piecesByColor[0] = pieces.transform.GetChild(0);
        piecesByColor[1] = pieces.transform.GetChild(1);

        // Tablice z figurami podzielonymi na typy
        Transform[] whitePiecesByType = new Transform[piecesByColor[0].transform.childCount];
        Transform[] blackPiecesByType = new Transform[piecesByColor[1].transform.childCount];

        // Podzial kolorow figur na typy
        for (int type = 0; type < piecesByColor[0].childCount; type++)
        {
            whitePiecesByType[type] = piecesByColor[0].GetChild(type);
        }
        for (int type = 0; type < piecesByColor[1].childCount; type++)
        {
            blackPiecesByType[type] = piecesByColor[1].GetChild(type);
        }

        List<Transform> whitePieces = new List<Transform>();
        List<Transform> blackPieces = new List<Transform>();

        // Podzial typow figur na pojedyncze figury
        // Biale
        for (int type = 0; type < whitePiecesByType.Length; type++)
        {
            for (int pawn = 0; pawn < whitePiecesByType[type].childCount; pawn++)
            {
                whitePieces.Add(whitePiecesByType[type].GetChild(pawn));
            }
        }
        // Czarne
        for (int type = 0; type < blackPiecesByType.Length; type++)
        {
            for (int pawn = 0; pawn < blackPiecesByType[type].childCount; pawn++)
            {
                blackPieces.Add(blackPiecesByType[type].GetChild(pawn));
            }
        }

        List<Transform> listOfAllPawns = whitePieces;
        listOfAllPawns.AddRange(blackPieces);

        PrepareBoard(listOfAllPawns);

        // Zwolnienie pamieci
        piecesByColor = null;
        whitePiecesByType = null;
        blackPiecesByType = null;
        whitePieces = null;
        blackPieces = null;
        listOfAllPawns = null;
    }
    private void PrepareBoard(List<Transform> list)
    {
        GameObject boardFields = GameObject.Find("Board");
        int boardSize = (int)Mathf.Sqrt(boardFields.transform.GetChild(0).transform.childCount + boardFields.transform.GetChild(1).transform.childCount);
        board = new ChessPiece[boardSize, boardSize];

        foreach (Transform piece in list)
        {
            board[(int)piece.position.x, (int)piece.position.y] = piece.GetComponent<ChessPiece>();
        }

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (board[i, j] != null)
                {
                    board[i, j].currentX = i;
                    board[i, j].currentY = j;
                }
            }
        }
    }
    private void GetRaycastHitOnClick()
    {
        if (Input.GetMouseButtonDown(0) && gameMode != Mode.blocked)
        {
            RaycastHit hit = GetRaycastHit();
            if (hit.point.x == -1)
            {
                ResetAllIndicators();
            }
            else if (hit.point.x < board.GetLength(0))
            {
                HitBoard(hit);
            }
            else
            {
                HitCards(hit);
            }
        }
    }
    private RaycastHit GetRaycastHit()
    {
        RaycastHit hit;
        Vector2 selection;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, LayerMask.GetMask("Raycastable")))
        {
            selection = new Vector2(Mathf.Round(hit.point.x), Mathf.Round(hit.point.y));
        }

        return hit;
    }
    private void ResetAllIndicators()
    {
        try
        {
            Destroy(fieldMark);
            fieldMark = null;
        }
        catch (System.NullReferenceException)
        {

        }
        foreach (GameObject indicator in moveMarks)
        {
            Destroy(indicator);
        }
        moveMarks.Clear();
        activeField = new Vector2(-1, -1);
    }
    private void HitBoard(RaycastHit hit)
    {
        if (gameMode == Mode.idle)
        {
            onBoardHit = SelectField;
        }
        else if (gameMode == Mode.readyToMove)
        {
            onBoardHit = TryToMovePiece;
        }
        else if (gameMode == Mode.pickField)
        {
            onBoardHit = PickField;
        }
        else if (gameMode == Mode.blocked)
        {
            return;
        }
        onBoardHit(hit);
    }
    private void HitCards(RaycastHit hit)
    {
        if (hit.collider.transform.GetComponent<BaseCard>() != null)
        {
            hit.collider.transform.GetComponent<BaseCard>().ActivateEffect(this);
        }
    }

    private void SelectField(RaycastHit hit)
    {
        ResetAllIndicators();
        fieldMark = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fieldMark.GetComponent<Renderer>().material.color = Color.cyan;
        fieldMark.transform.position = new Vector3(hit.point.x, hit.point.y, 0.9f);

        activeField = new Vector2(hit.point.x, hit.point.y);

        if (board[x, y] != null && board[x, y].isWhite == whiteTurn)
        {
            gameMode = Mode.readyToMove;

            List<Vector2> possibleMoves = new List<Vector2>();
            if (board[x, y].isWhite == whiteTurn)
                possibleMoves = board[x, y].GetPosibbleMoves(board);

            foreach (Vector2 move in possibleMoves)
            {
                GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
                indicator.transform.position = new Vector3(move.x, move.y, 0.9f);
                if (board[(int)move.x, (int)move.y] == null)
                    indicator.GetComponent<Renderer>().material.color = Color.yellow;
                else
                    indicator.GetComponent<Renderer>().material.color = Color.magenta;
                moveMarks.Add(indicator);
            }
        }
    }
    private void TryToMovePiece(int x, int y)
    {
        foreach (GameObject field in moveMarks)
        {
            if (field.transform.position.x == x && field.transform.position.y == y)
            {
                if (board[x, y] == null)
                {
                    // Move
                    print("Move");
                    MovePiece(board[(int)activeField.x, (int)activeField.y], x, y);
                }
                else
                {
                    // Attack
                    print("Attack");

                    Attack(board[(int)activeField.x, (int)activeField.y], x, y);
                }
                ResetAllIndicators();
                gameMode = Mode.idle;
                return;
            }
        }

        //Gracz zaznaczyl inne pole
        SelectField(x, y);
    }
    private void MovePiece(ChessPiece piece, int x, int y)
    {
        SwitchTurns();
        board[piece.currentX, piece.currentY] = null;
        board[x, y] = piece;

        piece.currentX = x;
        piece.currentY = y;

        StartCoroutine(MoveOnlyGraphical(piece, x, y));

        activeField = new Vector2(-1, -1);
    }
    private IEnumerator MoveOnlyGraphical(ChessPiece piece, int x, int y)
    {
        float xOffset = (x - piece.transform.position.x) / 10;
        float yOffset = (y - piece.transform.position.y) / 10;

        for (int i = 0; i < 10; i++)
        {
            piece.transform.position += new Vector3(xOffset, yOffset, 0);
            yield return new WaitForSeconds(0);
        }
    }
    private void Attack(ChessPiece piece, int x, int y)
    {
        bool ifDead = piece.DealDamageFromAttack(board[x, y]);
        //bool ifDead = false;
        if (ifDead)
        {
            MovePiece(piece, x, y);
        }
        else
        {
            Vector2 newPosition = piece.GetPositionAfterAttack(board, x, y);
            StartCoroutine(MoveWithAttack(piece, x, y, (int)newPosition.x, (int)newPosition.y));
        }
    }
    private IEnumerator MoveWithAttack(ChessPiece piece, int x, int y, int newX, int newY)
    {
        StartCoroutine(MoveOnlyGraphical(piece, x, y));
        yield return new WaitForSeconds(0.3f);
        MovePiece(piece, newX, newY);
    }
    private void SwitchTurns()
    {
        onNewTurn();
    }
    private void EndTurn()
    {
        whiteTurn = !whiteTurn;
        GetPlayer().maxMana++;
        GetPlayer().RefillManaPoints();
        if (whiteTurn)
            turn++;
        GetPlayer().OnTurnChange();
        StartCoroutine(RotateTurnIndicator());
    }
    public Player GetPlayer()
    {
        if (whiteTurn)
            return whitePlayer;
        else
            return blackPlayer;
    }
    public Player GetPlayer(bool whiteTurn)
    {
        if (whiteTurn)
            return whitePlayer;
        else
            return blackPlayer;
    }
    private IEnumerator RotateTurnIndicator()
    {
        for (int i = 0; i < 18; i++)
        {
            turnIndicator.transform.Rotate(0, 10, 0, Space.Self);
            yield return new WaitForSeconds(0);
        }
    }
    private void PickField(int x, int y)
    {
        onCardHit(x, y);
    }*/

}
