using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public Transform Player;
    public float MoveSpeed = 1.5f;
    public float rotationSpeed = 30f;
    private float rotSpeed;
    public float MaxDist = 8.0f;
    public float MinDist = 0.8f;

    private Animator _animator;
    private CapsuleCollider _collider;
    //private Rigidbody _rigidbody;
    public int atkCooldown;
    float lastAttack;
    float atkNr;
    int attackDamage;
    public int atk1damage;
    public int atk2damage;
    public int atk3damage;

    private bool canMove = true;
    private bool dead;

    public Transform attackPoint;
    public LayerMask playerLayer;
    public float attackRange = 0.5f;
    
    public int maxHealth = 100;
    int currentHealth;
    public HealthBarScript healthBar;
    public GameObject lootPrefab;
    public ParticleSystem hitPsPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider>();
        //_rigidbody = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        rotSpeed = rotationSpeed;
        lastAttack = -1 * atkCooldown;
        //_rigidbody.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        Look();

        if (dead == true || canMove == false)
        {
            return;
        }

        // transform.LookAt(new Vector3(Player.position.x, 0, Player.position.z));

        if (Vector3.Distance(transform.position, Player.position) >= MinDist && Vector3.Distance(transform.position, Player.position) <= MaxDist)
        {
            _animator.SetBool("Walking", true);
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        }
        else if (Vector3.Distance(transform.position, Player.position) >= MaxDist)
        {
            _animator.SetBool("Walking", false);
        }
        else if (Vector3.Distance(transform.position, Player.position) <= MinDist)
        {
            _animator.SetBool("Walking", false);
            
            if (Time.time - lastAttack >= atkCooldown)
            {
                atkNr = Random.Range(0f, 1f);
                if (atkNr <= 0.3f)
                {
                    attackDamage = atk2damage;
                    _animator.SetTrigger("Attack2");
                }
                else if (atkNr <= 0.6f)
                {
                    attackDamage = atk3damage;
                    _animator.SetTrigger("Attack3");
                }
                else
                {
                    attackDamage = atk1damage;
                    _animator.SetTrigger("Attack1");
                }
            
                lastAttack = Time.time;
            }
        }
    }

    private void Look()
    {
        if (rotSpeed < rotationSpeed && canMove == true)
        {
            rotSpeed += 0.1f;
        }
        Vector3 dir = Player.position - transform.position;
        Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir, Vector3.up), rotSpeed * Time.deltaTime);
        rot.x = 0;
        rot.z = 0;
        transform.rotation = rot;
    }

    public void CanMove()
    {
        canMove = true;
        /* Vector3 relativePos = Player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 1f);*/
    }

    public void CanNotMove()
    {
        canMove = false;
        rotSpeed = 0;
    }

    public void TakeDamage(int damage)
    {
        if (dead == true)
        {
            return;
        }

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        _animator.SetTrigger("Hit");
        PlayPs();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void PlayPs()
    {
        Vector3 psDir = transform.position - Player.position;
        Quaternion psRot = Quaternion.LookRotation(psDir, Vector3.up);
        psRot.x = 0;
        psRot.z = 0;
        ParticleSystem ps = Instantiate(hitPsPrefab, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), psRot);
        Destroy(ps.gameObject, 2f);
    }

    private void Die()
    {
        _animator.SetTrigger("Dead");
        dead = true;
        _collider.enabled = false;
    }

    private void DisableEnemy()
    {
        GameObject loot = Instantiate(lootPrefab, new Vector3(transform.position.x, 0.4f, transform.position.z), Quaternion.identity);

        this.enabled = false;
    }

    private void HitPlayer()
    {
        Collider[] hitPlayer = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);
        
        foreach (Collider player in hitPlayer)
        {
            player.GetComponent<PlayerCombatScript>().TakeDamage(attackDamage);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
