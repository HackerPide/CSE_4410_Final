using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed to reload scene

public class PlatformerPlayerArrow : MonoBehaviour
{
    public float speed = 4.5f;
    public float jumpForce = 10f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D box;
    private Vector3 platformVelocity = Vector3.zero;
    private Rigidbody2D platformBody = null;

    private bool isGrounded;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            platformBody = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            platformBody = null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hazard"))
        {
            StartCoroutine(RestartAfterDelay());
        }
    }

    IEnumerator RestartAfterDelay()
    {
        audioManager.PlaySFX(audioManager.death);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

        if (Input.GetKey(KeyCode.RightArrow))
        {
            deltaX = speed;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            deltaX = -speed;
        }

        Vector2 baseVelocity = new Vector2(deltaX, body.linearVelocity.y);

        if (platformBody != null)
        {
            baseVelocity += platformBody.linearVelocity;
        }

        body.linearVelocity = baseVelocity;

        anim.SetFloat("speed", Mathf.Abs(deltaX));
        if (!Mathf.Approximately(deltaX, 0))
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX), 1, 1);
        }

        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); //

        Vector3 max = box.bounds.max;
        Vector3 min = box.bounds.min;
        Vector2 corner1 = new Vector2(max.x, min.y - .1f);
        Vector2 corner2 = new Vector2(min.x, min.y - .2f);
        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        isGrounded = false;
        if (hit != null) {
            isGrounded = true;
        }

        body.gravityScale = (isGrounded && Mathf.Approximately(deltaX, 0)) ? 0 : 1;
        if (isGrounded && Input.GetKeyDown(KeyCode.UpArrow)) {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        MovingPlatform platform = null;
        if (hit != null) {
            platform = hit.GetComponent<MovingPlatform>();
        }
        if (platform != null) {
            transform.parent = platform.transform;
        }
        else {
            transform.parent = null;
        }

        anim.SetFloat("speed", Mathf.Abs(deltaX));

        Vector3 pScale = Vector3.one;
        if (platform != null) {
            pScale = platform.transform.localScale;
        }
        if (!Mathf.Approximately(deltaX, 0)) {
            transform.localScale = new Vector3(Mathf.Sign(deltaX) / pScale.x, 1 / pScale.y, 1);
        }

        /*
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            audioManager.PlaySFX(audioManager.jump);
        }
        */
    }
}

