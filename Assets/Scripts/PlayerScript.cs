using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    public Text winText;
    public Text lives;
    private int scoreValue = 0;
    private int livesValue = 3;
    private bool isOnGround;
    private bool isJumping;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    public float jumpForce;
    public Text hozText;
    public Text jumpText;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;
    Animator anim;
    private bool facingRight = true;
    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        winText.text = "";
        lives.text = livesValue.ToString();
        musicSource.clip = musicClipOne;
        musicSource.loop = true;
        musicSource.Play();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        

        if(Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (livesValue <=0)
        {
            winText.text = "Uh-oh\nYou Lose!";
            Destroy(this);
        }
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }

    void Update()
    {
        
        if(Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0)
        {
            anim.SetInteger("State", 1);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            anim.SetInteger("State", 2);
        }
        else
            anim.SetInteger("State", 0);
        
    }
    void Flip()
    {
        facingRight =!facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            Destroy(collision.collider.gameObject);
            scoreValue = scoreValue + 1;
            SetScoreValue();
        }
        if (collision.collider.tag == "Enemy")
        {
            Destroy(collision.collider.gameObject);
            livesValue -= 1;
            lives.text = livesValue.ToString();
        }
        
    }

    void SetScoreValue ()
    {
        score.text = scoreValue.ToString();
        if (scoreValue == 4)
        {
            transform.position = new Vector3(82f, 1f, 0f);
            livesValue = 3;
            lives.text = livesValue.ToString();
        }
        if (scoreValue >=8)
        {
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            musicSource.loop = false;
            winText.text = "You Win!\nA Game by Stephanie Davis";
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground" && isOnGround)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }
}
