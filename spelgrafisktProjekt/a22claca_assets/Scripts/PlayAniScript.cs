using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAniScript : MonoBehaviour
{
    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void PlayWalkAni()
    {
        _animator.SetBool("Walking", true);
    }

    private void StopWalkAni()
    {
        _animator.SetBool("Walking", false);
    }
    
    private void PlayRunAni()
    {
        _animator.SetBool("Running", true);
    }

    private void StopRunAni()
    {
        _animator.SetBool("Running", false);
    }
    
    private void PlayAttack1Ani()
    {
        _animator.SetTrigger("Attack1");
    }

    private void PlayAttack2Ani()
    {
        _animator.SetTrigger("Attack2");
    }

    private void PlayAttack3Ani()
    {
        _animator.SetTrigger("Attack3");
    }

    private void PlayDeathAni()
    {
        _animator.SetTrigger("Death");
    }
    
    private void PlayHitAni()
    {
        _animator.SetTrigger("Hit");
    }
    
    private void PlayInteractAni()
    {
        _animator.SetTrigger("Interact");
    }
}
