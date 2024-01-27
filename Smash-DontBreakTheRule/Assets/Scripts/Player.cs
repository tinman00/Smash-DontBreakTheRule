using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    [SerializeField] private int maxHP = 20;
    [SerializeField] public int hp { private set; get; }

    [Header("Conponents")]
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        hp = maxHP;
    }

    public void Attacked(int damage, Vector2 direction)
    {
        
        Debug.Log($"Player {id + 1} is attacked: damage {damage}");
    }

    public void Die() {

    }
}
