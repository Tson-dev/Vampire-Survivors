using System.Collections.Generic;
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
        instance = this;

        if (assignedWeapons == null)
            assignedWeapons = new List<Weapon>();

        if (unassignedWeapons == null)
            unassignedWeapons = new List<Weapon>();
    }

    
    void Start()
    {
        if (assignedWeapons.Count == 0 && unassignedWeapons.Count > 0)
        {
            AddWeapon(Random.Range(0, unassignedWeapons.Count));
        }

        if (PlayerStatController.instance != null)
        {
            if (PlayerStatController.instance.moveSpeed != null && PlayerStatController.instance.moveSpeed.Count > 0)
                moveSpeed = PlayerStatController.instance.moveSpeed[0].value;

            if (PlayerStatController.instance.pickupRange != null && PlayerStatController.instance.pickupRange.Count > 0)
                pickupRange = PlayerStatController.instance.pickupRange[0].value;

            if (PlayerStatController.instance.maxWeapons != null && PlayerStatController.instance.maxWeapons.Count > 0)
                maxWeapons = Mathf.RoundToInt(PlayerStatController.instance.maxWeapons[0].value);
        }
    }

    private void Update()
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

    public void AddWeapon(int weaponNumber)
    {
        if (unassignedWeapons == null || assignedWeapons == null)
            return;

        if (weaponNumber >= 0 && weaponNumber < unassignedWeapons.Count)
        {
            assignedWeapons.Add(unassignedWeapons[weaponNumber]);
            unassignedWeapons[weaponNumber].gameObject.SetActive(true);
            unassignedWeapons.RemoveAt(weaponNumber);
        }
    }
   
}