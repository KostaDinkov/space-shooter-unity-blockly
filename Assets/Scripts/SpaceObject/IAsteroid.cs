using UnityEngine;

#if UnityEditor
[ExecuteInEditMode]
#endif
namespace Scripts.SpaceObject
{
    public class IAsteroid : MonoBehaviour
    {
        private Transform asteroid;
        private Renderer rend;
        private Material blackMaterial;
        private Material defaultMaterial;
        private string asteroidName;

        void Awake()
        {
            this.asteroid = this.transform.GetChild(0);
            if (this.asteroid == null)
            {
                return;
            }
            this.rend = this.asteroid.gameObject.GetComponent<Renderer>();
            this.name = this.asteroid.name;
       
            this.blackMaterial = Resources.Load<Material>($"LoadableMaterials/{this.name}_mat_black");
            this.defaultMaterial = Resources.Load<Material>($"LoadableMaterials/{this.name}_mat");

            this.ApplyMaterial();

        }
    
    
        // Update is called once per frame
        void Update()
        {
#if UnityEditor        
        //Updates material while in edit mode. In play mode we do not need to apply material constantly;
        if (!EditorApplication.isPlaying)
        {
            ApplyMaterial();
        }
#endif
        
        }

        public void ApplyMaterial()
        {
            if (!this.GetComponent<SpaceObject>().IsDestroyable)
            {
            
                this.rend.material = this.blackMaterial;
            }
            else
            {
                this.rend.material = this.defaultMaterial;
            }
        }
    }
}
