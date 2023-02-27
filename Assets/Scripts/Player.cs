using UnityEngine;


public class Player : Character
{

	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private LayerMask groundLayer; // Layer that raycast interact with
	[SerializeField] private float speed = 300;
	[SerializeField] private float jumpForce = 350f;
	[SerializeField] private Kunai kunaiPrefab;
	[SerializeField] private Transform throwPosition;
	[SerializeField] private GameObject attackArea;
	
	
	private bool isGrounded;

	private bool isJumping;

	private bool isAttacking;

	private bool isDead;

	private float horizontal;

	private int coin = 0;

	private Vector3 savePoint;
	
	private void Awake()
	{

		coin = PlayerPrefs.GetInt("coin", 0);
	}

	
	
	public override void OnInit()
	{
		base.OnInit();
		
		isDead = false;
		isAttacking = false;
		
		transform.position = savePoint;
		
		ChangeAnimation("idle");
		DeActiveAttack();
		SavePoint();
		
		UIManager.instance.SetCoin(coin);
	}

	private void Update()
	{
		if (isDead)
		{
			return;
		}

		if (isAttacking)
		{
			return;
		}
		if (isGrounded)
		{
			if (isJumping) return;
			if (Input.GetKeyDown(KeyCode.Space)) Jump();
			if (Input.GetKeyDown(KeyCode.C)) Attack();
			if (Input.GetKeyDown(KeyCode.V)) Throw();
		}

		if (transform.position.y <= -4f && !isDead)
		{
			isDead = true;
			Die();
		}
		
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		isGrounded = CheckGrounded();
		
		// GetAxisRaw -> Get input with that direction, return float from -1 to 1
		// horizontal = Input.GetAxisRaw("Horizontal");
		rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);

		// short for if else statement: condition true ? true case : false case

		if (isDead)
		{
			return;
		}
		
		if (isAttacking)
		{
			rb.velocity = new Vector2(0, rb.velocity.y);
			return;
		}

		if (isGrounded)
		{
			if (isJumping) return;
			
			
			if (Mathf.Abs(horizontal) > 0.1f)
	        {
	            Run();
	        }
	        else
	        {
	            Idle();
	        }
		}
		if (!isGrounded && rb.velocity.y < 0)
		{
			isJumping = false;
            ChangeAnimation("fall");
		}
	}

	private bool CheckGrounded()
	{
		Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.2f, Color.red);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.2f, groundLayer);
		return hit.collider != null;
	}

	private void Idle()
	{
        ChangeAnimation("idle");
        rb.velocity = Vector2.zero;
    }

	private void Run()
	{
		transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
		ChangeAnimation("run");
	}

	public void Attack()
	{
		rb.velocity = new Vector2(0, rb.velocity.y);
		isAttacking = true;
		ChangeAnimation("attack");
		Invoke(nameof(ResetAttack), 0.5f);
		ActiveAttack();
		Invoke(nameof(DeActiveAttack), 0.5f);
	}

	public void Throw()
	{
		rb.velocity = new Vector2(0, rb.velocity.y);
		isAttacking = true;
		ChangeAnimation("throw");
		Invoke(nameof(ResetAttack), 0.5f);

		Instantiate(kunaiPrefab, throwPosition.position, throwPosition.rotation);
	}
	public void Jump()
	{
		if (isGrounded && !isJumping)
		{
			isJumping = true;
	        rb.AddForce(jumpForce * Vector2.up);
	        transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
	        ChangeAnimation("jump");
		}
	}
    
	private void ResetAttack() 
	{
		isAttacking = false;
		ChangeAnimation("idle");
	}

	public override void OnDespawn()
	{
		base.OnDespawn();
		OnInit();
	}

	protected override void Die()
	{
		base.Die();
	}

	private void ActiveAttack()
	{
		attackArea.SetActive(true);
	}

	private void DeActiveAttack()
	{
		attackArea.SetActive(false);
	}

	public void SetMove(float horizontal)
	{
		print(this.horizontal);
		this.horizontal = horizontal;
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Coin")
		{
			coin++;
			PlayerPrefs.SetInt("coin", coin);
			UIManager.instance.SetCoin(coin);
			Destroy(col.gameObject);
		}
	}

	public void SavePoint()
	{
		savePoint = transform.position;
	}
	
	
}
