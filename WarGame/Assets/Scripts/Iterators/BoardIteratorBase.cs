using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardIteratorBase : ScriptableObject
{
    protected int totalRows = 0;
    protected int totalColumns = 0;

    public virtual IEnumerator Iteract(Tile[,] board, Tile initialTile, Tile targetTile, IteratorEventHandler callback = null)
    {
        totalRows = board.GetLength(0);
        totalColumns = board.GetLength(1);

        yield return null;
    }

    protected bool IsOnBounds(int row, int column)
    {
        if (row < 0 || row >= totalRows) return false;

        if (column < 0 || column >= totalColumns) return false;

        return true;
    }
}

public delegate void IteratorEventHandler(IteratorResult result);

public class IteratorResult
{
    public List<Tile> tiles = new List<Tile>();
    public int movimentCost = 0;
}
