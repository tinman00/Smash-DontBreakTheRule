using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    [SerializeField] private float attackInputDelay;

    public static InputManager instance { get; private set; }
    public float[] GetAxisHorizontal { get; private set; }
    public float[] GetAxisVertical { get; private set; }
    public bool[] JumpStart { get; private set; }
    public bool[] Jump { get; private set; }
    public bool[] JumpEnd { get; private set; }
    public bool[] DownJump { get; private set; }
    public bool[] Attack { get; private set; }

    public bool[] Block { get; private set; }

    public float lastAttackTime = float.NaN;

    private void Start() {
        if (instance == null) {
            instance = this;
        }
        GetAxisHorizontal = new float[2] { 0f, 0f };
        GetAxisVertical = new float[2] { 0f, 0f };

        Jump = new bool[2] { false, false };
        JumpStart = new bool[2] { false, false };
        JumpEnd = new bool[2] { false, false };
        Attack = new bool[2] { false, false };
        Block = new bool[2] { false, false };
        DownJump = new bool[2] { false, false };
    }

    private void Update() {
        float x1 = Input.GetAxis("Horizontal"), y1 = Input.GetAxis("Vertical");
        // float x2 = Input.GetAxis("Horizontal"), y2 = Input.GetAxis("Vertical");
        float x2 = 0f, y2 = 0f;
        float rx1 = Input.GetAxisRaw("Horizontal"), ry1 = Input.GetAxisRaw("Vertical");
        // float rx2 = Input.GetAxisRaw("Horizontal"), ry2 = Input.GetAxisRaw("Vertical");
        float rx2 = 0f, ry2 = 0f;
        GetAxisHorizontal[0] = x1;
        GetAxisHorizontal[1] = x2;
        GetAxisVertical[0] = y1;
        GetAxisVertical[1] = y2;

        Jump[0] = Jump[1] = false;
        JumpStart[0] = JumpStart[1] = false;
        JumpEnd[0] = JumpEnd[1] = false;
        Attack[0] = Attack[1] = false;
        Block[0] = Block[1] = false;
        DownJump[0] = DownJump[1] = false;

        Jump[0] = Input.GetKey(KeyCode.Space);
        JumpStart[0] = Input.GetKeyDown(KeyCode.Space);
        JumpEnd[0] = Input.GetKeyUp(KeyCode.Space);

        if (Input.GetMouseButtonDown(0)) {
            Attack[0] = true;
        }
        if (Input.GetMouseButton(1))
        {
            Block[0] = true;
        }

        if (ry1 < -0.25f && JumpStart[0]) {
            DownJump[0] = true;
        }
    }
}
