using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using JetBrains.Annotations;
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
    [SerializeField] private ParticleSystem attackParticles = default;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private Weapon _defaultWeapon = default;
    #endregion

    #region Health Variables
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    private int _numberOfHearts;
    [SerializeField] private int _health;
    #endregion

    private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private GameObject _gameOverCanvas = default;
    private Animator _gameOverCanvasAnimator;
    
    #region Animation
    [SerializeField] private Animator animator = default;
    [SerializeField] private string isMovingAnimatorParameter = default;
    [SerializeField] private string attackedAnimatorParamenter = default;
    #endregion
    
    #region FMOD Events
    [EventRef] [SerializeField] private string footsteps = default;
    [EventRef] [SerializeField] private string jumpStartFMODEvent = default;
    [EventRef] [SerializeField] private string attackStartFMODEvent = default;
    [EventRef] [SerializeField] private string  attackHitFMODEvent = default;

    #endregion

    private void Start()
    {
        this._rb = this.GetComponent<Rigidbody2D>();
        this._playerAttack = this.GetComponentInChildren<PlayerAttack>();
        this._speed = this.maxSpeed;
        this._weaponEquipped = this._defaultWeapon;
        this._sprite = this.GetComponentInChildren<SpriteRenderer>();
        this._numberOfHearts = this._health;
        this.StartCoroutine(this.FootstepsCoroutine());
        this._gameOverCanvasAnimator = _gameOverCanvas.GetComponent<Animator>();
    }

    private void Update()
    {
        this.HealthCounter();
        this.CanDoubleJump();
        this.PlayerAttack();
    }

    private void FixedUpdate()
    {
        this.Movement();
        this.Jump();
    }

    private void FlipCharacter() //Inverte o lado para o qual o sprite está olhando
    {
        if (this._isAttacking != false)
            return;
        this._facingRight = !this._facingRight;
        Vector3 scale = this.transform.localScale;
        scale.x *= -1;
        this.transform.localScale = scale;
    }

    public void AddWeapon(Weapon weapon)
    {
        this._weaponEquipped = weapon;
        this.GetComponentInChildren<PlayerAttack>().SetWeapon(this._weaponEquipped.damage);
    }

    private void Movement()
    {
        bool isMoving = false;
        if (this._isAttacking)
        {
            //Player para o movimento caso ataque
            //isMoving = false; 
            //this._rb.velocity = new Vector2(0.0f, this._rb.velocity.y);
        }
        else
        {
            float h = Input.GetAxisRaw("Horizontal");

            if (this._canTakeDamage)
                this._rb.velocity = new Vector2(h * this._speed, this._rb.velocity.y);

            if (h > 0 && !this._facingRight)
                this.FlipCharacter();
            else if (h < 0 && this._facingRight)
                this.FlipCharacter();

            isMoving = Mathf.Abs(h) > 0.001f;
        }
        this.animator.SetBool(this.isMovingAnimatorParameter, isMoving);
    }

    private void Jump()
    {
        if (this._jump)
        {
            this._rb.velocity = Vector2.zero;
            this._rb.AddForce(Vector2.up * this.jumpForce);
            RuntimeManager.PlayOneShot(this.jumpStartFMODEvent, transform.position);
            this._jump = false;
        }
    }

    private void CanDoubleJump()
    {
        this._isGrounded = Physics2D.Linecast(this.transform.position, this.groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        
        if (this._isGrounded)
        {
            this._doubleJump = false; //Desabilita double jump se o personagem estiver no chão
        }

        if (Input.GetButtonDown("Jump") && (this._isGrounded || !this._doubleJump))
        {
            this._jump = true;
            if (!this._doubleJump && !this._isGrounded)
            {
                this._doubleJump = true; //Habilita double jump se o personagem não estiver tocando no chão)
                RuntimeManager.PlayOneShot(this.jumpStartFMODEvent, transform.position);
            }
        }
    }

    private void PlayerAttack()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > this.nextAttack)
        {
            this._isAttacking = true;
            this.animator.SetTrigger(this.attackedAnimatorParamenter);
            this.nextAttack = Time.time + this.fireRate;
            RuntimeManager.PlayOneShot(this.attackStartFMODEvent, transform.position);

            this.StartCoroutine(this.AttackCooldown());
        }
    }

    [UsedImplicitly]
    public void TriggerAttackParticles()
    {
        this.attackParticles.Play();
    }

    public void TakeDamage(int damage)
    {
        if (!this._canTakeDamage)
            return;
        this._canTakeDamage = false;
        this._health -= damage;
        RuntimeManager.PlayOneShot(this.attackHitFMODEvent, transform.position);
        if(this._health <= 0)
        {
            _gameOverCanvas.SetActive(true);
            Destroy(this.gameObject);
        }
        else
        {
            this.StartCoroutine(this.DamageCoroutine());
        }
    }

    private void HealthCounter()
    {
        if (this._health > this._numberOfHearts)
            this._health = this._numberOfHearts;

        for (int i = 0; i < this.hearts.Length; i++)
        {
            this.hearts[i].sprite = i < this._health ? this.fullHeart : this.emptyHeart;
            this.hearts[i].enabled = i < this._numberOfHearts;
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
            this._sprite.enabled = false;
            yield return new WaitForSeconds(.07f);
            this._sprite.enabled = true;
            yield return new WaitForSeconds(.07f);
        }

        this._canTakeDamage = true;
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(this.attackCooldown);
        this._isAttacking = false;
    }

    IEnumerator FootstepsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            if (this._isGrounded && ((this._rb.velocity.x > 0.1f) || this._rb.velocity.x < -0.1f))
                RuntimeManager.PlayOneShot(this.footsteps);
        }
    }
}
