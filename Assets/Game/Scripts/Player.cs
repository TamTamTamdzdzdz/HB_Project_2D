using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField]private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed=5;
    [SerializeField] private float JumpForce = 500;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform kunaiPoint;
    [SerializeField] private GameObject attackArea;
    private bool isGrounded=true,  isAttack=false;
    private bool isJumping=false;
    private float horizontal,vertical;
    private int coin = 0;
    //private bool isDead = false;
    private Vector3 savePoint;
    // Start is called before the first frame update


    // Update is called once per frame
    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);

    }
    void Update()
    {

        if (isDead)
            return;
        isGrounded=CheckGrounded();
        //-1->0->1
       
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
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
               
            }


            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }
            if (Input.GetKeyDown(KeyCode.C) && isGrounded)

                Attack();
            if (Input.GetKeyDown(KeyCode.V) && isGrounded)

                Throw();


        }

        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }

       
       

        if (Mathf.Abs( horizontal) > 0.1f)
        {
           
            rb.velocity=new Vector2(horizontal*speed ,rb.velocity.y);
            transform.rotation= Quaternion.Euler(new Vector3(0,horizontal>0?0:180,0)) ;
        }
        else if(isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.up * rb.velocity.y;
        }

    }
    public override void OnInit()
    {
        base.OnInit();
        //isDead=false;
        isAttack=false;
        transform.position = savePoint;
        ChangeAnim("idle");
        SavePoint();
        DeActiveAttack();
        UIManager.instance.SetCoin(coin);
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
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
   public void Attack()
    {
        ChangeAnim("attack");
        isAttack=true;

        Invoke(nameof(ResetAttack), 0.5f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }
    public void Throw()
    {

        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        Instantiate(kunaiPrefab, kunaiPoint.position, kunaiPoint.rotation);

    }
    public void ResetAttack()
    {
         ChangeAnim("ilde");
        isAttack = false;
       
    }
    public  void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(JumpForce * Vector2.up);
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            coin++;
            PlayerPrefs.SetInt("coin", coin);
            UIManager.instance.SetCoin(coin);
            Destroy(collision.gameObject);
        }
        if (collision.tag == "DeathZOne")
        {
            //isDead=true; 
            ChangeAnim("die");
            Invoke(nameof(OnInit), 1f);
        }
       
    }
    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }
    internal void SavePoint()
    {
        savePoint=transform.position;
    }

}
