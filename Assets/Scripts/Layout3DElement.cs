using UnityEngine;

namespace Runtime
{
    public class Layout3DElement : MonoBehaviour
    {
        public Vector3 dimension = Vector3.one;
        public Vector3 centerOffset = Vector3.zero;
        public Vector3 rotationOffset = Vector3.zero;
        
        public Vector3 localPosition
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }
        
        public Quaternion localRotation
        {
            get => transform.localRotation;
            set => transform.localRotation = value;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            
            Gizmos.DrawWireCube(transform.position + centerOffset, dimension);
        }
        
        
    }
}