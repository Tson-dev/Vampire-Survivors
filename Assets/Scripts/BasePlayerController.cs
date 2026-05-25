using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerController : MonoBehaviour
{
    public static BasePlayerController instance;

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

    protected virtual void Awake()
    {
        instance = this;
    }

    protected virtual void Start()
    {
        if (assignedWeapons.Count == 0)
        {
            AddWeapon(Random.Range(0, unassignedWeapons.Count));
        }

        moveSpeed = PlayerStatController.instance.moveSpeed[0].value;
        pickupRange = PlayerStatController.instance.pickupRange[0].value;
        maxWeapons = Mathf.RoundToInt(PlayerStatController.instance.maxWeapons[0].value);
    }

    public virtual void AddWeapon(int weaponNumber)
    {
        if (weaponNumber < unassignedWeapons.Count)
        {
            assignedWeapons.Add(unassignedWeapons[weaponNumber]);

            unassignedWeapons[weaponNumber].gameObject.SetActive(true);

            unassignedWeapons.RemoveAt(weaponNumber);
        }
    }

    public virtual void AddWeapon(Weapon weaponToAdd)
    {
        weaponToAdd.gameObject.SetActive(true);

        assignedWeapons.Add(weaponToAdd);

        unassignedWeapons.Remove(weaponToAdd);
    }
}