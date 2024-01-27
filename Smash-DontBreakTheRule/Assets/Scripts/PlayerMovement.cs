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
    // Start is called before the first frame update
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        plr = GetComponent<Player>();
        id = plr.id;
    }

    // Update is called once per frame
    void Update() {
        CheckCollision();
        GetInput();
        MoveCharacter();
        Flip();

        Jump();
        JumpPhysics();
        Fall();
    }

    private void GetInput() {
        dir = new Vector2(InputManager.instance.GetAxisHorizontal[id], InputManager.instance.GetAxisVertical[id]);
    }

    private void Flip() {
        if (dir.x == 0f) return;
        if (facingRight ^ dir.x > 0f) {
            facingRight = !facingRight;
            transform.Rotate(new Vector3(0f, 180f, 0f));
        }
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
                if (leftObj != null) leftObj.GetComponent<Platform>().Disable(1);
                if (rightObj != null) rightObj.GetComponent<Platform>().Disable(1);
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
        rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);
    }

    private void OnDrawGizmos() {
        Gizmos.color = grounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(bottomCenter, boxSize);

        Gizmos.color = roofed ? Color.green : Color.red;
        Gizmos.DrawWireCube(topCenter, boxSize);
    }
}