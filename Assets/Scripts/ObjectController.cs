using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [SerializeField]private float centerOffset = 0.5f; // 物体的中心点偏移量
    [SerializeField]private float raycasetDistance = 0.8f; // 射线检测的距离
    public void Move(Vector3 direction)
    {
        transform.Translate(direction);
    }

    public bool CanObjectMove(Vector3 direction)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position + direction * centerOffset, direction, raycasetDistance);
        if (raycastHit2D)
        {
            string hitObjectTag = raycastHit2D.collider.gameObject.tag;
            if (hitObjectTag == "Wall")
            {
                return false;
            }
            else if (hitObjectTag == "Box")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return true;

    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * centerOffset, transform.position + Vector3.up * centerOffset + Vector3.up * raycasetDistance);
        Gizmos.DrawLine(transform.position + Vector3.down * centerOffset, transform.position + Vector3.down * centerOffset + Vector3.down * raycasetDistance);
        Gizmos.DrawLine(transform.position + Vector3.left * centerOffset, transform.position + Vector3.left * centerOffset + Vector3.left * raycasetDistance);
        Gizmos.DrawLine(transform.position + Vector3.right * centerOffset, transform.position + Vector3.right * centerOffset + Vector3.right * raycasetDistance);
    }
}
