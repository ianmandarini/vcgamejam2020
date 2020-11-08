using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageOnCollide : MonoBehaviour
{
    private Vector3 _playerDistance;
    public int damage;
    public int damageForce;
    private Vector2 _directionToPlayer;

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
