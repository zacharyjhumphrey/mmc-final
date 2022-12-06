using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public BoxCollider2D bc;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] public float runMaxSpeed;
    [SerializeField] public float runAcceleration;
    [SerializeField] public float runDecceleration;
    [SerializeField] public float runVelocityPower;
    [SerializeField] public float frictionCoefficient;
    [SerializeField] public float jumpForce;
    [SerializeField] public float coyoteTimeWindow;
    [SerializeField] public float jumpBufferWindow;

    [SerializeField] Transform groundCheckCollider;

    private float _coyoteTimeCounter;
    private float _jumpBufferCounter;

    #region CONTROLLER FLAGS
    private bool _jumpPressed = false;
    #endregion

    public Rigidbody2D rb { get; private set; }

    void Start() {
        print("start");
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        #region CONTROLLER
        if (Input.GetKeyDown(KeyCode.Space)) {
            _jumpPressed = true;
        }
        #endregion
    }

    void FixedUpdate() {
        #region ADD SPEED FORCE
        float targetSpeed = Input.GetAxisRaw("Horizontal") * runMaxSpeed;
        float speedDif = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAcceleration : runDecceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, runVelocityPower) * Mathf.Sign(speedDif);
        rb.AddForce(movement * Vector2.right);
        #endregion

        #region FRICTION
        //if (isGrounded() && Input.GetAxisRaw("Horizontal") == 0) {
        //    float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionCoefficient));
        //    amount *= Mathf.Sign(rb.velocity.x);
        //    rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        //}
        #endregion

        handleJump();
    }

    // TODO falling onto the ground while holding space feels bad
    private void handleJump() {
        _coyoteTimeCounter = (isGrounded()) ? coyoteTimeWindow : (_coyoteTimeCounter - Time.deltaTime);
        _jumpBufferCounter -= Time.deltaTime;

        if (_jumpBufferCounter > 0f && isGrounded()) {
            jump();
            return;
        }

        if (!_jumpPressed) {
            return;
        }

        _jumpBufferCounter = jumpBufferWindow;

        if (canJump()) {
            jump();
            return;
        }
    }

    private void jump() {
        //rb.AddRelativeForce()
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        _jumpPressed = false;
        _coyoteTimeCounter = 0f;
        _jumpBufferCounter = 0f;
    }

    private bool canJump() {
        return isGrounded() || _coyoteTimeCounter > 0f;
    }

    private bool isGrounded() {
        // TODO need to move this up some
        RaycastHit2D raycastHit = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
}
