using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Soldier : Piece, IReceiver<Player>
{
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private int defaultMovimentPoints = 30;
    [SerializeField]
    private int defaultLifePoints = 50;
    [SerializeField]
    private int defaultAttackPower = 10;

    public string GetName() => "Soldier name";
    public string GetPointsLeft() => currentMovimentPoints.ToString();

    public Player currentPlayer { get; private set; } = Player.Null;

    private Player turnPlayer = Player.Null;
    private int currentMovimentPoints = 0;
    private int currentLifePoints = 0;

    public override int GetMovimentPoints()
    {
        return currentMovimentPoints;
    }

    public int GetAttackPower() => defaultAttackPower;
    public int GetLifePoints() => currentLifePoints;

    public override void ExecuteMoviment(int movimentCost)
    {
        currentMovimentPoints -= movimentCost;
    }

    public void TakeDamage(int damage)
    {
        currentLifePoints -= damage;
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
        currentLifePoints = defaultLifePoints;
    }

    public void ReceiveUpdate(Player updatedValue)
    {
        turnPlayer = updatedValue;
        currentMovimentPoints = defaultMovimentPoints;
    }
}
