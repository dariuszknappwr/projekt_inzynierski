using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    public static Collider GetCollisionContacts(Ray ray,Vector3 halfExtends, Quaternion orientation, LayerMask layer)
    {
        RaycastHit[] raycasthit = new RaycastHit[32];
        int contacts = Physics.BoxCastNonAlloc(ray.origin, halfExtends, ray.direction, raycasthit, orientation, 0.001f, 
            layer, QueryTriggerInteraction.Ignore);

        for(int i = 0; i < contacts; i++)
        {
            Collider col = raycasthit[i].collider;
            if (col != null)
                return col;
        }
        return null;
    }


    public static Vector3 MakeVectorNonZero(Vector3 moveVector)
    {
        if (moveVector.magnitude < 0.001f)
        {
            moveVector = Vector3.forward * 0.0001f;
        }
        return moveVector;
    }
}
