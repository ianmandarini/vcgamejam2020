using UnityEngine;

public class FollowPlayer: MonoBehaviour
{
    [SerializeField] private Rigidbody2D actor = default;
    [SerializeField] private TransformSO playerTransformSO = default;
    [SerializeField] private float speed = default;

    private void Update()
    {
        Vector2 directionToPlayer = (this.playerTransformSO.Transform.position - this.actor.transform.position).normalized;
        this.actor.velocity = directionToPlayer * this.speed;
    }
}