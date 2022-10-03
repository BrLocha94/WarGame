using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    [SerializeField]
    private PieceType pieceType;

    public PieceType PieceType => pieceType;

    public virtual bool CanIteract() => pieceType == PieceType.Soldier;

    public void SetParent(Transform parent, bool animate = false, Action callback = null)
    {
        transform.parent = parent;
        transform.position = new Vector3(parent.position.x, transform.position.y, parent.position.z);
    }

    public void Clear()
    {
        Destroy(gameObject);
    }
}

public enum PieceType
{
    Null,
    Soldier,
    Obstacle
}
