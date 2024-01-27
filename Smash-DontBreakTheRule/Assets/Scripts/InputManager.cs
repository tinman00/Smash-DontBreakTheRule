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
    public bool[] Skill1 { get; private set; }
    public bool[] Skill2 { get; private set; }

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
        Skill1 = new bool[2] { false, false };
        Skill2 = new bool[2] { false, false };
    }

    private void Update() {
        float x1 = Input.GetAxis("Horizontal"), y1 = Input.GetAxis("Vertical");
        float x2 = Input.GetAxis("Horizontal1"), y2 = Input.GetAxis("Vertical1");
        float rx1 = Input.GetAxisRaw("Horizontal"), ry1 = Input.GetAxisRaw("Vertical");
        float rx2 = Input.GetAxisRaw("Horizontal1"), ry2 = Input.GetAxisRaw("Vertical1");
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
        Skill1[0] = Skill1[1] = false;
        Skill2[0] = Skill2[1] = false;

        Jump[0] = Input.GetButton("Jump");
        JumpStart[0] = Input.GetButtonDown("Jump");
        JumpEnd[0] = Input.GetButtonUp("Jump");

        if (Input.GetButtonDown("Fire1")) {
            Attack[0] = true;
        }
        if (Input.GetButton("Fire2"))
        {
            Block[0] = true;
        }

        if (ry1 < -0.25f && JumpStart[0]) {
            DownJump[0] = true;
        }

        Skill1[0] = Input.GetButtonDown("Skill11");
        Skill2[0] = Input.GetButtonDown("Skill21");
        Skill1[1] = Input.GetButtonDown("Skill12");
        Skill2[1] = Input.GetButtonDown("Skill22");

        Jump[1] = Input.GetButton("Jump1");
        JumpStart[1] = Input.GetButtonDown("Jump1");
        JumpEnd[1] = Input.GetButtonUp("Jump1");

        if (Input.GetButtonDown("Fire11")) {
            Attack[1] = true;
        }
        if (Input.GetButton("Fire21"))
        {
            Block[1] = true;
        }

        if (ry1 < -0.25f && JumpStart[1]) {
            DownJump[1] = true;
        }
    }
}
