using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5.0f;

    [Header("점프 설정")]
    public float jumpForce = 10.0f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded = false;
    private int score = 0;

    private Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        startPosition = transform.position;
        Debug.Log("시작 위치 저장: " + startPosition);
    }

    void Update()
    {
        float moveX = 0f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;

        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        if (moveX != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveX), 1, 1);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }

        float currentSpeed = Mathf.Abs(rb.linearVelocity.x);
        animator.SetFloat("Speed", currentSpeed);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 코인 수집
        if (other.CompareTag("Coin"))
        {
            score += 1;
            Debug.Log("💰 코인 획득! 현재 점수: " + score);
            Destroy(other.gameObject);
        }

        // 별 수집
        if (other.CompareTag("Star"))
        {
            score += 5;
            Debug.Log("⭐ 별 획득! +5점! 현재 점수: " + score);
            Destroy(other.gameObject);
        }

        // 골 도달 - 새로 추가!
        if (other.CompareTag("Goal"))
        {
            Debug.Log("🎉🎉🎉 게임 클리어! 🎉🎉🎉");
            Debug.Log("최종 점수: " + score + "점");

            // 캐릭터 조작 비활성화
            enabled = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("⚠️ 장애물 충돌! 시작 지점으로 돌아갑니다.");
            transform.position = startPosition;
            rb.linearVelocity = Vector2.zero;
        }
    }
}
