using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Movement Variables
    public float maxSpeed;
    public Transform groundCheck;
    public float jumpForce;
    private float _speed;
    private bool _facingRight = true;
    [SerializeField] private bool _isGrounded = true;
    [SerializeField] private bool _jump = false;
    [SerializeField] private bool _doubleJump = false;
    #endregion

    #region Attack Variables
    public float fireRate;
    private PlayerAttack _playerAttack;
    private bool _isAttacking = false;
    private Weapon _weaponEquipped;
    private float nextAttack;
    private bool _canTakeDamage = true;
    [SerializeField] private Weapon _defaultWeapon;
    #endregion

    #region Health Variables
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    private int _numberOfHearts;
    [SerializeField] private int _health;
    #endregion

    #region VFX //Mudar para FMOD depois
    public AudioClip jumpSound;
    public AudioClip attackSound;
    public AudioClip[] footStepsSound;
    #endregion

    private Rigidbody2D _rb;
    private Animator _animator;
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
        StartCoroutine(FootstepsCoroutine());
    }

    private void Update()
    {
        HealthCounter();

        CanDoubleJump();

        PlayerAttack();
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

    private void CanDoubleJump()
    {
        _isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        
        if (_isGrounded)
        {
            _doubleJump = false; //Desabilita double jump se o personagem estiver no chão
        }

        if (Input.GetButtonDown("Jump") && (_isGrounded || !_doubleJump))
        {
            _jump = true;
            if (!_doubleJump && !_isGrounded)
            {
                _doubleJump = true; //Habilita double jump se o personagem não estiver tocando no chão)
                FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/gameplay/nun jump start", GetComponent<Transform>().position);
            }
        }
    }

    private void PlayerAttack()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextAttack)
        {
            _isAttacking = true;
            _animator.SetTrigger("Attack");
            _playerAttack.PlayAnimation(_weaponEquipped.animation);
            nextAttack = Time.time + fireRate;
            FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/gameplay/nun attack start", GetComponent<Transform>().position);

            StartCoroutine(AttackCooldown());
        }
    }

    public void TakeDamage(int damage)
    {
        if (_canTakeDamage)
        {
            _canTakeDamage = false;
            _health -= damage;
            FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/gameplay/enemy attack hit", GetComponent<Transform>().position);
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

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(.4f);
        _isAttacking = false;
    }

    IEnumerator FootstepsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            if (_isGrounded && ((_rb.velocity.x > 0.1f) || _rb.velocity.x < -0.1f))
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/gameplay/nun footsteps");
            }
        }
    }
}
