using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultIterator", menuName = "GameAssets/Iterators/Default")]
public sealed class DefaultIterator : BoardIteratorBase
{
    [SerializeField]
    private int movimentCost = 5;

    int totalMovimentCost = 0;
    int points = 0;

    public override IEnumerator Iteract(Tile[,] board, Tile initialTile, Tile targetTile, IteratorEventHandler callback = null)
    {
        yield return base.Iteract(board, initialTile, targetTile, callback);

        totalMovimentCost = 0;
        points = 0;

        //Get initial tile soldier moviment points
        points = initialTile.GetMovimentPoints();

        List<Tile> tiles = new List<Tile>();

        bool check = CheckMoviment(board, initialTile.row, initialTile.column, initialTile, targetTile, tiles, points);

        callback?.Invoke(new IteratorResult() {
            tiles = tiles,
            excecutionCost = totalMovimentCost,
            canExecute = check
        });
    }

    private bool CheckMoviment(Tile[,] board, int row, int column, Tile initialTile, Tile targetTile, List<Tile> list, int currentPoints, bool initial = true)
    {
        if (initial)
        {
            //UP
            if (CheckMoviment(board, row + 1, column, initialTile, targetTile, list, currentPoints, false)) return true;
            //DOWN
            if (CheckMoviment(board, row - 1, column, initialTile, targetTile, list, currentPoints, false)) return true;
            //RIGHT
            if (CheckMoviment(board, row, column + 1, initialTile, targetTile, list, currentPoints, false)) return true;
            //LEFT
            if (CheckMoviment(board, row, column - 1, initialTile, targetTile, list, currentPoints, false)) return true;

            return false;
        }

        //Check bounds
        if (!IsOnBounds(row, column)) return false;

        //Check moviment
        currentPoints -= movimentCost;
        if (currentPoints < 0) return false;

        // Avoid same tile processing
        if (list.Contains(board[row, column]))
            return false;

        if (board[row, column] != initialTile)
        {
            //Check obstacle and soldiers
            if (board[row, column].currentPiece != null)
            {
                if (board[row, column] == targetTile)
                {
                    list.Add(board[row, column]);
                    totalMovimentCost = points - currentPoints;
                    return true;
                }

                return false;
            }

            list.Add(board[row, column]);

            if (board[row, column] == targetTile)
            {
                totalMovimentCost = points - currentPoints;
                return true;
            }
        }
        else
            return false;

        //UP
        if (CheckMoviment(board, row + 1, column, initialTile, targetTile, list, currentPoints, false)) return true;
        //DOWN
        if (CheckMoviment(board, row - 1, column, initialTile, targetTile, list, currentPoints, false)) return true;
        //RIGHT
        if (CheckMoviment(board, row, column + 1, initialTile, targetTile, list, currentPoints, false)) return true;
        //LEFT
        if (CheckMoviment(board, row, column - 1, initialTile, targetTile, list, currentPoints, false)) return true;

        list.Remove(board[row, column]);
        return false;
    }
}
