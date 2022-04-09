using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private GameObject player;
    public float speed = 400;
    private Rigidbody2D rb;
    private float move;
    private float t = 0;
    private float tStep = 0.05f;
    private float tMax = 1;
    private float tMin = 0;
    public float attackPrepareTime = 0.3f;
    public float attackTime = 0.5f;
    public float attackCoolDownTime = 2f;
    private int attackStage = 0;
    public int hp = 2;
    private bool lookRight = true;
    private bool godMode = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        collision();
        updateMove();
        dieCheck();
    }

    private void FixedUpdate()
    {
        if (godMode)
        {
            if (rb.velocity == Vector2.zero) godMode = false;
        }
        else
        {
            rb.velocity = new Vector2(speed * move * Time.fixedDeltaTime, rb.velocity.y);
        }
    }

    // Столкновение с игроком
    void collision()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 0.5f, 1 << 7);
        if (col == null)
        {
            if (t + tStep < tMax) t += tStep;
            else t = tMax;
        }
        else
        {
            if (t + tStep > tMin) t += tStep;
            else t = tMin;
            if (attackStage == 0) StartCoroutine(attackScenario());
            if (attackStage == 2) col.GetComponent<Player>().damage();
        }
    }

    // Единичный вектор движения к игроку
    void updateMove()
    {
        move = Mathf.Lerp(0, (player.transform.position - transform.position).normalized.x, t);
        if (move > 0 && !lookRight || move < 0 && lookRight) flip();
    }

    // Получение урона
    public void damage()
    {
        if (!godMode)
        {
            godMode = true;
            hp--;
            Vector2 forceVector = new Vector2((transform.position - player.transform.position).normalized.x * 500, 500);
            rb.AddForce(forceVector);
        }
    }

    // Уничтожение объекта
    void dieCheck()
    {
        if (hp == 0) Destroy(gameObject);
    }

    void flip()
    {
        lookRight = !lookRight;
        transform.Rotate(0, 180, 0);
    }

    // Этапы атаки: 0 - не атакует, 1 - подготовка, 2 - атака, 3 - кд
    IEnumerator attackScenario()
    {
        attackStage = 1;
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(attackPrepareTime);

        attackStage = 2;
        GetComponent<SpriteRenderer>().color = Color.magenta;
        yield return new WaitForSeconds(attackTime);

        GetComponent<SpriteRenderer>().color = Color.yellow;
        attackStage = 3;
        yield return new WaitForSeconds(attackCoolDownTime);

        attackStage = 0;
        GetComponent<SpriteRenderer>().color = Color.black;
    }
}
