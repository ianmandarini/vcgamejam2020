using UnityEngine;

public class TransformSetter: MonoBehaviour
{
        [SerializeField] private TransformSO transformSO = default;

        private void Update()
        {
                this.transformSO.SetTransform(this.transform);
        }
}