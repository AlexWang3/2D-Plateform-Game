using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    public float jumpForce;
    private float moveInput;

    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;


    private Animator anim;
    private bool isFacingRight = true;

    private bool isAttacking = false;
    private float attackTimeCounter;
    private float attackTime = 0.15f;
    private bool keepAttacking = false;
    private int attackingPartern = 1;
    public int totalPartern;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public int damage;
    private bool damageOutputAvaliable = false;

    private bool movingInputAvaliable = true;
    private bool attackingInputAvaliable = true;

    void Start()
    {

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        attackTime = (attackTime * 10.0f) / 7.0f;

    }

    
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isAttacking)
        {
            Attack();
        }
        
        if (isGrounded && Input.GetKeyDown(KeyCode.K) && movingInputAvaliable)
        {
            anim.SetTrigger("TakeOff");
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        if (isGrounded)
        {
            anim.SetBool("IsJumping", false);
            if (Input.GetKeyDown(KeyCode.J) && attackingInputAvaliable)
            {
                if (attackingPartern != totalPartern)
                {
                    attackingPartern++;
                } else
                {
                    attackingPartern = 1;
                }
                anim.SetInteger("AttackingPatern", attackingPartern);
                if (!isAttacking )
                {
                    anim.SetBool("Attacking", true);
                    isAttacking = true;
                    attackTimeCounter = attackTime;
                } else
                {
                    keepAttacking = true;
                }
                damageOutputAvaliable = true;

            }
        }
        else
        {
            anim.SetBool("IsJumping", true);
        }
        if (Input.GetKey(KeyCode.K) && isJumping && movingInputAvaliable)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            } else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            isJumping = false;
        }
    }

    private void FixedUpdate()
    {
        if (movingInputAvaliable)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            moveInput = 0;
        }
        if ((moveInput > 0 && !isFacingRight) || (moveInput < 0 && isFacingRight))
        {
            Flip();
        }
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (moveInput == 0)
        {
            anim.SetBool ("IsWalking", false);
        }
        else
        {
            anim.SetBool ("IsWalking", true);
        }
    }

    protected void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        // Flip collider over the x-axis
        //center.x = -center.x;
    }

    protected void Attack()
    {
        if (attackTimeCounter > 0)
        {
            attackTimeCounter -= Time.deltaTime;
            movingInputAvaliable = false;
        } 
        else if (keepAttacking)
        {
            attackTimeCounter = attackTime;
            movingInputAvaliable = false;
            keepAttacking = false;
        }
        else
        {
            isAttacking = false;
            movingInputAvaliable = true;
            anim.SetBool("Attacking", false);
        }

        if (attackTimeCounter < 0.7 * attackTime && attackTimeCounter < 0.6 * attackTime && damageOutputAvaliable)
        {
            doDamageOutput();
            damageOutputAvaliable = false;
        }
        if (attackTimeCounter > 0.2 * attackTime)
        {
            attackingInputAvaliable = false;
        }
        else
        {
            attackingInputAvaliable = true;
        }
    }

    protected void doDamageOutput()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<Enermy>().TakeDamage(damage);
        }
    }
}
