using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	[Header("Core")]
	public Rigidbody2D theRB;
	public float moveSpeed = 2f;
	private Transform target;

	[Header("Combat")]
	public float damage = 1f;
	public float hitWaitTime = 1f;
	private float hitCounter;

	[Header("Health")]
	public float health = 5f;

	[Header("Knockback")]
	public float knockBackTime = 0.5f;
	private float knockBackCounter;

	[Header("Drop")]
	public int expToGive = 1;
	public float expMultiplier = 1f;
	public int coinValue = 1;
	public float coinDropRate = 0.5f;

	[Header("Sprite Settings")]
	public bool faceLeftByDefault = false;

	private Vector2 moveDirection;

	private void Awake()
	{
		theRB = GetComponent<Rigidbody2D>();
		// default target: player health controller if available
		if (PlayerHealthController.instance != null)
			target = PlayerHealthController.instance.transform;
	}

	void FixedUpdate()
	{
		if (moveSpeed <= 0f) return;

		// ensure player controller exists; target validity checked below
		if (PlayerController.instance == null)
		{
			theRB.velocity = Vector2.zero;
			return;
		}

		if (target == null || !target.gameObject.activeSelf)
		{
			theRB.velocity = Vector2.zero;
			return;
		}

		// knockback handling
		if (knockBackCounter > 0f)
		{
			knockBackCounter -= Time.fixedDeltaTime;
			theRB.velocity = -moveDirection * moveSpeed * 2f;
			return;
		}

		// movement towards target
		moveDirection = (target.position - transform.position).normalized;
		theRB.velocity = moveDirection * moveSpeed;

		// flip sprite based on relative position
		Vector3 newScale = transform.localScale;
		float baseScaleX = Mathf.Abs(newScale.x);
		if (target.position.x > transform.position.x)
			newScale.x = baseScaleX * (faceLeftByDefault ? -1f : 1f);
		else if (target.position.x < transform.position.x)
			newScale.x = baseScaleX * (faceLeftByDefault ? 1f : -1f);
		transform.localScale = newScale;

		// hit cooldown
		if (hitCounter > 0f) hitCounter -= Time.fixedDeltaTime;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && hitCounter <= 0f)
		{
			if (PlayerHealthController.instance != null)
				PlayerHealthController.instance.TakeDamage(damage);

			hitCounter = hitWaitTime;
		}
	}

	public void TakeDamage(float damageToTake)
	{
		health -= damageToTake;

		if (health <= 0f)
		{
			Die();
			return;
		}

		if (SFXManager.instance != null)
			SFXManager.instance.PlaySFXPitched(1);

		if (DamageNumberController.instance != null)
			DamageNumberController.instance.SpawnDamage(damageToTake, transform.position);
	}

	public void TakeDamage(float damageToTake, bool shouldKnockBack)
	{
		TakeDamage(damageToTake);
		if (shouldKnockBack)
		{
			knockBackCounter = knockBackTime;
		}
	}

	private void Die()
	{
		// spawn exp and coin, play VFX/SFX, then destroy
		int expAmount = Mathf.Max(1, Mathf.RoundToInt(expToGive * expMultiplier));
		if (ExperienceLevelController.instance != null)
			ExperienceLevelController.instance.SpawnExp(transform.position, expAmount);

		if (Random.value <= coinDropRate && CoinController.instance != null)
			CoinController.instance.DropCoin(transform.position, coinValue);

		if (SFXManager.instance != null)
			SFXManager.instance.PlaySFXPitched(0);

		Destroy(gameObject);
	}

	public void SetTarget(GameObject newTarget)
	{
		if (newTarget == null) { target = null; return; }
		target = newTarget.transform;
	}
}