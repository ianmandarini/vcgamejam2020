using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float maxSpeed;
    public Transform groundCheck;
    public float jumpForce;
    public float fireRate;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private Rigidbody2D _rb;
    private Animator _animator;
    private PlayerAttack _playerAttack;
    private float _speed;
    private bool _facingRight = true;
    [SerializeField]private bool _isGrounded = true;
    private bool _isAttacking = false;
    [SerializeField]private bool _jump = false;
    [SerializeField]private bool _doubleJump = false;
    private Weapon _weaponEquipped;
    [SerializeField]
    private Weapon _defaultWeapon;
    private float nextAttack;
    private bool _canTakeDamage = true;
    [SerializeField]
    private int _health;
    private int _numberOfHearts;
    private SpriteRenderer _sprite;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerAttack = GetComponentInChildren<PlayerAttack>();
        _speed = maxSpeed;
        _weaponEquipped = _defaultWeapon;
        _sprite = GetComponent<SpriteRenderer>();
        _numberOfHearts = _health;
    }

    private void Update()
    {
        HealthCounter();
        
        _isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (_isGrounded)
        {
            _doubleJump = false; //Desabilita double jump se o personagem estiver no chão
        }

        if (Input.GetButtonDown("Jump") && (_isGrounded || !_doubleJump))
        {
            _jump = true;
            if(!_doubleJump && !_isGrounded)
            {
                _doubleJump = true; //Habilita double jump se o personagem não estiver tocando no chão)
            }
        }

        if (Input.GetButtonDown("Fire1") && Time.time > nextAttack)
        {
            _isAttacking = true;
            _animator.SetTrigger("Attack");
            _playerAttack.PlayAnimation(_weaponEquipped.animation);
            nextAttack = Time.time + fireRate;
            StartCoroutine(AttackCooldown());
        }
    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
    }

    private void FlipCharacter() //Inverte o lado para o qual o sprite está olhando
    {
        if (_isAttacking == false)
        { 
            _facingRight = !_facingRight;
            Vector3 _scale = transform.localScale;
            _scale.x *= -1;
            transform.localScale = _scale;
        }
    }

    public void AddWeapon(Weapon weapon)
    {
        _weaponEquipped = weapon;
        GetComponentInChildren<PlayerAttack>().SetWeapon(_weaponEquipped.damage);
    }

    private void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");

        if (_canTakeDamage)
        {
            _rb.velocity = new Vector2(h * _speed, _rb.velocity.y);
        }

        //Caso o último input seja para a direita o personagem vira para a direita
        //Caso seja para a esquerda, vira para essa direção
        if (h > 0 && !_facingRight)
        {
            FlipCharacter();
        }
        else if (h < 0 && _facingRight)
        {
            FlipCharacter();
        }
    }

    private void Jump()
    {
        if (_jump)
        {
            _rb.velocity = Vector2.zero;
            _rb.AddForce(Vector2.up * jumpForce);
            _jump = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (_canTakeDamage)
        {
            _canTakeDamage = false;
            _health -= damage;
            if(_health <= 0)
            {
                Debug.Log("Game Over");
                Destroy(this.gameObject);
                Invoke("ReloadScene", 2f);
            }
            else
            {
                StartCoroutine(DamageCoroutine());
            }
        }
    }

    IEnumerator DamageCoroutine()
    {
        for (float i = 0; i < 0.6f; i += 0.2f)
        {
            _sprite.enabled = false;
            yield return new WaitForSeconds(.1f);
            _sprite.enabled = true;
            yield return new WaitForSeconds(.1f);
        }

        _canTakeDamage = true;
    }

    private void HealthCounter()
    {
        if (_health > _numberOfHearts)
        {
            _health = _numberOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < _health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < _numberOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(.4f);
        _isAttacking = false;
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
