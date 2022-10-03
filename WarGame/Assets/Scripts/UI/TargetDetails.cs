using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetDetails : DetailsScreenBase<Tile>
{
    [SerializeField]
    private Text nameText;

    public override void Activate(Tile target)
    {
        base.Activate(target);
    }
}
