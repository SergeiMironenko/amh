using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private float move;
    public float speed = 600;
    private bool isJump = false;
    public float jump = 800;
    public int hp = 3;
    public float godModeDuration = 0.5f;
    private bool godMode = false;
    private float dashMult = 1;
    public float dashMax = 10;
    private int dashStage = 0;
    public float dashTime = 1f;
    public float dashCoolDown = 1f;
    private bool isGrounded = false;
    private Transform groundCheck;
    private bool lookRight = true;
    private Animator animator;
    private Transform attackPoint;
    private int attackStage = 0;
    public float attackPrepareTime = 0.3f;
    public float attackTime = 0.1f;
    public float attackCoolDownTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // Application.targetFrameRate = 5;
        attackPoint = transform.GetChild(1);
        groundCheck = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerGrounded();
        updateMove();
        updateJump();
        isAttacking();
        updateDash();
        dieCheck();
    }

    private void FixedUpdate()
    {
        if (dashMult == 1)
        {
            rb.velocity = new Vector2(speed * move * Time.fixedDeltaTime, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(speed * dashMult * Time.fixedDeltaTime, rb.velocity.y);
        }

        if (isJump)
        {
            rb.AddForce(new Vector2(0, jump));
            isJump = false;
        }
    }

    // Находится ли игрок на земле
    void isPlayerGrounded()
    {
        Collider2D ground = Physics2D.OverlapCircle(groundCheck.position, 0.2f, 1 << 9);
        if (ground == null) isGrounded = false;
        else isGrounded = true;
    }

    // Вектор движения игрока
    void updateMove()
    {
        move = Input.GetAxis("Horizontal");
        animator.SetFloat("move", Mathf.Abs(move));
        if (move < 0 && lookRight || move > 0 && !lookRight) flip();
        // Debug.Log(move);
    }

    // Прыжок
    void updateJump()
    {
        if (Input.GetKeyDown("w") && isGrounded)
        {
            isJump = true;
        }
    }

    // Атакует ли игрок
    void isAttacking()
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(attackPoint.position, 1.1f, 1 << 8);
        if (attackStage == 2 && col.Length != 0)
        {
            col[0].GetComponent<Zombie>().damage();
        }
        if (Input.GetButtonDown("Jump") && attackStage == 0)
        {
            StartCoroutine(attack());
        }
    }

    // Дэш
    void updateDash()
    {
        if (Input.GetButtonDown("Dash") && Input.GetAxisRaw("Horizontal") != 0 && dashStage == 0)
        {
            StartCoroutine(dash());
        }
    }

    // Получение урона
    public void damage()
    {
        if (!godMode)
        {
            godMode = true;
            hp--;
            StartCoroutine(godModeCoolDown());
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    // Gameover
    void dieCheck()
    {
        if (hp == 0) gameObject.SetActive(false);
    }

    void flip()
    {
        lookRight = !lookRight;
        transform.Rotate(0, 180, 0);
    }

    // Неуязвимость после получения урона
    IEnumerator godModeCoolDown()
    {
        yield return new WaitForSeconds(godModeDuration);
        godMode = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    // Время дэша
    IEnumerator dash()
    {
        // Дэш
        dashStage = 1;
        if (move > 0) dashMult = dashMax;
        else dashMult = -dashMax;
        yield return new WaitForSeconds(dashTime);

        // Кулдаун
        dashStage = 2;
        dashMult = 1;
        yield return new WaitForSeconds(dashCoolDown);

        // Готовность к новому дэшу
        dashStage = 0;
    }

    // Анимация атаки
    IEnumerator attack()
    {
        // Подготовка к атаке (замах)
        attackStage = 1;
        animator.SetBool("attack", true);
        yield return new WaitForSeconds(attackPrepareTime);

        // Атака (удар)
        attackStage = 2;
        yield return new WaitForSeconds(attackTime);

        // Кулдаун (отмах)
        attackStage = 3;
        yield return new WaitForSeconds(attackCoolDownTime);

        // Простой (готовность к новой атаке)
        attackStage = 0;
        animator.SetBool("attack", false);
    }
}
