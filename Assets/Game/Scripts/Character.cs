using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private float hp;
    private string currentAnim;
    [SerializeField] public Animator animator;

    public bool isDead => hp <= 0;

    private void Start()
    {
        OnInit();
        
    }

    public virtual void OnInit()
    {
        hp = 100;
    }
    public virtual void OnDespawn()
    {
        
    }
    protected virtual void OnDeath()
    {

    }
    protected void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            animator.ResetTrigger(animName);
            currentAnim = animName;
            animator.SetTrigger(currentAnim);
        }
    }

    public void OnHit(float damage)
    {
        if(!isDead)
            OnDeath();
        if (isDead)
        {
            hp -= damage;
        }
    }
   

}
