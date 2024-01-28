using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
    [Header("Player ID")]
    private int id = 0;

    [SerializeField] private Hitbox NrmAttHitbox;// Normal Attack 's Hitbox;
    [SerializeField] private Hitbox SplAttHitbox;// Special Attack 's Hitbox;
    [SerializeField] private GameObject Bomb;
    [SerializeField] private float throwingForce;
    [SerializeField] private Animator emote;

    [Header("Conponents")]
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private Player plr;
    private PlayerMovement move;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        plr = GetComponent<Player>();
        move = GetComponent<PlayerMovement>();
        id = plr.id;
    }

    private float emoteRuleTime = float.NaN;

    private void Update() {
        anim.SetBool("NormalAttack", InputManager.instance.Attack[id]);
        anim.SetBool("Block", InputManager.instance.Block[id]);
        if (InputManager.instance.Block[id]) {
            Block();
        } else {
            BlockEnd();
        }
        if (InputManager.instance.Taunt[id]) {
            emote.SetTrigger("Laugh");
            if (RuleManager.instance.HasRule[(int)Rule.NotEmote]) {
                emoteRuleTime = Time.time + 5f;
            }
        }

        if (RuleManager.instance.HasRule[(int)Rule.NotEmote]) {
            if (float.IsNaN(emoteRuleTime))
                emoteRuleTime = Time.time + 5f;
            if (!float.IsNaN(emoteRuleTime) && Time.time > emoteRuleTime) {
                RuleManager.instance.BreakRule(Rule.NotEmote, plr);
                emoteRuleTime = float.NaN;
            }
        } else {
            emoteRuleTime = float.NaN;
        }

        if (InputManager.instance.Skill2[id]) {
            var item = plr.items.Use(1);
            if (item == PickUp.None) return;
            switch(item) {
                case PickUp.Shield:
                    plr.AddShield();
                    break;
                case PickUp.Shoe:
                    move.AddShoe();
                    break;
                case PickUp.Mace:
                    anim.SetBool("SpecialAttack", true);
                    break;
                case PickUp.Bomb:
                    var obj = Instantiate(Bomb, transform.position, new Quaternion());
                    obj.GetComponent<Rigidbody2D>().AddForce(
                        (plr.state.isFacingRight ? Vector2.right : Vector2.left) * throwingForce, ForceMode2D.Impulse);
                    break;
            }
            if (RuleManager.instance.HasRule[(int)Rule.UseItem]) {
                anim.SetBool("SpecialAttack", false);
                RuleManager.instance.BreakRule(Rule.UseItem, plr);
            }
        }
        if (InputManager.instance.Skill1[id]) {
            var item = plr.items.Use(0);
            if (item == PickUp.None) return;
            switch (item) {
                case PickUp.Shield:
                    plr.AddShield();
                    break;
                case PickUp.Shoe:
                    move.AddShoe();
                    break;
                case PickUp.Mace:
                    anim.SetBool("SpecialAttack", true);
                    break;
                case PickUp.Bomb:
                    var obj = Instantiate(Bomb, transform.position, new Quaternion());
                    obj.GetComponent<Rigidbody2D>().AddForce(
                        (plr.state.isFacingRight ? Vector2.right : Vector2.left) * throwingForce, ForceMode2D.Impulse);
                    break;
            }
            if (RuleManager.instance.HasRule[(int)Rule.UseItem]) {
                anim.SetBool("SpecialAttack", false);
                RuleManager.instance.BreakRule(Rule.UseItem, plr);
            }
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
    public void SpecialAttack() {
        plr.state.attacking = true;
    }

    public void SpecialAttackEnd() {
        plr.state.attacking = false;
        anim.SetBool("SpecialAttack", false);
    }

    public void ActivateSplHitbox() {
        SplAttHitbox.Activate();
    }

    public void DeactivateSplHitbox() {
        SplAttHitbox.Deactivate();
    }
    public void Block()
    {
        plr.state.blocking = true;
    }
    public void BlockEnd() {
        plr.state.blocking = false;
    }
}
