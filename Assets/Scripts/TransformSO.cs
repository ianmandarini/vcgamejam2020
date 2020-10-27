using UnityEngine;

[CreateAssetMenu(menuName = "Custom/TransformSO")]
public class TransformSO: ScriptableObject
{
    public Transform Transform { get; private set; }
    
    public void SetTransform(Transform transform)
    {
        this.Transform = transform;
    }
}