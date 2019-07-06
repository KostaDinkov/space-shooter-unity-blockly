using UnityEngine;
using Game.SpaceObject;
using UnityEditor;

#if UnityEditor
[ExecuteInEditMode]
#endif
public class IAsteroid : MonoBehaviour
{
    private Transform asteroid;
    private Renderer rend;
    private Material blackMaterial;
    private Material defaultMaterial;
    private string asteroidName;

    void Awake()
    {
        asteroid = transform.GetChild(0);
        if (asteroid == null)
        {
            return;
        }
        rend = asteroid.gameObject.GetComponent<Renderer>();
        name = asteroid.name;
        Debug.Log($"model name:{name}");
        blackMaterial = Resources.Load<Material>($"LoadableMaterials/{name}_mat_black");
        defaultMaterial = Resources.Load<Material>($"LoadableMaterials/{name}_mat");

        ApplyMaterial();

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
       if (!this.GetComponent<ISpaceObject>().IsDestroyable)
        {
            
            rend.material = blackMaterial;
        }
        else
        {
            rend.material = defaultMaterial;
        }
    }
}
