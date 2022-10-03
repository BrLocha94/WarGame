using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonUiBase : MonoBehaviour
{
    public virtual void Activate()
    {
        gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public abstract void OnClick();
}
