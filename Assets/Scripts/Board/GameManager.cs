using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode { blocked, idle, readyToMove, pickField }
public class GameManager : MonoBehaviour
{

    public Player whitePlayer, blackPlayer;
    public GameObject turnIndicator;
    public GameObject blur;

    public Field[,] board;

    public GameObject cardPrefabsObject;
    public List<GameObject> cardPrefabs = new List<GameObject>();

    public Mode gameMode = Mode.blocked;

    public bool whiteTurn;

    public int turn = 0;


    // Delegaty
    public delegate void VoidAndRaycastHit(RaycastHit hit);
    public VoidAndRaycastHit onMouseMove;
    public VoidAndRaycastHit onMouseClick;
    public VoidAndRaycastHit onBoardMove;
    public VoidAndRaycastHit onBoardHit;
    public VoidAndRaycastHit onCardMove;
    public VoidAndRaycastHit onCardHit;
    public delegate void VoidAndVoid();
    public VoidAndVoid onNewTurn;
    public VoidAndVoid onVoidMove;
    public VoidAndVoid onVoidHit;


    // Wskazniki pol
    private Field _activeField;
    public Field activeField
    {
        get
        {
            return _activeField;
        }
        set
        {
            ResetIndicators();
            _activeField = value;
        }
    }
    GameObject selectedFieldMark;
    List<GameObject> possibleMovesMarks = new List<GameObject>();


    // Pola potrzene do obrotu planszy
    private GameObject boardRotationPoint;
    private List<ChessPiece> allChessPieces = new List<ChessPiece>();
    private GameObject cameraRotationPoint;
    private float sleepValue = 0;

    private BaseCard enlargedCard;

    public void Start()
    {

        cameraRotationPoint = GameObject.Find("Camera Rotation Point").gameObject;
        PrepareCards();
        PrepareBoard();
        PreparePlayers();
        onNewTurn += EndTurn;
        SwitchTurns();
    }

    public void Update()
    {
        GetRaycastHitOnClick();
    }

    private void PrepareCards()
    {
        for (int i = 0; i < cardPrefabsObject.transform.childCount; i++)
        {
            cardPrefabs.Add(cardPrefabsObject.transform.GetChild(i).gameObject);
        }
    }
    private void PrepareBoard()
    {
        blur.GetComponent<SpriteRenderer>().enabled = true;
        List<Field> fields = GetAllFields();
        List<ChessPiece> pieces = GetAllPieces();

        int boardSize = (int)Mathf.Sqrt(fields.Count);
        board = new Field[boardSize, boardSize];

        foreach (Field field in fields)
        {
            foreach (ChessPiece piece in pieces)
            {
                if (piece.transform.position.x == field.transform.position.x && piece.transform.position.y == field.transform.position.y)
                {
                    field.piece = piece;
                    break;
                }
            }

            board[(int)field.transform.position.x, (int)field.transform.position.y] = field;
        }
    }
    private List<Field> GetAllFields()
    {
        boardRotationPoint = GameObject.Find("Board Rotation Point");
        GameObject allFields = boardRotationPoint.transform.Find("Board").gameObject;
        List<Field> fields = new List<Field>();

        for (int i = 0; i < allFields.transform.childCount; i++)
        {
            fields.Add(allFields.transform.GetChild(i).GetComponent<Field>());
        }

        print("ILOSC POL: " + fields.Count);

        return fields;
    }
    private List<ChessPiece> GetAllPieces()
    {
        GameObject pieces = boardRotationPoint.transform.Find("Pieces").gameObject;

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

        List<ChessPiece> whitePieces = new List<ChessPiece>();
        List<ChessPiece> blackPieces = new List<ChessPiece>();

        // Podzial typow figur na pojedyncze figury
        // Biale
        for (int type = 0; type < whitePiecesByType.Length; type++)
        {
            for (int pawn = 0; pawn < whitePiecesByType[type].childCount; pawn++)
            {
                whitePieces.Add(whitePiecesByType[type].GetChild(pawn).GetComponent<ChessPiece>());
            }
        }
        // Czarne
        for (int type = 0; type < blackPiecesByType.Length; type++)
        {
            for (int pawn = 0; pawn < blackPiecesByType[type].childCount; pawn++)
            {
                blackPieces.Add(blackPiecesByType[type].GetChild(pawn).GetComponent<ChessPiece>());
            }
        }

        List<ChessPiece> listOfAllPawns = whitePieces;
        listOfAllPawns.AddRange(blackPieces);

        allChessPieces = listOfAllPawns;

        print("ILOSC FIGUR: " + allChessPieces.Count);

        return listOfAllPawns;
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

    public void GetRaycastHitOnClick()
    {
        RaycastHit hit = GetRaycastHit();
        try
        {
            if (hit.collider.transform.GetComponent<Field>() != null)
            {
                // Trafiono w pole
                onMouseMove = OnFieldMove;
                onMouseClick = OnFieldClick;
            }
            else if (hit.collider.transform.GetComponent<BaseCard>() != null)
            {
                // Trafiono w karte
                onMouseMove = OnCardMove;
                onMouseClick = OnCardClick;
            }
        }
        catch (System.NullReferenceException)
        {
            // Trafiono w pustke
            onMouseMove = OnVoidMove;
            onMouseClick = OnVoidClick;
        }


        onMouseMove(hit);
        if (Input.GetMouseButtonDown(0) && gameMode != Mode.blocked)
            onMouseClick(hit);
    }
    public RaycastHit GetRaycastHit()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, LayerMask.GetMask("Raycastable"));
        return hit;
    }
    private void OnFieldMove(RaycastHit hit)
    {
        // Najechano mysza na plansze

        //print("Board");

        try
        {
            onBoardMove(hit);
        }
        catch (System.NullReferenceException) { }
    }
    private void OnFieldClick(RaycastHit hit)
    {
        // Kliknieto na pole

        //print("Board CLICKED");

        //onBoardHit = null;

        if (gameMode == Mode.blocked)
        {
            return;
        }
        else if (gameMode == Mode.idle)
        {
            onBoardHit = SelectField;

        }
        else if (gameMode == Mode.readyToMove)
        {
            onBoardHit = TryToMovePiece;
        }
        else if (gameMode == Mode.pickField)
        {
            //onBoardHit = PickField;
        }

        onBoardHit(hit);
    }
    private void OnCardMove(RaycastHit hit)
    {
        // Najechano mysza na karte

        //print("Card");
        try
        {
            onCardMove(hit);
        }
        catch (System.NullReferenceException) { }

    }
    private void OnCardClick(RaycastHit hit)
    {
        // Kliknieto na karte

        //print("Card CLICKED");

        try
        {
            onCardHit(hit);
        }
        catch (System.NullReferenceException) { }

        hit.collider.transform.GetComponent<BaseCard>().ActivateEffect(this);
    }
    private void OnVoidClick(RaycastHit hit)
    {
        // Kliknieto w pustke
        // TODO - Zresetowac wskazniki

        ResetIndicators();

        try
        {
            onVoidHit();
        }
        catch (System.NullReferenceException) { }

        //print("Void");

    }
    private void OnVoidMove(RaycastHit hit)
    {
        try
        {
            onVoidMove();
        }
        catch (System.NullReferenceException) { }
    }

    private void SelectField(RaycastHit hit)
    {
        ResetIndicators();

        activeField = hit.collider.transform.GetComponent<Field>();
        selectedFieldMark = CreateCubeIndicator(hit.collider.transform.position.x, hit.collider.transform.position.y, Color.cyan);

        gameMode = Mode.readyToMove;

        List<Vector2> possibleMoves = new List<Vector2>();
        if (board[(int)hit.collider.transform.position.x, (int)hit.collider.transform.position.y].piece != null
            && board[(int)hit.collider.transform.position.x, (int)hit.collider.transform.position.y].piece.isWhite == whiteTurn)
        {
            possibleMoves = board[(int)hit.collider.transform.position.x, (int)hit.collider.transform.position.y].piece.GetPossibleMoves(board);
        }

        foreach (Vector2 move in possibleMoves)
        {
            if (board[(int)move.x, (int)move.y].piece == null)
                possibleMovesMarks.Add(CreateCubeIndicator(move.x, move.y, Color.yellow));
            else
                possibleMovesMarks.Add(CreateCubeIndicator(move.x, move.y, Color.red));
        }

    }
    private void TryToMovePiece(RaycastHit hit)
    {
        foreach (GameObject field in possibleMovesMarks)
        {
            if (field.transform.position.x == hit.collider.transform.position.x && field.transform.position.y == hit.collider.transform.position.y)
            {
                if (hit.collider.transform.GetComponent<Field>().piece == null)
                {
                    // Move
                    MovePiece(activeField, hit.collider.transform.GetComponent<Field>());
                }
                else
                {
                    // Attack
                    Attack(activeField, hit.collider.transform.GetComponent<Field>());
                }
                ResetIndicators();
                gameMode = Mode.idle;
                SwitchTurns();
                return;
            }
        }

        //Gracz zaznaczyl inne pole
        SelectField(hit);
    }
    private void MovePiece(Field start, Field destination)
    {
        if (start != destination)
        {
            destination.piece = start.piece;
            start.piece = null;
        }

        destination.piece.currentX = (int)destination.transform.position.x;
        destination.piece.currentY = (int)destination.transform.position.y;

        StartCoroutine(MoveOnlyGraphical(destination.piece, (int)destination.transform.position.x, (int)destination.transform.position.y));

        activeField = null;
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
    private void Attack(Field start, Field destination)
    {
        bool ifDead = start.piece.DealDamageFromAttack(destination.piece);
        if (ifDead)
        {
            sleepValue = 0.4f;
            MovePiece(start, destination);
        }
        else
        {
            sleepValue = 0.65f;
            Field newPosition = start.piece.GetPositionAfterAttack(board, destination);
            StartCoroutine(MoveWithAttack(start, destination, newPosition));
        }
    }
    private IEnumerator MoveWithAttack(Field start, Field destination, Field newPosition)
    {
        CameraShake.power += (float)start.piece.attack / (float)150;
        print("Camera shook, " + CameraShake.power + " " + start.piece.attack);
        CameraShake.shakeEnabled = true;

        StartCoroutine(MoveOnlyGraphical(start.piece, (int)destination.transform.position.x, (int)destination.transform.position.y));
        yield return new WaitForSeconds(0.3f);
        MovePiece(start, newPosition);

        CameraShake.shakeEnabled = false;
        CameraShake.power -= start.piece.attack / 100;
    }

    public void PickField(RaycastHit hit)
    {
        onCardHit(hit);
    }

    private void ResetIndicators()
    {
        try
        {
            Destroy(selectedFieldMark);
            selectedFieldMark = null;
        }
        catch (System.NullReferenceException) { }

        foreach (GameObject indicator in possibleMovesMarks)
        {
            Destroy(indicator);
        }

        possibleMovesMarks.Clear();
        _activeField = null;
    }
    public GameObject CreateCubeIndicator(float x, float y, Color color)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.GetComponent<Renderer>().material.color = color;
        obj.transform.position = new Vector3(x, y, 0.9f);

        return obj;
    }
    public GameObject CreateSphereIndicator(float x, float y, float z, Color color)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.GetComponent<Renderer>().material.color = color;
        obj.transform.position = new Vector3(x, y, z);

        return obj;
    }

    public Player GetPlayer()
    {
        return GetPlayer(whiteTurn);
    }
    public Player GetPlayer(bool isWhite)
    {
        if (isWhite)
            return whitePlayer;
        else
            return blackPlayer;
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
        // StartCoroutine(RotateTurnIndicator());
    }
    private IEnumerator RotateTurnIndicator()
    {
        yield return new WaitForSeconds(sleepValue);
        for (int i = 0; i < 18; i++)
        {
            turnIndicator.transform.Rotate(0, 10, 0, Space.Self);
            cameraRotationPoint.transform.Rotate(0, 0, 10, Space.Self);
            foreach (ChessPiece piece in allChessPieces)
            {
                try
                {
                    piece.transform.Rotate(0, 0, 10, Space.Self);
                }
                catch (MissingReferenceException) { }
                catch (System.Exception) { }
            }
            yield return new WaitForSeconds(0);
        }
    }


}