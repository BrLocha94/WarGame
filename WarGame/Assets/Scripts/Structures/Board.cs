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

    [Header("Custom board default configurations")]
    [SerializeField]
    private int totalRows = 3;
    [SerializeField]
    private int totalColumns = 3;
    [SerializeField]
    private Vector2 pieceOffset = Vector2.one;

    private Tile[,] board;

    private void CreateBoard()
    {
        Vector2 baseSpawn = transform.localPosition;
        baseSpawn = SetBaseSpaw(baseSpawn, pieceOffset);

        StartCoroutine(CreateBoardRoutine(pieceOffset.x, pieceOffset.y, baseSpawn.x, baseSpawn.y));
    }

    private IEnumerator CreateBoardRoutine(float offsetHorizontal, float offsetVertical, float startHorizontal, float startVertical)
    {
        board = new Tile[totalRows, totalColumns];

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
                Destroy(board[i, j].gameObject);
            }
        }

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
