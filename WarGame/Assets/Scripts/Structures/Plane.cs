using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    [Header("External references")]
    [SerializeField]
    private MeshRenderer meshRenderer;

    [Header("Material references")]
    [SerializeField]
    private Material pulseMaterial;
    [SerializeField]
    private Material selectedMaterial;
    [SerializeField]
    private Material unselectedMaterial;
    [SerializeField]
    private Material litMaterial;

    [Header("Animation configs")]
    [SerializeField]
    private AnimationCurve pulseCurve;
    [SerializeField]
    private float pulseTime;

    private Coroutine coroutine = null;
    private PlaneState currentState = PlaneState.Unlit;

    public void LitPlane()
    {
        ChangeState(PlaneState.Pulse);
    }
    
    public void UnlitPlane()
    {
        ChangeState(PlaneState.Unlit);
    }

    public void Select()
    {
        ChangeState(PlaneState.Selected);
    }

    public void Unselect()
    {
        ChangeState(PlaneState.Unselected);
    }

    public void Deactivate()
    {
        ChangeState(PlaneState.Deactivate);
    }

    public void Lit()
    {
        ChangeState(PlaneState.Lit);
    }

    private void ChangeState(PlaneState nextState)
    {
        if(nextState == PlaneState.Deactivate)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);

            currentState = PlaneState.Unlit;
            gameObject.SetActive(false);

            return;
        }

        if(currentState == PlaneState.Unlit)
        {
            if(nextState == PlaneState.Pulse)
            {
                gameObject.SetActive(true);

                if (coroutine != null)
                    StopCoroutine(coroutine);

                meshRenderer.material = pulseMaterial;
                currentState = nextState;

                coroutine = StartCoroutine(PulseRoutine(pulseCurve, pulseTime, true));

                return;
            }
        }

        if(currentState == PlaneState.Pulse)
        {
            if(nextState == PlaneState.Lit)
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);

                meshRenderer.material = litMaterial;
                currentState = nextState;

                coroutine = StartCoroutine(PulseRoutine(pulseCurve, pulseTime, true));

                return;
            }

            if(nextState == PlaneState.Unlit)
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);

                currentState = nextState;

                gameObject.SetActive(false);

                return;
            }

            if(nextState == PlaneState.Selected)
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);

                meshRenderer.material = selectedMaterial;
                currentState = nextState;

                coroutine = StartCoroutine(PulseRoutine(pulseCurve, pulseTime, true));

                return;
            }
        }
    }

    private IEnumerator PulseRoutine(AnimationCurve curve, float time, bool repeat)
    {
        Color color = meshRenderer.material.color;
        float routineTimer = 0f;
        bool loop = true;

        while (loop)
        {
            while (routineTimer < time)
            {
                float newAlpha = Mathf.Lerp(0.3f, 1f, curve.Evaluate(routineTimer / time));
                color.a = newAlpha;
                meshRenderer.material.color = color;
                routineTimer += Time.deltaTime;
                yield return null;
            }

            routineTimer = 0f;
            loop = repeat;
        }
    }
}

public enum PlaneState
{
    Unlit,
    Pulse,
    Selected,
    Unselected,
    Deactivate,
    Lit
}