using UnityEngine;

namespace Scripts.SpaceObject
{
    public class ISpaceObject : MonoBehaviour
    {
        //if the object can be destroyed by collisions or lasers
        public bool IsDestroyable;

        //if the object has been scanned and identified
        public bool IsIdentified;

        public SpaceObjectType SpaceObjectType;


       
    }
}