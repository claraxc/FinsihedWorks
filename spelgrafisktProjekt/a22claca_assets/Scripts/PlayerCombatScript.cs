using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PlayerCombatScript : MonoBehaviour
{
    private ThirdPersonController movementScript;
    private Animator _animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    private float lastAttack;
    
    public float attackRange = 0.5f;
    private int attackDamage;
    public int attack1damage;
    public int attack2damage;
    public int attack3damage;

    private bool canCombo2 = false;
    private bool canCombo3 = false;
    private bool isAttacking = false;

    public int maxHealth = 200;
    int currentHealth;
    public HealthBarScript healthBar;
    
    public GameObject bomb;
    public Transform bombRoot;
    public float throwCooldown;
    public float throwForce;
    public float throwUpwardForce;
    private bool readyToThrow = true;

    public float dashSpeed;
    public float dashTime;
    public float dashCooldown;
    private bool canDash = true;

    public GameObject plant = null;
    public GameObject waterinCan = null;
    public GameObject scythe = null;
    public GameObject scythePlant = null;
    private bool scytheHarvesting = false;

    public GameObject PlantsAlpha1 = null;
    public GameObject PlantsAlpha2 = null;
    public GameObject PlantsAlpha3 = null;

    // Start is called before the first frame update
    void Start()
    {
        movementScript = GetComponent<ThirdPersonController>();
        _animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        PlantsAlpha1.SetActive(true);
        PlantsAlpha2.SetActive(false);
        PlantsAlpha3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking == false && Input.GetKeyDown(KeyCode.Mouse0))
        {
            isAttacking = true;

            if (canCombo2 == true)
            {
                Debug.Log("combo2");
                canCombo2 = false;
                canCombo3 = false;
                Attack2();
            }
            else if (canCombo3 == true)
            {
                Debug.Log("combo3");
                canCombo2 = false;
                canCombo3 = false;
                Attack3();
            }
            else
            {
                canCombo2 = false;
                canCombo3 = false;
                Debug.Log("combo1");
                Attack1();
            }
        }

        if (movementScript.enabled == true)
        {
            isAttacking = false;
        }

        if (Input.GetKeyDown(KeyCode.Q) && readyToThrow == true)
        {
            readyToThrow = false;
            movementScript.enabled = false;

            _animator.SetTrigger("ThrowBomb");

            Invoke(nameof(ResetThrow), throwCooldown);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && canDash == true)
        {
            movementScript.enabled = true;
            _animator.SetTrigger("Dash");
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlantsAlpha1.SetActive(true);
            PlantsAlpha2.SetActive(false);
            PlantsAlpha3.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlantsAlpha1.SetActive(false);
            PlantsAlpha2.SetActive(true);
            PlantsAlpha3.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlantsAlpha1.SetActive(false);
            PlantsAlpha2.SetActive(false);
            PlantsAlpha3.SetActive(true);
        }
    }

    private void CanNotMove()
    {
        movementScript.enabled = false;
    }

    private void CanMove()
    {
        movementScript.enabled = true;
    }

    private void StartCombo2Window()
    {
        canCombo2 = true;
    }

    private void StopCombo2Window()
    {
        canCombo2 = false;
    }

    private void StartCombo3Window()
    {
        canCombo3 = true;
    }

    private void StopCombo3Window()
    {
        canCombo3 = false;
    }

    private void AttackCooldownStart()
    {
        isAttacking = true;
        movementScript.enabled = false;
    }

    private void AttackCooldownEnd()
    {
        isAttacking = false;
        movementScript.enabled = true;
    }

    IEnumerator Dash()
    {
        canDash = false;
        float startTime = Time.time;

        // change to AddForce eller velocity
        while (Time.time < startTime + dashTime)
        {
            transform.Translate(Vector3.forward * dashSpeed);

            yield return null;
        }

        Invoke(nameof(ResetDash), dashCooldown);
    }
    
    private void ResetDash()
    {
        canDash = true;
        UnigonreEnemyCollisions();
    }

    private void IgnoreEnemyCollisions()
    {
        Physics.IgnoreLayerCollision(7, 8, true);
    }

    private void UnigonreEnemyCollisions()
    {
        Physics.IgnoreLayerCollision(7, 8, false);
    }

    private void ThrowBomb()
    {
        GameObject projectile = Instantiate(bomb, bombRoot.position, bombRoot.rotation);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 force = bombRoot.transform.forward * throwForce + transform.up * throwUpwardForce;
        projectileRb.AddForce(force, ForceMode.Impulse);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    private void ShowWateringCan()
    {
        waterinCan.SetActive(true);
    }

    private void HideWateringCan()
    {
        waterinCan.SetActive(false);
    }

    private void Harvest()
    {
        if (scytheHarvesting == false)
        {
            plant.SetActive(false);
        }
        else if (scytheHarvesting == true)
        {
            scythePlant.SetActive(false);
            scythe.SetActive(true);
        }
    }

    public void ScytheHarvesting(Transform target)
    {
        scytheHarvesting = true;
        Harvesting(target);
    }

    public void Watering(Transform target)
    {
        transform.LookAt(new Vector3(target.position.x, this.transform.position.y, target.position.z));

        _animator.SetTrigger("Watering");
    }

    public void Harvesting(Transform target)
    {
        transform.LookAt(new Vector3(target.position.x, this.transform.position.y, target.position.z));

        _animator.SetTrigger("Harvesting");
    }

    public void WaterTank(Transform target)
    {
        transform.LookAt(new Vector3(target.position.x, this.transform.position.y, target.position.z));

        _animator.SetTrigger("WaterTank");
    }

    public void Crafting(Transform target)
    {
        transform.LookAt(new Vector3(target.position.x, this.transform.position.y, target.position.z));

        _animator.SetTrigger("Crafting");
    }

    private void Attack1()
    {
        _animator.SetTrigger("Attack1");
        attackDamage = attack1damage;
    }

    private void Attack2()
    {
        _animator.SetTrigger("Attack2");
        attackDamage = attack2damage;
    }

    private void Attack3()
    {
        _animator.SetTrigger("Attack3");
        attackDamage = attack3damage;
    }

    private void HitEnemies()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyScript>().TakeDamage(attackDamage);
        }
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
