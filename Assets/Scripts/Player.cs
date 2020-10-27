using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed;
    public Transform groundCheck;
    public float jumpForce;

    private Rigidbody2D _rb;
    private Animator _animator;
    private PlayerAttack _playerAttack;
    private float _speed;
    private bool _facingRight = true;
    private bool _isGrounded = true;
    private bool _jump = false;
    private bool _doubleJump = false;
    private Weapon _weaponEquipped;
    [SerializeField]
    private Weapon _defaultWeapon;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerAttack = GetComponentInChildren<PlayerAttack>();
        _speed = maxSpeed;
        _weaponEquipped = _defaultWeapon;
    }

    private void Update()
    {
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

        if (Input.GetButtonDown("Fire1"))
        {
            _animator.SetTrigger("Attack");
            _playerAttack.PlayAnimation(_weaponEquipped.animation);
        }
        //Resolver state machine do ataque
    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
    }

    private void FlipCharacter() //Inverte o lado para o qual o sprite está olhando
    {
        _facingRight = !_facingRight;
        Vector3 _scale = transform.localScale;
        _scale.x *= -1;
        transform.localScale = _scale;
    }

    public void AddWeapon(Weapon weapon)
    {
        _weaponEquipped = weapon;
        GetComponentInChildren<PlayerAttack>().SetWeapon(_weaponEquipped.damage);
    }

    private void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");

        _rb.velocity = new Vector2(h * _speed, _rb.velocity.y);

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
}
