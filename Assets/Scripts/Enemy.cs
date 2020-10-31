using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public GameObject itemDrop;
    public int damage;
    public int damageForce;
    //public ConsumableItem item;

    private Transform _player;
    private bool _isDead = false;
    private SpriteRenderer _sprite;
    private Rigidbody2D _rb;
    private Vector3 _playerDistance;
    private bool _canTakeDamage = true;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _playerDistance = _player.transform.position - this.transform.position;
    }

    public void TakeDamage(int damage)
    {
        if (_canTakeDamage)
        {
            health -= damage;
        }

        _canTakeDamage = false;

        if (health <= 0)
        {
            _isDead = true;
            _rb.velocity = Vector2.zero;
            /*
            if(item != null)
            {
                GameObject tempItem = Instantiate(itemDrop, transform.position, transform.rotation);
                tempItem.GetComponente<ItemDrop>().item = item;

            Instanciamento de item ao derrotar inimigos. Deixar pra depois.
            }
            */
            Destroy(this.gameObject);
        }
        else
        {
            StartCoroutine(DamageWaitCoroutine());
            StartCoroutine(DamageFlashCoroutine());
        }
    }

    IEnumerator DamageFlashCoroutine()
    {
        for (float i = 0; i < 0.8; i += 0.2f)
        {
            _sprite.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            _sprite.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator DamageWaitCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        _canTakeDamage = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if(player != null)
        {
            player.TakeDamage(damage);
            float sideImpulse = _playerDistance.x / Mathf.Abs(_playerDistance.x);
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.right * damageForce * sideImpulse, ForceMode2D.Impulse);
        }
    }
}
