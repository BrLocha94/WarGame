using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    [SerializeField]
    private PieceType pieceType;
    [SerializeField]
    private AnimationCurve animationCurve;
    [SerializeField]
    private float animationTime;

    public PieceType PieceType => pieceType;

    public virtual int GetMovimentPoints() => 0;
    public virtual void ExecuteMoviment(int movimentCost) { }
    public virtual bool CanIteract() => pieceType == PieceType.Soldier;

    public void SetParent(Transform parent, bool animate = false, Action callback = null)
    {
        transform.parent = parent;

        if (animate)
        {
            Vector3 position01 = transform.localPosition;
            Vector3 position02 = new Vector3(0f, position01.y, 0f);
            StartCoroutine(MoveRoutine(position01, position02, animationCurve, animationTime, callback));
        }
        else
            transform.position = new Vector3(parent.position.x, transform.position.y, parent.position.z);
    }

    private IEnumerator MoveRoutine(Vector3 position01, Vector3 position02, AnimationCurve curve, float animationTime, Action callback)
    {
        float timer = 0f;
        while (timer < animationTime)
        {
            transform.localPosition = Vector3.Lerp(position01, position02, curve.Evaluate(timer / animationTime));
            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = position02;

        callback?.Invoke();
    }

    public void Clear()
    {
        Destroy(gameObject);
    }
}

public enum PieceType
{
    Null,
    Soldier,
    Obstacle
}
