using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Enemy : MonoBehaviour
{
    public int health;
    public GameObject itemDrop;
    public int damage;
    public int damageForce;
    //public ConsumableItem item;

    private Transform _player;
    [SerializeField] private SpriteRenderer _sprite = default;
    [SerializeField] bool killOnCollide = default;
    private Rigidbody2D _rb;
    private Vector3 _playerDistance;
    private bool _canTakeDamage = true;

    #region FMOD Events
    [EventRef] [SerializeField] private string _numAttackHit = default;

    #endregion

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(_player != null)
        {
            _playerDistance = _player.transform.position - this.transform.position;
        }
    }

    public void TakeDamage(int damage)
    {
        if (_canTakeDamage)
        {
            health -= damage;

            FMODUnity.RuntimeManager.PlayOneShot(_numAttackHit, GetComponent<Transform>().position);
        }

        _canTakeDamage = false;

        if (health <= 0)
        {
            _rb.velocity = Vector2.zero;
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
        for (int i = 0; i < 3; i += 1)
        {
            _sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator DamageWaitCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
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
            if(this.killOnCollide)
                Destroy(this.gameObject);
        }
    }
}
