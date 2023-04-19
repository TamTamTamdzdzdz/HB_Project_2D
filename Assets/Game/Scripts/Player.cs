using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField]private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed=200;
    [SerializeField] private float JumpForce = 350;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform kunaiPoint;
    private bool isGrounded=true,  isAttack=false;
    private bool isJumping=false;
    private float horizontal,vertical;
    //private bool isDead=false;
    private Vector3 savePoint;
    // Start is called before the first frame update
    void Start()
    {
        SavePoint(); 
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (isDead)
            return;
        isGrounded=CheckGrounded();
        //-1->0->1
        horizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");

        if(isAttack )
        {
            rb.velocity=Vector2.zero;
            return;
        }

        if(isGrounded)
        {
            if (isJumping)
                return;
            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                Jump();
            }


            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }
            if (Input.GetKey(KeyCode.C) && isGrounded)

                Attack();
            if (Input.GetKey(KeyCode.V) && isGrounded)

                Throw();


        }

        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }

        if (Input.GetKey(KeyCode.Space)&&isGrounded)
        {
            //isJumping = true;

            ChangeAnim("jump");
            rb.AddForce(JumpForce * Vector2.up);
        }
        if(!isGrounded&&rb.velocity.y < 0)
        {
            ChangeAnim("fall");
        }

        if (Mathf.Abs( horizontal) > 0.1f)
        {
            ChangeAnim("run");
            rb.velocity=new Vector2(horizontal*Time.fixedDeltaTime*speed,rb.velocity.y);
            transform.rotation= Quaternion.Euler(new Vector3(0,horizontal>0?0:180,0)) ;
        }
        else if(isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity=Vector2.zero;
        }

    }
    public override void OnInit()
    {
        base.OnInit();
        //isDead=false;
        isAttack=false;
        transform.position = savePoint;
        ChangeAnim("idle");
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
    }
    protected override void OnDeath()
    {
        base.OnDeath();
    }
    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        return hit.collider != null;
    }
    private void Attack()
    {
        ChangeAnim("attack");
        isAttack=true;

        Invoke(nameof(ResetAttack), 0.5f);
    }
    private void Throw()
    {

        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        Instantiate(kunaiPrefab, kunaiPoint.position, kunaiPoint.rotation);

    }
    private void ResetAttack()
    {
         ChangeAnim("ilde");
        isAttack = false;
       
    }
    private void Jump()
    {
        //isJumping = true;
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(JumpForce * Vector2.up);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            Destroy(collision.gameObject);
        }
        if (collision.tag == "DeathZOne")
        {
            //isDead=true; 
            ChangeAnim("die");
            Invoke(nameof(OnInit), 1f);
        }
    }
    internal void SavePoint()
    {
        savePoint=transform.position;
    }

}
