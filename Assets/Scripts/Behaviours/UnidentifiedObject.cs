using System.Linq;
using Scripts.SpaceObject;
using UnityEngine;

namespace Scripts.Behaviours
{
    [RequireComponent(typeof(SpaceObject.SpaceObject))]
    public class UnidentifiedObject : MonoBehaviour
    {
        public bool IsIdentified;
        //public GameObject UnidentifiedPrefab;
        public SpaceObjectModelsMap ModelsMap;
        private SpaceObjectType spaceObjectType;

        void Awake()
        {
            this.spaceObjectType = this.GetComponent<SpaceObject.SpaceObject>().SpaceObjectType;
        }
        public void Identify()
        {
            //Should only work on Space Objects that are not identified
            if (this.IsIdentified) return;
            this.IsIdentified = true;

            //Todo remove question box

            var position = this.transform.position;
            this.gameObject.SetActive(false);
            this.RenderSOModel(position);

            //TODO change the Destroy on Collision event type
        }

        private void RenderSOModel(Vector3 position)
        {
            var sOModel = this.ModelsMap.ModelMap.FirstOrDefault(m => m.SpaceObjectType == this.spaceObjectType);
            var go = Instantiate(sOModel.Model, position, Quaternion.identity);
            //go.transform.localPosition = Vector3.zero;
            //var localPosition = go.transform.localPosition;

        }
    }
}
