using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackIterator", menuName = "GameAssets/Iterators/Attack")]
public class AttackIterator : BoardIteratorBase
{
    [SerializeField]
    private int attackCost = 13;

    int points = 0;

    public override IEnumerator Iteract(Tile[,] board, Tile initialTile, Tile targetTile, IteratorEventHandler callback = null)
    {
        yield return base.Iteract(board, initialTile, targetTile, callback);

        //Get initial tile soldier moviment points
        points = initialTile.GetMovimentPoints();

        int distanceX = (initialTile.row > targetTile.row) ? initialTile.row - targetTile.row : targetTile.row - initialTile.row;
        int distanceY = (initialTile.column > targetTile.column) ? initialTile.column - targetTile.column : targetTile.column - initialTile.column;

        int totalDistance = distanceX + distanceY;

        float acuracy = 0f;

        if (totalDistance <= 1)
            acuracy = 100f;
        else if (totalDistance <= 2)
            acuracy = 85f;
        else if (totalDistance <= 4)
            acuracy = 50f;
        else
            acuracy = 0f;

        callback?.Invoke(new IteratorResult()
        {
            canExecute = acuracy > 0,
            excecutionCost = attackCost,
            acuracy = acuracy
        });
    }
}
