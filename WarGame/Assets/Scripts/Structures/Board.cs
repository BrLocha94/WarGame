using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour, IReceiver<GameState>, IReceiver<Player>
{
    public delegate void OnSoldierSelectedHandler(Soldier soldier);
    public delegate void OnTargetSelectedHandler(string info);

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
    [SerializeField]
    private BoardIteratorBase defaultMovimentIterator;
    [SerializeField]
    private BoardIteratorBase defaultAttackIterator;

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

    private IteratorResult iteratorResult = null;
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

        iteratorResult = null;

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
            if (iteratorResult != null && iteratorResult.canExecute) {

                //Move
                if (targetTile.currentPiece == null)
                {
                    if (iteratorResult != null && iteratorResult.canExecute)

                    StateMachineController.ExecuteTransition(GameState.Moving);
                    StartCoroutine(MovimentRoutine(iteratorResult));

                    return;
                }
                //Attack
                else
                {
                    StateMachineController.ExecuteTransition(GameState.Attacking);
                    StartCoroutine(AttackRoutine(iteratorResult));

                    return;
                }
            }

            return;
        }

        // Change target
        if (targetTile != null)
        {
            if (iteratorResult != null)
            {
                if (iteratorResult.canExecute)
                {
                    foreach (Tile tile in iteratorResult.tiles)
                    {
                        tile.UnlitTile();
                    }
                }
                else
                    targetTile.UnlitTile();
            }
            else
                targetTile.UnlitTile();
        }

        targetTile = target;
        onTargetSelectedEvent?.Invoke(null);

        iteratorResult = null;

        if (targetTile != null)
        {
            // Moviment
            if (targetTile.currentPiece == null)
            {
                StateMachineController.ExecuteTransition(GameState.Processing);
                StartCoroutine(defaultMovimentIterator.Iteract(board, selectedTile, targetTile, OnMovimentIteratorCallback));
            }
            // Attack
            else
            {
                StateMachineController.ExecuteTransition(GameState.Processing);
                StartCoroutine(defaultMovimentIterator.Iteract(board, selectedTile, targetTile, OnAttackIteratorCallback));
            }
        }
        else
            iteratorResult = null;
    }

    private void OnMovimentIteratorCallback(IteratorResult result)
    {
        iteratorResult = result;

        if (iteratorResult.canExecute)
        {
            foreach (Tile tile in iteratorResult.tiles)
            {
                tile.SelectTile();
            }

            onTargetSelectedEvent?.Invoke("Can move to tile [" + targetTile.row + "][" + targetTile.column + "] using " +
                result.excecutionCost + " points");
        }
        else
        {
            targetTile.UnselectTile();
            onTargetSelectedEvent?.Invoke("Cant move to this tile");
        }

        StateMachineController.ExecuteTransition(GameState.SelectedSoldier);
    }

    private void OnAttackIteratorCallback(IteratorResult result)
    {
        iteratorResult = result;

        if (iteratorResult.canExecute)
        {
            // Attack action
            targetTile.SelectTile();

            onTargetSelectedEvent?.Invoke("Can atack soldier on tile [" + targetTile.row + "][" + targetTile.column + "] using " +
                result.excecutionCost + " points with " + result.acuracy + " acuracy");
        }
        else
        {
            // Moviment action
            targetTile.UnselectTile();
            onTargetSelectedEvent?.Invoke("Cant attack this soldier");
        }

        StateMachineController.ExecuteTransition(GameState.SelectedSoldier);
    }

    private IEnumerator MovimentRoutine(IteratorResult result)
    {
        bool wait = true;

        Piece piece = null;
        Tile lastTile = selectedTile;
        Tile tile;

        Debug.Log(iteratorResult.tiles);

        for(int i = 0; i < iteratorResult.tiles.Count; i++)
        {
            Debug.Log(iteratorResult.tiles[i].name);
            tile = iteratorResult.tiles[i];
            wait = true;
            piece = lastTile.PopPiece();
            tile.SetPiece(piece, true, () => wait = false);

            yield return new WaitWhile(() => wait);

            lastTile = tile;
        }

        if(piece != null)
            piece.ExecuteMoviment(result.excecutionCost);

        StateMachineController.ExecuteTransition(GameState.Ready);
    }

    private IEnumerator AttackRoutine(IteratorResult result)
    {
        Soldier selectedSoldier = (Soldier)selectedTile.currentPiece;
        Soldier targetSoldier = (Soldier) targetTile.currentPiece;

        int random = UnityEngine.Random.Range(0, 100);
        if(iteratorResult.acuracy >= random)
        {
            targetSoldier.TakeDamage(selectedSoldier.GetAttackPower());

            //Killed enemy soldier
            if(targetSoldier.GetLifePoints() <= 0)
            {
                Piece piece = targetTile.PopPiece();

                if (currentPlayer == Player.Player01)
                    player02SoldierList.Remove(targetSoldier);
                else
                    player01SoldierList.Remove(targetSoldier);

                piece.Clear();
            }
        }

        yield return new WaitForSeconds(0.5f);

        StateMachineController.ExecuteTransition(GameState.CheckGameOver);
    }

    public void ReceiveUpdate(GameState updatedValue)
    {
        canCheckMouseInput = false;

        if (updatedValue == GameState.Initializing)
        {
            CreateBoard();
            return;
        }

        if(updatedValue == GameState.CheckGameOver)
        {
            if (player01SoldierList.Count == 0 || player02SoldierList.Count == 0)
                this.InvokeAfterFrame(() => StateMachineController.ExecuteTransition(GameState.GameOver));
            else
                this.InvokeAfterFrame(() => StateMachineController.ExecuteTransition(GameState.Ready));

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
            iteratorResult = null;

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
