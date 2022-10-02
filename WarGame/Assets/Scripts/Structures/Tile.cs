using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : ClickableObjectBase<Tile>
{
    [SerializeField]
    private Plane plane;

    public int row { get; private set; } = -1;
    public int column { get; private set; } = -1;

    private Piece currentPiece;

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

    public void Clear()
    {
        //TODO: Clear piece

        Destroy(gameObject);
    }

    public override void OnEnter()
    {
        plane.LitPlane();
    }

    public override void OnExit()
    {
        plane.UnlitPlane();
    }

    public override void OnClick()
    {
        plane.Select();

        base.OnClick();
    }
}
