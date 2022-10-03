using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivot : MonoBehaviour, IReceiver<GameState>
{
    [Header("Camera movement keymap")]
    [SerializeField]
    private KeyCode frontMoveKey = KeyCode.W;
    [SerializeField]
    private KeyCode backMoveKey = KeyCode.S;
    [SerializeField]
    private KeyCode leftMoveKey = KeyCode.A;
    [SerializeField]
    private KeyCode rightMoveKey = KeyCode.D;

    [Space]
    [SerializeField]
    private KeyCode leftRotationKey = KeyCode.Q;
    [SerializeField]
    private KeyCode rightRotationKey = KeyCode.E;

    [Header("General configs")]
    [SerializeField]
    private float verticalSpeed = 1f;
    [SerializeField]
    private float horizontalSpeed = 1f;
    [SerializeField]
    private float rotationSpeed = 1f;

    private Vector3 moveVector = Vector3.zero;
    private Vector3 rotationVector = Vector3.zero;

    bool canMove = true;

    private void FixedUpdate()
    {
        moveVector = Vector3.zero;

        if (Input.GetKey(frontMoveKey))
            moveVector.z += verticalSpeed;
        if (Input.GetKey(backMoveKey))
            moveVector.z -= verticalSpeed;

        if (Input.GetKey(rightMoveKey))
            moveVector.x += horizontalSpeed;
        if (Input.GetKey(leftMoveKey))
            moveVector.x -= horizontalSpeed;

        rotationVector = Vector3.zero;

        if (Input.GetKey(rightRotationKey))
            rotationVector.y += rotationSpeed;
        if (Input.GetKey(leftRotationKey))
            rotationVector.y -= rotationSpeed;
    }

    private void Update()
    {
        if (!canMove) return;

        transform.Translate(moveVector * Time.deltaTime);
        transform.Rotate(rotationVector * Time.deltaTime);
    }

    public void ReceiveUpdate(GameState updatedValue)
    {
        canMove = (updatedValue == GameState.Ready ||
                   updatedValue == GameState.SelectedSoldier ||
                   updatedValue == GameState.Moving ||
                   updatedValue == GameState.Attacking);
    }
}
