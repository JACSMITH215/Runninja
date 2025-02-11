using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAxisMovement : MonoBehaviour
{
    //Movement Variables
    public float speed;
    public float jumpForce;
    public float gravityRegular;
    public float gravityOnWall;
    private Rigidbody2D rb2d;
    private float axisH = 1f;
    private float axisV;
    private SpriteRenderer spr;
    private Animator animator;

    public bool isGrounded;
    public LayerMask layer;
    public LayerMask layerWall;

    private Transform groundcheck;
    private Transform wallcheck1;
    private Transform wallcheck2;

    public bool isOnWall = false;

    //Jump Variables
    public AudioClip jumpSound;

    //HP Variables
    public float hurtTimer;
    private bool gameOver = false;

    private const float immuneFrames = 2;
    public AudioClip hurtSound;

    //Attack
    public Collider2D sliceCollider;
    [SerializeField] private Rigidbody2D shurikenObject;
    public AudioClip throwSound;
    public float attackTimer;
    [SerializeField] private Rigidbody2D finalSlashObject;
    public AudioClip finalSlashSound;

    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        groundcheck = GameObject.Find("GroundCheck").GetComponent<Transform>();
        wallcheck1 = GameObject.Find("WallCheck1").GetComponent<Transform>();
        wallcheck2 = GameObject.Find("WallCheck2").GetComponent<Transform>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        /*rb2d.gravityScale = 3;
        axisH = Input.GetAxisRaw("Horizontal");
        rb2d.velocity = new Vector2(axisH*speed, rb2d.velocity.y);*/

        isGrounded = Physics2D.Linecast(transform.position, groundcheck.position, layer);
        isOnWall = Physics2D.Linecast(transform.position, wallcheck1.position, layerWall) || Physics2D.Linecast(transform.position, wallcheck2.position, layerWall);

        //immune visuals
        if(hurtTimer>0){
            float rate = 30f;
            spr.color = new Color(1f,1f,1f,((int)(hurtTimer*60)%(int)rate)/rate);
        }else{
            spr.color = new Color(1f,1f,1f,1f);
        }

        if(canMove){
            if(hurtTimer<=0 && attackTimer <= 0){
                if(isOnWall){
                    rb2d.gravityScale = gravityOnWall;
                    axisV = Input.GetAxis("Vertical");

                    //animator.SetFloat("animationSpeed", Math.Sign(rb2d.velocity.y));

                    if(Input.GetKeyDown(KeyCode.Space)){
                        axisH *= -1;
                        rb2d.velocity = new Vector2(axisH*speed, 0);
                        rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                        isOnWall = false;
                    }
                }else{
                    rb2d.gravityScale = gravityRegular;
                    rb2d.velocity = new Vector2(axisH*speed, rb2d.velocity.y);

                    if(Input.GetKeyDown(KeyCode.Z)){
                        if(Input.GetAxisRaw("Vertical") < 0){
                            SliceAttack();
                        }else{
                            ShurikenAttack();
                        }
                    }
                }

                if(axisH>0){
                    spr.flipX = false;
                }else if(axisH<0){
                    spr.flipX = true;
                }
            }

            //animator.SetFloat("Speed", Math.Abs(rb2d.velocity.x));
            //animator.SetBool("isOnWall", isOnWall);

            if(isGrounded && Input.GetKeyDown(KeyCode.Space) && hurtTimer<=0){
                Jump();
                SoundFXManager.instance.PlaySoundFXClip(jumpSound, transform, 1f);
            }

            if(Input.GetKeyUp(KeyCode.Space)){
                if(rb2d.velocity.y>0.01f){
                    rb2d.velocity = new Vector2(rb2d.velocity.x,rb2d.velocity.y*0.5f);
                }
            }
        }

        //animator.SetFloat("YSpeed", rb2d.velocity.y);

        if(hurtTimer>0){
            animator.SetBool("hurt", true);
        }else{
            animator.SetBool("hurt", false);
        }

        if(hurtTimer>0){
            hurtTimer -= Time.deltaTime;
        }

        if(gameOver && hurtTimer <= 1f){
            ResetPosition();
        }

        if(attackTimer>0){
            attackTimer -= Time.deltaTime;
        }

        if(attackTimer<=0){
            animator.SetBool("throw", false);
            animator.SetBool("slice", false);
            sliceCollider.GetComponent<SliceScript>().canHit = false;
        }

        animator.SetBool("grounded", isGrounded);
        animator.SetBool("onWall", isOnWall);

        if(sliceCollider != null){
            sliceCollider.transform.position = transform.position;
        }

        CheckGameOver();
    }

    public void Jump(){
        rb2d.velocity = new Vector2(axisH*speed, 0);
        rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    private void ShurikenAttack(){
        animator.SetBool("throw", true);
        Rigidbody2D shuriken = Instantiate(shurikenObject, transform.position, Quaternion.identity);
        shuriken.velocity = new Vector2(3*axisH*speed, 0);
        SoundFXManager.instance.PlaySoundFXClip(throwSound, transform, 1f);
        attackTimer = 0.2f;
    }

    private void SliceAttack(){
        animator.SetBool("slice", true);
        sliceCollider.GetComponent<SliceScript>().canHit = true;
        attackTimer = 0.4f;
    }

    public void Hurt(){
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(new Vector2((spr.flipX?1:-1)*speed, speed),ForceMode2D.Impulse);
        hurtTimer = immuneFrames;
        attackTimer = 0;

        SoundFXManager.instance.PlaySoundFXClip(hurtSound, transform, 1f);
        gameOver = true;
    }

    private void CheckGameOver(){
        if(transform.position.y < -10f && !gameOver){
            Hurt();
        }
    }

    private void ResetPosition(){
        /*rb2d.velocity = Vector2.zero;
        hurtTimer = 0;
        transform.position = new Vector3(respawnPoint.x, respawnPoint.y, transform.position.z);*/
        SceneManager.LoadScene("Level1");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Enemy")){
            if(other.GetComponent<SamuraiScript>().hurtTimer <= 0){
                other.GetComponent<SamuraiScript>().Attack();
                Hurt();
            }
        }
        if(other.gameObject.CompareTag("Boss")){
            if(!other.GetComponent<BossScript>().defeated){
                animator.SetBool("throw", true);
                attackTimer = 0.2f;
                SoundFXManager.instance.PlaySoundFXClip(finalSlashSound, transform, 1f);
                Instantiate(finalSlashObject, other.transform.position, Quaternion.identity);
                other.GetComponent<BossScript>().Defeat();
            }
        }
        if(other.gameObject.CompareTag("Goal")){
            hurtTimer = 0;
            attackTimer = 0;
            canMove = false;
        }
    }
}
