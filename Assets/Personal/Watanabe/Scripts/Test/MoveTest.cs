using UnityEngine;

/// <summary>
/// Delayを確認するためのもの
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MoveTest : MonoBehaviour
{
    [Header("PlayerStatus一覧")]
    [SerializeField] private float _moveSpeed = 1f;

    private Rigidbody2D _rb = default;
    private float _input = 0;

    private void Start()
    {
        //コンポーネントの取得
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Movement();
    }

    private void FixedUpdate()
    {
        //物理演算はFixedupdate()で行う
        _rb.velocity = new Vector2(_input * _moveSpeed, _rb.velocity.y);
    }

    private void Movement()
    {
        var hol = Input.GetAxisRaw("Horizontal");

        //横方向の入力を反映
        _input = hol;
    }
}
