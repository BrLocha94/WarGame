using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyEditorObject : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_EDITOR
        return;
#endif

        Destroy(gameObject);
    }
}
