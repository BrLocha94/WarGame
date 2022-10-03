using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DetailsScreenBase<T> : MonoBehaviour
{
    public virtual void Activate(T target)
    {
        gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
