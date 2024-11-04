using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BombBehaviour : MonoBehaviour
{
    public int damage;
    public float damageRange;
    public LayerMask enemyLayers;
    public VisualEffect vfxPrefab;

    void OnCollisionEnter(Collision collision)
    {
        HitEnemies();
        PlayVFX();
        Destroy(gameObject);
    }

    private void PlayVFX()
    {
        VisualEffect vfx = Instantiate(vfxPrefab, transform.position, transform.rotation);
        vfx.Play();
        Destroy(vfx.gameObject, 2f);
    }

    private void HitEnemies()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, damageRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyScript>().TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, damageRange);
    }
}
