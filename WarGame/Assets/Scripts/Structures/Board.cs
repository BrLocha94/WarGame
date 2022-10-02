using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour, IReceiver<GameState>
{
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
    private int soldiersPerPlayer = 6;
    [SerializeField]
    private int totalRows = 3;
    [SerializeField]
    private int totalColumns = 3;
    [SerializeField]
    private Vector2 pieceOffset = Vector2.one;

    [Header("Material options for players")]
    [SerializeField]
    private Material player01Material;
    [SerializeField]
    private Material player02Material;

    private Tile[,] board;
    private List<Soldier> player01SoldierList;
    private List<Soldier> player02SoldierList;

    private void CreateBoard()
    {
        Vector2 baseSpawn = transform.localPosition;
        baseSpawn = SetBaseSpaw(baseSpawn, pieceOffset);

        StartCoroutine(CreateBoardRoutine(pieceOffset.x, pieceOffset.y, baseSpawn.x, baseSpawn.y));
    }

    private IEnumerator CreateBoardRoutine(float offsetHorizontal, float offsetVertical, float startHorizontal, float startVertical)
    {
        board = new Tile[totalRows, totalColumns];

        // Set all tiles on board
        for (int i = 0; i < totalRows; i++)
        {
            for (int j = 0; j < totalColumns; j++)
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

        //TODO: Set all soldiers on Board

        player01SoldierList = new List<Soldier>();
        player02SoldierList = new List<Soldier>();

        for (int i = 0; i < soldiersPerPlayer; i++)
        {
            Soldier soldier01 = Instantiate(soldierPrefab);
            soldier01.Initialize(Player.Player01, player01Material);

            Soldier soldier02 = Instantiate(soldierPrefab);
            soldier02.Initialize(Player.Player02, player02Material);

            player01SoldierList.Add(soldier01);
            player02SoldierList.Add(soldier02);
        }

        yield return null;

        int initialPoint = (totalColumns / 2) - (soldiersPerPlayer / 2);
        int finalPoint = initialPoint + soldiersPerPlayer;

        for (int i = initialPoint; i < finalPoint; i++)
        {
            board[i, 0].SetPiece(player01SoldierList[i - initialPoint]);
            board[i, totalColumns - 1].SetPiece(player02SoldierList[i - initialPoint]);
        }

        yield return null;

        //TODO: Set all obstacles on Board

        yield return null;

        onBoardCreatedEvent?.Invoke();
    }

    private void ClearBoard()
    {
        if (board == null) return;

        for (int i = 0; i < totalRows; i++)
        {
            for (int j = 0; j < totalColumns; j++)
            {
                if (board[i, j] == null) continue;

                board[i, j].onTargetClickedEvent -= OnTileClicked;
                board[i, j].Clear();
            }
        }

        player01SoldierList.Clear();
        player02SoldierList.Clear();

        board = null;
    }

    private float GetOffsetPosition(float offset, float start, int factor)
    {
        return start + (offset * factor);
    }

    private Vector2 SetBaseSpaw(Vector2 baseSpaw, Vector2 offset)
    {
        if (totalColumns > 1)
        {
            float baseX = baseSpaw.x;
            baseX = baseX + (-1 * (offset.x * totalColumns) / 2) + (offset.x / 2);
            baseSpaw.x = baseX;
        }

        if (totalRows > 1)
        {
            float baseY = baseSpaw.y;
            baseY = baseY + (-1 * (offset.y * totalRows) / 2) + (offset.y / 2);
            baseSpaw.y = baseY;
        }

        return baseSpaw;
    }

    private void OnTileClicked(Tile target)
    {
        Debug.Log("Received callback from tile ");
    }

    public void ReceiveUpdate(GameState updatedValue)
    {
        if (updatedValue == GameState.Initializing)
            CreateBoard();
    }
}
