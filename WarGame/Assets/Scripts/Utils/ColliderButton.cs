using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ColliderButton : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onMouseEnterCallback;
    [SerializeField]
    private UnityEvent onMouseExitCallback;
    [SerializeField]
    private UnityEvent onClickCallback;

    private bool isOnCollider = false;
    private bool clicked = false;

    private void OnMouseEnter()
    {
        isOnCollider = true;

        onMouseEnterCallback?.Invoke();
    }

    private void OnMouseExit()
    {
        isOnCollider = false;
        clicked = false;

        onMouseExitCallback?.Invoke();
    }

    private void OnMouseDown()
    {
        if (!isOnCollider) return;

        clicked = true;
    }

    private void OnMouseUp()
    {
        if (!isOnCollider || !clicked) return;

        clicked = false;

        onClickCallback?.Invoke();
    }
}
