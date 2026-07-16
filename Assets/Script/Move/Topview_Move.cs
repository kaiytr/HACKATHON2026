using UnityEngine;

public class Topview_Move : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 moveVelocity;

    void Start()
    {
        // Rigidbody2D 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();

        // 물리 충돌 시 캐릭터가 물체처럼 굴러가는 것을 방지
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 탑뷰 게임이므로 중력이 영향을 주지 않도록 설정
        rb.gravityScale = 0f;
    }

    void Update()
    {
        // 매 프레임마다 키보드 입력 감지 (Update에서 처리)
        // A/D, W/S, 방향키 사용
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // 대각선 이동 시 속도가 과도하게 빨라지는 것을 방지하기 위해 정규화(Normalize) 처리
        // 입력이 없을 때(0,0)는 연산이 무의미하므로 키 입력이 있을 때만 처리
        if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();

        // 이동 속도 계산
        moveVelocity = moveInput * moveSpeed;
    }

    void FixedUpdate()
    {
        // 물리 업데이트 주기(FixedUpdate)에 맞춰 캐릭터 이동 처리
        // MovePosition을 사용하면 충돌 처리가 더 부드러움
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }
}
