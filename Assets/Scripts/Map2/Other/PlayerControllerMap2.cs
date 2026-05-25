using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerMap2 : BasePlayerController
{
    private Rigidbody2D rb;
    private Vector2 moveInput;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        spriteRenderer.flipX = moveInput.x < 0;

        anim.SetBool("isMoving", moveInput != Vector2.zero);
    }

    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }
}