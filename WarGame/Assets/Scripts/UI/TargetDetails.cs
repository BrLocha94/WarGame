using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetDetails : DetailsScreenBase<string>
{
    [SerializeField]
    private Text nameText;

    public override void Activate(string info)
    {
        base.Activate(info);

        nameText.text = info;
    }
}
