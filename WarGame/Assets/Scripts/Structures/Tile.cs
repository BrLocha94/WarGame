using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : ClickableObjectBase<Tile>, IReceiver<GameState>
{
    [SerializeField]
    private Plane plane;

    public int row { get; private set; } = -1;
    public int column { get; private set; } = -1;

    public Piece currentPiece { get; private set; } = null;

    private bool canCheck = false;

    public void Initialize(int row, int column, Transform parent, Vector3 localPosition)
    {
        this.row = row;
        this.column = column;

        name = "Tile[ " + row + " ][ " + column + " ]";

        transform.parent = parent;
        transform.localPosition = localPosition;
    }

    public void SetPiece(Piece piece, bool animate = false, Action callback = null)
    {
        if(currentPiece == null)
        {
            currentPiece = piece;
            currentPiece.SetParent(transform);
        }
    }

    public int GetMovimentPoints()
    {
        if (currentPiece != null && currentPiece.CanIteract())
            return currentPiece.GetMovimentPoints();

        return 0;
    }

    public void LitTile()
    {
        plane.Lit();
    }

    public void UnlitTile()
    {
        plane.Deactivate();
    }

    public void Clear()
    {
        currentPiece?.Clear();

        Destroy(gameObject);
    }

    public override void OnEnter()
    {
        if (!canCheck) return;

        plane.LitPlane();
    }

    public override void OnExit()
    {
        if (!canCheck) return;

        plane.UnlitPlane();
    }

    public override void OnClick()
    {
        if (!canCheck) return;

        base.OnClick();
    }

    public void ReceiveUpdate(GameState updatedValue)
    {
        // Dont lit obstacles
        if(currentPiece != null && currentPiece.PieceType == PieceType.Obstacle)
        {
            canCheck = false;
            return;
        }

        // Reset Tile on new State
        if(updatedValue == GameState.Ready)
        {
            plane.Deactivate();

            // Can select only current player soldiers
            if (currentPiece != null && currentPiece.CanIteract())
            {
                canCheck = true;
                return;
            }
        }

        // Try to move or attack
        if(updatedValue == GameState.SelectedSoldier)
        {
            // Cant move to or attack current player soldiers
            if(currentPiece != null && currentPiece.CanIteract())
            {
                canCheck = false;
                return;
            }

            canCheck = true;
            return;
        }

        // Reset all lit values on Player change or GameOver
        if(updatedValue == GameState.GameOver || updatedValue == GameState.NextPlayer)
        {
            plane.Deactivate();
            return;
        }

        canCheck = false;
    }
}
