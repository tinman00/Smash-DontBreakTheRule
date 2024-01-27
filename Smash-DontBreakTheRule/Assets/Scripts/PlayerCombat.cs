using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
    [Header("Player ID")]
    private int id = 0;

    [SerializeField] private Hitbox NrmAttHitbox;// Normal Attack 's Hitbox;

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
        anim.SetBool("NormalAttack", InputManager.instance.Attack[id]);
        anim.SetBool("Block", InputManager.instance.Block[id]);
        if (InputManager.instance.Block[id]) {
            Block();
        } else {
            BlockEnd();
        }
    }


    public void NormalAttack() {
        plr.state.attacking = true;
    }

    public void NormalAttackEnd() {
        plr.state.attacking = false;
    }

    public void ActivateNrmHitbox() {
        NrmAttHitbox.Activate();
    }

    public void DeactivateNrmHitbox() {
        NrmAttHitbox.Deactivate();
    }
    public void Block()
    {
        plr.state.blocking = true;
    }
    public void BlockEnd() {
        plr.state.blocking = false;
    }
}
