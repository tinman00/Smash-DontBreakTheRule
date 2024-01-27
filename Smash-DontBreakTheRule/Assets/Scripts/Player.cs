using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    [SerializeField] private int maxHP = 20;
    [SerializeField] public int hp { private set; get; }
    [SerializeField] private float knockBack = 10f;
    [SerializeField] private float knockBackDuration = 0.2f;

    [Header("Conponents")]
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private PlayerMovement move;

    public PlayerState state = new();
    private float stunedTime = 0f;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        move = GetComponent<PlayerMovement>();
        hp = maxHP;
    }

    private void Update() {
        if (state.blocking || state.attacking || state.hurted || state.stuned) {
            Freeze();
        } else {
            Unfreeze();
        }
        if (Time.time > stunedTime) {
            state.stuned = false;
            anim.SetBool("Stunning", false);
        }
        if (hp == 0) {
            Die();
        }
    }

    public void Freeze() {
        state.canMove = false;
        state.canFlip = false;
    }

    public void Unfreeze() {
        state.canMove = true;
        state.canFlip = true;
    }

    public void Attacked(int damage, Vector2 direction, Player src)
    {
        if (src == null) {
            hp -= damage;
        } else {
            if (state.blocking && (state.isFacingRight ^ (Mathf.Sign(direction.x) == 1f))) {
                src.Stune(0.5f);
            } else {
                anim.SetTrigger("Hurt");
                rb.velocity = new();
                Vector2 vec = new Vector2(Mathf.Sign(direction.x), 0.4f);
                vec.Normalize();
                move.Knockback(vec * knockBack, knockBackDuration);
                state.attacking = state.blocking = false;
                stunedTime = 0f;
                hp -= damage;
            }
        }
        if (hp <= 0) {
            hp = 0;
        }
    }

    public void Stune(float duration) {
        anim.SetBool("Stunning", true);
        state.stuned = true;
        stunedTime = Time.time + duration;
    }

    public void Hurt() {
        state.hurted = true;
    }

    public void HurtEnd() {
        state.hurted = false;
    }

    public void Die() {
        anim.SetTrigger("Die");
        state.died = true;

    }
}
