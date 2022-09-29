using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("External references")]
    [SerializeField]
    private Tile tilePrefab = null;

    [Header("Custom board configurations")]
    [SerializeField]
    private int totalRows = 3;
    [SerializeField]
    private int totalColumns = 3;
    [SerializeField]
    private Vector2 pieceOffset = Vector2.one;

    private Tile[,] board;

    private void Start()
    {
        Vector2 baseSpawn = transform.localPosition;
        baseSpawn = SetBaseSpaw(baseSpawn, pieceOffset);

        CreateBoard(pieceOffset.x, pieceOffset.y, baseSpawn.x, baseSpawn.y);
    }

    private void CreateBoard(float offsetHorizontal, float offsetVertical, float startHorizontal, float startVertical)
    {
        board = new Tile[totalRows, totalColumns];

        for (int i = 0; i < totalRows; i++)
        {
            for (int j = 0; j < totalColumns; j++)
            {
                Tile tile = Instantiate(tilePrefab, transform);
                tile.gameObject.name = "Tile[ " + i + " ][ " + j + " ]";
                board[i, j] = tile;

                Vector3 position = new Vector3(GetOffsetPosition(offsetHorizontal, startHorizontal, j),
                                               0f,
                                               GetOffsetPosition(offsetVertical, startVertical, i));

                tile.gameObject.transform.localPosition = position;
            }
        }
    }

    private void ClearBoard()
    {
        for (int i = 0; i < totalRows; i++)
        {
            for (int j = 0; j < totalColumns; j++)
            {
                if (board[i, j] == null) continue;

                Destroy(board[i, j].gameObject);
            }
        }
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
}
