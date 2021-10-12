using System.Diagnostics.Eventing.Reader;
using System.Linq;
using UnityEngine;

namespace Scripts.SpaceObject
{
    public class SpaceObject : MonoBehaviour
    {
        
        //if the object can be destroyed by collisions or lasers
        public bool IsDestroyable;

        //if the object has been scanned and identified
        

        public SpaceObjectType SpaceObjectType;

        public bool IsCollectable;

        
    }
}