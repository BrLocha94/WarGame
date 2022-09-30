using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : ClickableObjectBase<Tile>
{
    [SerializeField]
    private GameObject targetObject;

    public int row { get; private set; } = -1;
    public int column { get; private set; } = -1;

    public void Initialize(int row, int column, Transform parent, Vector3 localPosition)
    {
        this.row = row;
        this.column = column;

        name = "Tile[ " + row + " ][ " + column + " ]";

        transform.parent = parent;
        transform.localPosition = localPosition;
    }

    public override void OnEnter()
    {
        targetObject.SetActive(true);

        base.OnEnter();
    }

    public override void OnExit()
    {
        targetObject.SetActive(false);

        base.OnExit();
    }

    public override void OnClick()
    {
        base.OnClick();
    }
}
