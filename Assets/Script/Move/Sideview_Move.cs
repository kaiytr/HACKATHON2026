// Player에게 Groundcheck emptyobject 상속시켜서 발 밑쪽에 놓기, Rigidbody2D, Collider설정
// 땅 오브젝트한테 Collider, Layer 'Ground' 설정하기 (없으면 만들기)
// Player Inspector 설정: Groundcheck -> Player에 상속시킨 emptyobject, Groundlayer -> 'Ground'
// Player Rigidbody Z축 Freeze 필요

using UnityEngine;

public class Sideview_Move : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 8f;
    
    [Header("점프 설정")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private Transform groundCheck; // 캐릭터 발 밑에 위치할 빈 오브젝트 (Grounded 판정용)
    [SerializeField] private LayerMask groundLayer;   // 땅으로 인식할 레이어 설정
    
    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;
    private bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // WASD 또는 방향키 입력 받기 (A/D, 좌/우 화살표)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 바닥에 닿아있는지 확인 (지름 0.2f의 원을 그려 바닥 레이어와 충돌하는지 체크)
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Space바를 누르고 바닥에 있을 때 점프 실행 (W키로도 점프하게 하려면 Input.GetKeyDown(KeyCode.W) 추가 가능)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // 캐릭터의 시선 방향 전환
        Flip();
    }

    void FixedUpdate()
    {
        // 물리적인 수평 이동 적용 (FixedUpdate에서 처리하여 물리 연산을 안정적으로 유지)
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Flip()
    {
        // 입력 방향에 따라 캐릭터의 이미지를 좌우로 뒤집음
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // 에디터 뷰에서 GroundCheck 범위를 시각적으로 확인하기 위함
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }
}