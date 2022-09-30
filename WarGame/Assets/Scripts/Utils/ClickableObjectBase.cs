using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClickableObjectBase<T> : MonoBehaviour
{
    public delegate void OnTargetClickedHandler(T target);
    public event OnTargetClickedHandler onTargetClickedEvent;

    public virtual void OnEnter()
    {
        Debug.Log("Enter on object " + name);
    }

    public virtual void OnExit()
    {
        Debug.Log("Exit off object " + name);
    }

    public virtual void OnClick()
    {
        Debug.Log("Clicked on object " + name);

        onTargetClickedEvent?.Invoke(GetComponent<T>());
    }
}
