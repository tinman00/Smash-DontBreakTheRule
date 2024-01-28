using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Header("Player ID")]
    private int id = 0;

    [Header("Conponents")]
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private Player plr;

    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float slowedMoveSpeed = 13f;
    private Vector2 dir;
    private bool facingRight = true;
    public bool freeze = false;

    [Header("Jump Variables")]
    [SerializeField] private float jumpSpeed = 15f;
    [SerializeField] private float extraJumpSpeed = 12f;
    [SerializeField] private float minimumLimit = 0.08f;
    [SerializeField] private float peakTime = 0.25f;
    [SerializeField] private float maxFallingSpeed = 20f;
    [SerializeField] private float airJumpBonus = 0.1f;
    [SerializeField] private int jumpLimit = 2;
    public bool jumping = false;
    private Vector2 jumpVelocity;
    private float jumpStart;
    private int jumpCount = 0;
    private bool groundedJump => (grounded || Time.time - lastJumpTime <= airJumpBonus);
    private bool canJump => groundedJump || (jumpCount + 1 < jumpLimit);
    private float lastJumpTime = float.NaN;

    [Header("Gound Collision Variables")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask ceilingLayer;
    [SerializeField] private LayerMask platformLayer;
    private bool grounded;
    private bool roofed;
    private float boxHeight = 0.2f;
    private Vector2 playerSize => coll == null ? new Vector2() : new Vector2(coll.size.x * transform.localScale.x, coll.size.y * transform.localScale.y);
    private Vector2 topCenter => (Vector2)transform.position + Vector2.up * playerSize.y * 0.5f;
    private Vector2 bottomCenter => (Vector2)transform.position + Vector2.down * playerSize.y * 0.5f;
    private Vector2 boxSize => new Vector2(playerSize.x * 1f, boxHeight);

    private class KnockBack {
        public Vector2 force { get; private set; }
        public float duration { get; private set; }
        public float start { get; private set; }

        public KnockBack(Vector2 _force, float _duration) {
            force = _force;
            duration = _duration;
            start = Time.time;
        }
    }
    [Header("Knockback Variables")]
    private List<KnockBack> knockbacks = new();

    [Header("Shoe")]
    private float lastShoe = 0f;
    [SerializeField] private float shoeTime = 10f;
    [SerializeField] private float shoeBoost = 1.5f;
    private bool shoe = false;


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        plr = GetComponent<Player>();
        id = plr.id;
    }

    void Update() {
        if (plr.state.died) return;

        CheckCollision();
        GetInput();
        MoveCharacter();
        Flip();

        Jump();
        JumpPhysics();
        Fall();
        if (Time.time > lastShoe + shoeTime) {
            shoe = false;
        }

        foreach (var t in flipHistory) {
            if (t + 8f < Time.time) {
                flipHistory.Remove(t);
            }
        }
    }
    
    private void GetInput() {
        dir = new Vector2(InputManager.instance.GetAxisHorizontal[id], InputManager.instance.GetAxisVertical[id]);
    }

    private List<float> flipHistory = new();

    private void Flip() {
        plr.state.isFacingRight = facingRight;
        if (!plr.state.canFlip) return;
        if (plr.state.attacking || plr.state.blocking) return;
        if (dir.x == 0f) return;
        var ruleManager = RuleManager.instance;
        if (!ruleManager.HasRule[(int)Rule.Turn]) {
            flipHistory.Clear();
        }

        if (facingRight ^ dir.x > 0f) {
            facingRight = !facingRight;
            transform.Rotate(new Vector3(0f, 180f, 0f));

            if (ruleManager.HasRule[(int)Rule.Turn]) {
                flipHistory.Add(Time.time);
            }
            if (flipHistory.Count >= 5) {
                ruleManager.BreakRule(Rule.Turn, plr);
            }
        }
    }
    public void AddShoe() {
        lastShoe = Time.time;
        shoe = true;
    }

    private void CheckCollision() {
        grounded = Physics2D.BoxCast(bottomCenter, boxSize, 0f, Vector2.down, 0f, groundLayer).collider != null;
        if (grounded) {
            lastJumpTime = Time.time;
            jumpCount = 0;
        }
        roofed = Physics2D.BoxCast(topCenter, boxSize, 0f, Vector2.up, 0f, ceilingLayer).collider != null;
        if (roofed) {
            JumpEnd(false);
        }
    }

    private void Jump() {
        if (!InputManager.instance.Jump[id]) {
            JumpEnd(true);
            return;
        }
        if (!jumping && canJump && InputManager.instance.JumpStart[id]) {
            var leftObj = Physics2D.Raycast(bottomCenter + Vector2.left * playerSize.x * 0.5f, Vector2.down, boxHeight, (~platformLayer) & groundLayer).collider;
            var rightObj = Physics2D.Raycast(bottomCenter + Vector2.right * playerSize.x * 0.5f, Vector2.down, boxHeight, (~platformLayer) & groundLayer).collider;
            if (groundedJump && InputManager.instance.DownJump[id] &&
                leftObj == null &&
                rightObj == null) {
                leftObj = Physics2D.Raycast(bottomCenter + Vector2.left * playerSize.x * 0.5f, Vector2.down, boxHeight, platformLayer).collider;
                rightObj = Physics2D.Raycast(bottomCenter + Vector2.right * playerSize.x * 0.5f, Vector2.down, boxHeight, platformLayer).collider;
                if (leftObj != null) leftObj.GetComponent<Platform>().Disable(id + 1);
                if (rightObj != null) rightObj.GetComponent<Platform>().Disable(id + 1);
                Debug.Log("returned");

                return;
            }
            // Debug.Log($"{jumpCount}");
            if (!groundedJump) {
                jumpCount++;
                jumpVelocity = new Vector2(rb.velocity.x, extraJumpSpeed);
            } else {
                jumpVelocity = new Vector2(rb.velocity.x, jumpSpeed);
            }
            jumpStart = Time.time;
            jumping = true;
        }
    }

    private void JumpPhysics() {
        if (!jumping) return;
        float currentTime = Time.fixedTime - jumpStart;
        if (currentTime <= peakTime) {
            rb.velocity = new(rb.velocity.x, jumpVelocity.y);
        } else {
            JumpEnd(false);
        }
    }

    private void Fall() {
        if (jumping || grounded) return;

        if (rb.velocity.y < -maxFallingSpeed) {
            rb.velocity = new Vector2(rb.velocity.x, -maxFallingSpeed);
        }
    }

    private void JumpEnd(bool manually) {
        float currentTime = Time.time - jumpStart;
        if (manually && currentTime < minimumLimit) {
            return;
        }
        jumpStart = float.NaN;
        jumping = false;
    }

    private void MoveCharacter() {
        // anim.SetBool("Running", dir.x != 0f);
        // rb.velocity = new(dir.x * moveSpeed, rb.velocity.y);
        var spd = moveSpeed;
        if (plr.state.attacking || plr.state.blocking) {
            spd = slowedMoveSpeed;
        }
        if (plr.state.canMove) {
            rb.velocity = new(dir.x * spd, rb.velocity.y);
            anim.SetBool("Running", Mathf.Abs(dir.x) >= 0.1f);
        } else {
            if (!plr.state.hurted) {
                rb.velocity = new(0, rb.velocity.y);
            }
            anim.SetBool("Running", false);
        }
        if (shoe) {
            rb.velocity = new(rb.velocity.x * shoeBoost, rb.velocity.y);
        }
    }
    private void LateUpdate() {
        if (plr.state.died) {
            knockbacks.Clear();
        }
        Vector2 tmp = Vector2.zero;
        for (var i = 0; i < knockbacks.Count; i++) {
            tmp += knockbacks[i].force;
        }
        Vector2 v = Vector2.zero;
        for (var i = 0; i < knockbacks.Count; i++) {
            if (knockbacks[i].start + knockbacks[i].duration >= Time.time) {
                v += knockbacks[i].force;
            } else {
                knockbacks.Remove(knockbacks[i]);
            }
        }
        if (v != Vector2.zero) rb.velocity = v;
        else if (tmp != Vector2.zero) {
            rb.velocity = Vector2.zero;
        }
    }

    public void Knockback(Vector2 force, float duration) {
        knockbacks.Add(new KnockBack(force, duration));
    }

    private void OnDrawGizmos() {
        Gizmos.color = grounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(bottomCenter, boxSize);

        Gizmos.color = roofed ? Color.green : Color.red;
        Gizmos.DrawWireCube(topCenter, boxSize);
    }
}
