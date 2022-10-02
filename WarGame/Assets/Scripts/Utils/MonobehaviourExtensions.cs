using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonobehaviourExtensions
{
    private static IEnumerator Invoke(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    private static IEnumerator InvokeAfterFrame(System.Action action)
    {
        yield return new WaitForEndOfFrame();
        action?.Invoke();
    }

    public static Coroutine Invoke(this MonoBehaviour host, float delay, System.Action action)
    {
        return host.StartCoroutine(Invoke(delay, action));
    }

    public static Coroutine InvokeAfterFrame(this MonoBehaviour host, System.Action action)
    {
        return host.StartCoroutine(InvokeAfterFrame(action));
    }
}
