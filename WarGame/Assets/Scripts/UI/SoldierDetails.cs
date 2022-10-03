using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierDetails : DetailsScreenBase<Soldier>
{
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text pointsText;

    public override void Activate(Soldier target)
    {
        base.Activate(target);

        nameText.text = "Name: " + target.GetName();
        pointsText.text = "Points: " + target.GetPointsLeft();
    }
}
