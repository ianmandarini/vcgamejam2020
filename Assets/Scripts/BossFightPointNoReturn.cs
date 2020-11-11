using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightPointNoReturn : MonoBehaviour
{
    private BoxCollider2D _boxCollider2D;

    void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        float distance = this.transform.position.x - other.transform.position.x;

        if (other.CompareTag("Player") && distance < 0)
        {
            _boxCollider2D.isTrigger = false;
        }
    }
}
