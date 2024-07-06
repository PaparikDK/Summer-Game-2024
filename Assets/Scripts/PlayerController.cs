using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Player Animation Settings")]
    public Animator animator;
    
    public float moveSpeed;
    public float jumpForce;
    public float acceleration;
    public float accelerationInterval = 30f; 

    private Rigidbody2D rb;
    private Collider2D myCollider;
    
    public bool isGrounded;
    public bool isDeath;
    public bool isDoubleJumped;
    public LayerMask whatIsGround;
    public LayerMask whatIsDeath;
    public GameObject deathScreen;

    public AudioSource backgroundAudio;
    public AudioSource gameOverAudio;

    public TMP_Text deathScreenCoinsText;
    public TMP_Text gameScreenCoinsText;
    private float timeSinceLastAcceleration; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        timeSinceLastAcceleration = 0f; // Инициализация таймера
    }

    // Update is called once per frame
    void Update()
    {
        // Обновляем таймер
        timeSinceLastAcceleration += Time.deltaTime;

        // Увеличиваем скорость игрока, если прошел интервал времени
        if (timeSinceLastAcceleration >= accelerationInterval && !isDeath)
        {
            moveSpeed += acceleration;
            timeSinceLastAcceleration = 0f; // Сбрасываем таймер
        }

        isDeath = Physics2D.IsTouchingLayers(myCollider, whatIsDeath);
        isGrounded = Physics2D.IsTouchingLayers(myCollider, whatIsGround);
        animator.SetFloat("HorizontalMove", Mathf.Abs(moveSpeed));
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        if (isDeath)
        {
            if (!deathScreen.activeSelf)
            {
                moveSpeed = 0;
                deathScreen.SetActive(true);

                backgroundAudio.Stop();
                gameOverAudio.Play();

                deathScreenCoinsText.text = "Монет: " + gameScreenCoinsText.text;
                gameScreenCoinsText.text = "";

                animator.SetBool("Jumping", false);
                animator.SetFloat("HorizontalMove", Mathf.Abs(0));
                animator.SetBool("Death", true); 
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (isGrounded && isDoubleJumped)
            {
                isDoubleJumped = false;
            }
            
            if (isGrounded && !isDoubleJumped)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isDoubleJumped = true;
            }

            if (!isGrounded && isDoubleJumped)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce / 1.2f);
                isDoubleJumped = false;
            }
        }

        if (isGrounded == false)
        {
            animator.SetBool("Jumping", true);
        }
        else
        {
            animator.SetBool("Jumping", false);
        }
    }
}