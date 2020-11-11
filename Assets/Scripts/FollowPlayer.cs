using UnityEngine;

public class FollowPlayer: MonoBehaviour
{
    [SerializeField] private Rigidbody2D actor = default;
    [SerializeField] private TransformSO playerTransformSO = default;
    [SerializeField] private float speed = default;
    [SerializeField] private GameObject _danceBehaviour = default;
    private Vector2 _directionToPlayer = default;
    
    private void Start()
    {
        if(_danceBehaviour != null)
        {
            _danceBehaviour.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        
        if(_danceBehaviour.GetComponent<DanceBehaviour>().isDancing == true)
        {
            _directionToPlayer = Vector2.zero;
        }
        else if (_danceBehaviour.GetComponent<DanceBehaviour>().isDancing == false)
        {
            if(this.playerTransformSO.Transform)
            {
                _directionToPlayer = (this.playerTransformSO.Transform.position - this.actor.transform.position).normalized;
            }
        }

        this.actor.velocity = _directionToPlayer * this.speed;
    }
}