using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static List<Player> list = new();
    public int id;
    [SerializeField] private int totalLifeCount = 10;
    [SerializeField] private int maxHP = 20;
    [SerializeField] public int hp { private set; get; }
    [SerializeField] private float knockBack = 10f;
    [SerializeField] private float knockBackDuration = 0.2f;
    [SerializeField] private Vector3[] spawnPoint;

    [Header("Conponents")]
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    public Animator anim;
    private PlayerMovement move;
    public PlayerItemStorage items;

    public PlayerState state = new();
    public GameObject ShieldPrefab;
    public float shieldDuration = 10f;
    private float shieldTime = 0f;
    private bool defending = false;
    private GameObject shield;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        move = GetComponent<PlayerMovement>();
        items = GetComponent<PlayerItemStorage>();
        hp = maxHP;
        list.Add(this);
    }

    private void Update() {
        if (state.hurted || state.stuned || state.reviving) {
            Freeze();
        } else {
            Unfreeze();
        }
        if (hp == 0 && !state.died) {
            Die();
        }
        if (shield != null && Time.time > shieldTime) {
            RemoveShield();
        }
        var ruleManager = RuleManager.instance;
        if (transform.position.x < 0) {
            if (ruleManager.HasRule[(int)Rule.StayLeft]) {
                ruleManager.BreakRule(Rule.StayLeft, this);
            }
        } else {
            if (ruleManager.HasRule[(int)Rule.StayRight]) {
                ruleManager.BreakRule(Rule.StayRight, this);
            }
        }
    }

    public void StuneEnd() {
        state.stuned = false;
    }
    public void AddShield() {
        shieldTime = Time.time + shieldDuration;
        if (shield == null) {
            shield = Instantiate(ShieldPrefab);
            shield.GetComponent<Shield>().target = transform;
            defending = true;
        }
    }

    public void RemoveShield() {
        if (shield != null) {
            shield.GetComponent<Shield>().destroy();
        }
        shield = null;
        defending = false;
        shieldTime = 0f;
    }

    public bool CanPick() {
        return items.CanAdd();
    }

    public void AddPickup(PickUp pickup) {
        items.Add(pickup);
    }

    public void Freeze() {
        state.canMove = false;
        state.canFlip = false;
        state.canUseSkill = false;
    }

    public void Unfreeze() {
        state.canMove = true;
        state.canFlip = true;
        state.canUseSkill = true;
    }

    public bool Attacked(int damage, Vector2 direction, Player src)
    {
        if (state.died || state.reviving) return false;
        if (defending) {
            RemoveShield();
            return false;
        }
        if (src == null) {
            hp -= damage;
        } else {
            if (state.blocking && (state.isFacingRight ^ (Mathf.Sign(direction.x) == 1f))) {
                src.Stune();
                return false;
            } else {
                anim.SetTrigger("Hurt");
                rb.velocity = new();
                Vector2 vec = new Vector2(Mathf.Sign(direction.x), 0.4f);
                vec.Normalize();
                move.Knockback(vec * knockBack, knockBackDuration);
                state.attacking = state.blocking = state.stuned = false;
                hp -= damage;
                if (RuleManager.instance.HasRule[(int)Rule.NotHurtOpponent]) {
                    RuleManager.instance.BreakRule(Rule.NotHurtOpponent, src);
                }
            }
        }
        if (hp <= 0) {
            hp = 0;
        }
        return true;
    }

    public void Stune() {
        anim.SetTrigger("Stunned");
        state.stuned = true;
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
        totalLifeCount--;
        if (totalLifeCount == 0) {
            // End Game
        }
    }

    public void Revive() {
        hp = maxHP;
        transform.position = spawnPoint[Random.Range(0, spawnPoint.Length)];
        state.died = false;
        state.reviving = true;
    }

    public void ReviveEnd() {
        state.reviving = false;
    }
}
