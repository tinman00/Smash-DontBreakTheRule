using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
    [Header("Player ID")]
    private int id = 0;

    [SerializeField] private Hitbox simpleHitbox;

    [Header("Conponents")]
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private Player plr;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        plr = GetComponent<Player>();
        id = plr.id;
    }

    private void Update() {
        anim.SetBool("SimpleAttack", InputManager.instance.Attack[id]);
    }

    public void SimpleAttack() {
        simpleHitbox.Activate();
    }

    public void SimpleAttackEnd() {
        simpleHitbox.Deactivate();
    }
}