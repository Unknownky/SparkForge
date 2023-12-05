using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{

    public void Move(Vector3 direction)
    {
        transform.Translate(direction);
    }

    public bool CanObjectMove(Vector3 direction)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position + direction * 0.5f, direction, 1f);
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
}
