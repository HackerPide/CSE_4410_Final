using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayerKey : MonoBehaviour
{
    public float speed = 4.5f;
    public float jumpForce = 10f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D box;

    private bool isGrounded;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = 0f;

        if (Input.GetKey(KeyCode.D))
        {
            deltaX = speed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            deltaX = -speed;
        }

        Vector2 movement = new Vector2(deltaX, body.linearVelocity.y);
        body.linearVelocity = movement;

        anim.SetFloat("speed", Mathf.Abs(deltaX));
        if (!Mathf.Approximately(deltaX, 0))
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX), 1, 1);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); //

        MovingPlatform platform = null;

        if (platform != null)
        {
            transform.parent = platform.transform;
        }
        else
        {
            transform.parent = null;
        }

        Vector3 playerScale = Vector3.one;
        if (platform != null)
        {
            playerScale = platform.transform.localScale;
        }
        if (!Mathf.Approximately(deltaX, 0))
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX) / playerScale.x, 1 / playerScale.y, 1);
        }

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            audioManager.PlaySFX(audioManager.jump);
        }
    }
}
