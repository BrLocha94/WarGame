using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Soldier : Piece
{
    [SerializeField]
    private MeshRenderer meshRenderer;

    public Player currentPlayer { get; private set; }

    public void Initialize(Player player, Material material)
    {
        currentPlayer = player;
        meshRenderer.material = material;
    }

}
