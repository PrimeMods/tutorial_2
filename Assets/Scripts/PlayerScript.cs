using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;
    public Text win;
    public Text lives;

    private int scoreValue = 0;
    private int livesValue = 3;
    public AudioSource musicSource;
    public AudioClip ambient;
    public AudioClip victory;
    Animator anim;
    private bool facingRight = true;
    private bool onGround;
    private bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        win.gameObject.SetActive(false);

        musicSource.clip = ambient;
        musicSource.Play();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");

        if (gameOver == false){
            rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        }

        if (facingRight == false && hozMovement > 0)
        {
             Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
             Flip();
        }

        if (vertMovement == 0)
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (gameOver == false){
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                anim.SetInteger("State", 1);
            }

            if (Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false && Input.GetKey(KeyCode.W) == false)
            {
                anim.SetInteger("State", 0);
            }

            if (!onGround || Input.GetKey(KeyCode.W))
            {
                anim.SetInteger("State", 2);
                Debug.Log(onGround);
            }
        }
        else
        {
            anim.SetInteger("State", 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            if (scoreValue == 4)
            {
                transform.position = new Vector3(46.0f, 0.0f, 0.0f);
                livesValue = 3;
                lives.text = "Lives: " + livesValue.ToString();
            }

            if (scoreValue >= 8)
            {
                win.gameObject.SetActive(true);

                musicSource.clip = victory;
                musicSource.Play();
                musicSource.loop = false;
                gameOver = true;
            }
        }

        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);

            if (livesValue <= 0)
            {
                win.text = "You Lose! :(";
                win.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse); //the 3 in this line of code is the player's "jumpforce," and you change that number to get different jump behaviors.  You can also create a public variable for it and then edit it in the inspector.
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}