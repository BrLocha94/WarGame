using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Soldier : Piece, IReceiver<Player>
{
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private int defaultMovimentPoints = 30;

    public string GetName() => "Soldier name";
    public string GetPointsLeft() => "30";

    public Player currentPlayer { get; private set; } = Player.Null;

    private Player turnPlayer = Player.Null;
    private int currentMovimentPoints = 0;

    public override int GetMovimentPoints()
    {
        return base.GetMovimentPoints();
    }

    public override bool CanIteract()
    {
        return turnPlayer == currentPlayer;
    }

    public void Initialize(Player player, Material material)
    {
        currentPlayer = player;
        currentMovimentPoints = defaultMovimentPoints;
        meshRenderer.material = material;
    }

    public void ReceiveUpdate(Player updatedValue)
    {
        turnPlayer = updatedValue;
        currentMovimentPoints = defaultMovimentPoints;
    }
}
