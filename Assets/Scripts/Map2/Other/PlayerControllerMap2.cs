using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerMap2 : MonoBehaviour
{
    public static PlayerControllerMap2 instance;

    [Header("References")]
    public SpriteRenderer spriteRenderer;
    public Animator anim;
    [Header("Movement")]
    public float moveSpeed;
    [Header("Pickup")]
    public float pickupRange = 1.5f;
    [Header("Weapons")]
    public List<Weapon> unassignedWeapons;
    public List<Weapon> assignedWeapons;
    public int maxWeapons = 3;
    [HideInInspector]
    public List<Weapon> fullyLevelledWeapons = new List<Weapon>();
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        instance = this;
        this.rb = GetComponent<Rigidbody2D>();

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

    private void FixedUpdate()
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
    public void AddWeapon(Weapon weaponToAdd)
    {
        if (weaponToAdd == null || unassignedWeapons == null || assignedWeapons == null)
            return;

        weaponToAdd.gameObject.SetActive(true);
        assignedWeapons.Add(weaponToAdd);
        unassignedWeapons.Remove(weaponToAdd);
    }
}