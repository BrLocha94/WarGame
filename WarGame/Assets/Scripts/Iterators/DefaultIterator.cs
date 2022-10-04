using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultIterator", menuName = "GameAssets/Iterators/Default")]
public sealed class DefaultIterator : BoardIteratorBase
{
    [SerializeField]
    private int movimentCost = 7;

    int totalMovimentCost = 0;

    public override IEnumerator Iteract(Tile[,] board, Tile initialTile, Tile targetTile, IteratorEventHandler callback = null)
    {
        yield return base.Iteract(board, initialTile, targetTile, callback);

        totalMovimentCost = 0;

        //Get initial tile soldier moviment points
        int points = initialTile.GetMovimentPoints();

        List<Tile> tiles = new List<Tile>();

        tiles = CheckMoviment(board, initialTile.row, initialTile.column, targetTile, tiles, points);

        callback?.Invoke(new IteratorResult() { tiles = tiles });
    }

    private List<Tile> CheckMoviment(Tile[,] board, int row, int column, Tile targetTile, List<Tile> list, int currentPoints)
    {
        currentPoints -= movimentCost;

        if (currentPoints < 0) return null;

        if (!IsOnBounds(row, column)) return null;

        return null;
    }
}
