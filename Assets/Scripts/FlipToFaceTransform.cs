using System;
using UnityEngine;

public class FlipToFaceTransform: MonoBehaviour
{
    [SerializeField] private SpriteRenderer target = default;
    [SerializeField] private TransformSO playerTransformSO = default;
    [SerializeField] private bool startsFacingLeft = default;
    
    private void Update()
    {
        if (this.transform.position.x <= this.playerTransformSO.Transform.position.x)
            this.FaceRight();
        else
            this.FaceLeft();
    }

    private void FaceLeft()
    {
        this.target.flipX = this.startsFacingLeft;
    }

    private void FaceRight()
    {
        this.target.flipX = !this.startsFacingLeft;
    }
}