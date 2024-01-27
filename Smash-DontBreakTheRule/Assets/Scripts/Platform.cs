using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
    [SerializeField] private LayerMask DefaultMask;
    [SerializeField] private LayerMask Player1;
    [SerializeField] private LayerMask Player2;
    [SerializeField] private float recoverTime = 0.1f;

    private PlatformEffector2D platformEffector;
    private float lastDownJumpTime1 = 0f;
    private float lastDownJumpTime2 = 0f;

    void Awake()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
        platformEffector.colliderMask = DefaultMask;
    }

    void Update()
    {
        if (Time.time - lastDownJumpTime1 >= recoverTime) {
            platformEffector.colliderMask = platformEffector.colliderMask | (Player1);
        }
        if (Time.time - lastDownJumpTime2 >= recoverTime) {
            platformEffector.colliderMask = platformEffector.colliderMask | (Player2);
        }
    }

    public void Disable(int num) {
        if (num == 1) {
            lastDownJumpTime1 = Time.time;
            platformEffector.colliderMask = platformEffector.colliderMask & (~Player1);
        }
        if (num == 2) {
            lastDownJumpTime2 = Time.time;
            platformEffector.colliderMask = platformEffector.colliderMask & (~Player2);
        }
    }
}
