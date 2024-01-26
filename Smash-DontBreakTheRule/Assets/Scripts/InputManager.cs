using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    [SerializeField] private float attackInputDelay;

    public static InputManager instance { get; private set; }
    public float GetAxisHorizontal { get; private set; }
    public float GetAxisVertical { get; private set; }
    public bool JumpStart { get; private set; }
    public bool Jump { get; private set; }
    public bool JumpEnd { get; private set; }
    public bool Attack { get; private set; }

    public float lastAttackTime = float.NaN;

    private void Start() {
        if (instance == null) {
            instance = this;
        }
    }

    private void Update() {
        float x = Input.GetAxis("Horizontal"), y = Input.GetAxis("Vertical");
        GetAxisHorizontal = x;
        GetAxisVertical = y;

        Jump = false;
        JumpStart = false;
        JumpEnd = false;
        Attack = false;

        Jump = Input.GetKey(KeyCode.Space);
        JumpStart = Input.GetKeyDown(KeyCode.Space);
        JumpEnd = Input.GetKeyUp(KeyCode.Space);
        if (Input.GetMouseButtonDown(0)) {
            Attack = true;
        }
    }
}
