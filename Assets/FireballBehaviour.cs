using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehaviour : MonoBehaviour
{
    [SerializeField] private float _fireballSpeed = 3f;
    [SerializeField] private Rigidbody2D _actor;
    [SerializeField] private TransformSO _playerTransformSO = default;
    private Vector3 _playerDistance;
    public int damage;
    public int damageForce;
    private bool _goingRight = true;
    private Vector2 _directionToPlayer;

    private void Start()
    {
        _actor = GetComponent<Rigidbody2D>();
        _directionToPlayer = (this._playerTransformSO.Transform.position - this._actor.transform.position).normalized;
    }

    private void FixedUpdate()
    {

        Debug.Log(_directionToPlayer.x);
        if (_directionToPlayer.x < 0f)
        {
            transform.Translate(Vector2.left * _fireballSpeed * Time.fixedDeltaTime);
        }

        else if (_directionToPlayer.x > 0f)
        {
            transform.Translate(Vector2.right * _fireballSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            _playerDistance = player.transform.position - this.transform.position;

            player.TakeDamage(damage);
            float sideImpulse = _playerDistance.x / Mathf.Abs(_playerDistance.x);
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.right * damageForce * sideImpulse, ForceMode2D.Impulse);
            Destroy(this.gameObject);
        }
    }
}
