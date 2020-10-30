using UnityEngine;

public class FollowPlayer: MonoBehaviour
{
    [SerializeField] private Rigidbody2D actor = default;
    [SerializeField] private TransformSO playerTransformSO = default;
    [SerializeField] private float _defaultSpeed = default;
    [SerializeField] private float _skeletonSpeed = 10f;
    [SerializeField] private float _batSpeed = 1f;
    private string _parentTag;

    private void FixedUpdate()
    {
        _parentTag = this.gameObject.transform.parent.tag;
        
        if (_parentTag == "Skeleton") this._defaultSpeed = _skeletonSpeed;
        else if (_parentTag == "Bat") this._defaultSpeed = _batSpeed;
        
        
        Vector2 directionToPlayer = (this.playerTransformSO.Transform.position - this.actor.transform.position).normalized;
        this.actor.velocity = directionToPlayer * this._defaultSpeed;
    }
}