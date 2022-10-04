using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour, IReceiver<GameState>, IReceiver<Player>
{
    public delegate void OnSoldierSelectedHandler(Soldier soldier);
    public delegate void OnTargetSelectedHandler(Tile tile);

    public event OnSoldierSelectedHandler onSoldierSelectedEvent;
    public event OnTargetSelectedHandler onTargetSelectedEvent;
    public Action onBoardCreatedEvent;

    [Header("External references")]
    [SerializeField]
    private Tile tilePrefab = null;
    [SerializeField]
    private Soldier soldierPrefab = null;
    [SerializeField]
    private Obstacle obstaclePrefab = null;

    [Header("Custom board default configurations")]
    [SerializeField]
    private int defaultSoldiersPerPlayer = 6;
    [SerializeField]
    private int defaultTotalRows = 3;
    [SerializeField]
    private int defaultTotalColumns = 3;
    [SerializeField]
    private Vector2 pieceOffset = Vector2.one;

    [Header("Material options for players")]
    [SerializeField]
    private Material player01Material;
    [SerializeField]
    private Material player02Material;

    private int currentSoldiersPerPlayer = 0;
    private int currentTotalRows = 0;
    private int currentTotalColumns = 0;

    private Tile[,] board;
    private List<Soldier> player01SoldierList;
    private List<Soldier> player02SoldierList;

    private Tile selectedTile = null;
    private Tile targetTile = null;

    private bool canCheckMouseInput = false;

    private Player currentPlayer;

    private void CreateBoard()
    {
        if (currentSoldiersPerPlayer <= 0)
            currentSoldiersPerPlayer = defaultSoldiersPerPlayer;

        if (currentTotalRows <= 0)
            currentTotalRows = defaultTotalRows;

        if (currentTotalColumns <= 0)
            currentTotalColumns = defaultTotalColumns;

        Vector2 baseSpawn = transform.localPosition;
        baseSpawn = SetBaseSpaw(baseSpawn, pieceOffset);

        StartCoroutine(CreateBoardRoutine(pieceOffset.x, pieceOffset.y, baseSpawn.x, baseSpawn.y));
    }

    private IEnumerator CreateBoardRoutine(float offsetHorizontal, float offsetVertical, float startHorizontal, float startVertical)
    {
        board = new Tile[currentTotalRows, currentTotalColumns];

        // Set all tiles on board
        for (int i = 0; i < currentTotalRows; i++)
        {
            for (int j = 0; j < currentTotalColumns; j++)
            {
                Tile tile = Instantiate(tilePrefab, transform);
                board[i, j] = tile;

                Vector3 position = new Vector3(GetOffsetPosition(offsetHorizontal, startHorizontal, j),
                                               0f,
                                               GetOffsetPosition(offsetVertical, startVertical, i));

                tile.Initialize(i, j, transform, position);
                tile.onTargetClickedEvent += OnTileClicked;
            }

            yield return null;
        }

        // Instantiate all soldiers on Board

        player01SoldierList = new List<Soldier>();
        player02SoldierList = new List<Soldier>();

        for (int i = 0; i < currentSoldiersPerPlayer; i++)
        {
            Soldier soldier01 = Instantiate(soldierPrefab);
            soldier01.Initialize(Player.Player01, player01Material);

            Soldier soldier02 = Instantiate(soldierPrefab);
            soldier02.Initialize(Player.Player02, player02Material);

            player01SoldierList.Add(soldier01);
            player02SoldierList.Add(soldier02);
        }

        yield return null;

        // Set Soldiers on position

        int initialPoint = (currentTotalColumns / 2) - (currentSoldiersPerPlayer / 2);
        int finalPoint = initialPoint + currentSoldiersPerPlayer;

        for (int i = initialPoint; i < finalPoint; i++)
        {
            board[i, 0].SetPiece(player01SoldierList[i - initialPoint]);
            board[i, currentTotalColumns - 1].SetPiece(player02SoldierList[i - initialPoint]);
        }

        yield return null;

        // Set all obstacles on Board

        int random = 0;
        for(int i = 0; i < currentTotalRows; i++)
        {
            for(int j = 0; j < currentTotalColumns; j++)
            {
                if (board[i, j].currentPiece != null) continue;

                random = UnityEngine.Random.Range(0, 100);

                if (random > 5) continue;

                Obstacle obstacle = Instantiate(obstaclePrefab);
                board[i, j].SetPiece(obstacle);
            }

            yield return null;
        }

        yield return null;

        onBoardCreatedEvent?.Invoke();
    }

    private void ClearBoard()
    {
        if (board == null) return;

        for (int i = 0; i < currentTotalRows; i++)
        {
            for (int j = 0; j < currentTotalColumns; j++)
            {
                if (board[i, j] == null) continue;

                board[i, j].onTargetClickedEvent -= OnTileClicked;
                board[i, j].Clear();
            }
        }

        player01SoldierList.Clear();
        player02SoldierList.Clear();

        selectedTile = null;
        onSoldierSelectedEvent?.Invoke(null);

        targetTile = null;
        onTargetSelectedEvent?.Invoke(null);

        board = null;
    }

    private float GetOffsetPosition(float offset, float start, int factor)
    {
        return start + (offset * factor);
    }

    private Vector2 SetBaseSpaw(Vector2 baseSpaw, Vector2 offset)
    {
        if (currentTotalColumns > 1)
        {
            float baseX = baseSpaw.x;
            baseX = baseX + (-1 * (offset.x * currentTotalColumns) / 2) + (offset.x / 2);
            baseSpaw.x = baseX;
        }

        if (currentTotalRows > 1)
        {
            float baseY = baseSpaw.y;
            baseY = baseY + (-1 * (offset.y * currentTotalRows) / 2) + (offset.y / 2);
            baseSpaw.y = baseY;
        }

        return baseSpaw;
    }

    public void SetCustomBoardConfig(int rows, int columns, int soldiersPerPlayer)
    {
        currentTotalRows = rows;
        currentTotalColumns = columns;
        currentSoldiersPerPlayer = soldiersPerPlayer;
    }

    private void Update()
    {
        if (!canCheckMouseInput) return;

        if (Input.GetMouseButtonDown(1))
            StateMachineController.ExecuteTransition(GameState.Ready);
    }

    private void OnTileClicked(Tile target)
    {
        Debug.Log("Received callback from tile ");

        if(selectedTile == null)
        {
            selectedTile = target;
            selectedTile.LitTile();
            onSoldierSelectedEvent?.Invoke((Soldier)target.currentPiece);

            StateMachineController.ExecuteTransition(GameState.SelectedSoldier);

            return;
        }

        // Execute Action
        if(targetTile == target)
        {
            //Move

            //Attack

            Debug.Log("Executing Action");

            return;
        }

        // Change target
        if (targetTile != null)
            targetTile.UnlitTile();

        targetTile = target;
        onTargetSelectedEvent?.Invoke(targetTile);
    }

    public void ReceiveUpdate(GameState updatedValue)
    {
        canCheckMouseInput = false;

        if (updatedValue == GameState.Initializing)
        {
            CreateBoard();
            return;
        }

        if(updatedValue == GameState.Ready || 
           updatedValue == GameState.NextPlayer ||
           updatedValue == GameState.GameOver)
        {
            if (selectedTile != null)
                selectedTile.UnlitTile();

            selectedTile = null;
            onSoldierSelectedEvent?.Invoke(null);

            if (targetTile != null)
                targetTile.UnlitTile();

            targetTile = null;
            onTargetSelectedEvent?.Invoke(null);
            return;
        }

        if(updatedValue == GameState.SelectedSoldier)
        {
            canCheckMouseInput = true;
            return;
        }
    }

    public void ReceiveUpdate(Player updatedValue)
    {
        currentPlayer = updatedValue;
    }
}
