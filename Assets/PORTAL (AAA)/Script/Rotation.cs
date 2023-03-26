using UnityEngine;
    
public class Rotation : MonoBehaviour
{
    [Range(-1.0f, 1.0f)]
    public float xForceDirection = 0.0f;
    [Range(-1.0f, 1.0f)]
    public float yForceDirection = 0.0f;
    [Range(-1.0f, 1.0f)]
    public float zForceDirection = 0.0f;
    
    public float speedMultiplier = 1;
    
    public bool worldPivote = false;
    
    private UnityEngine.Space spacePivot = UnityEngine.Space.Self;
    
    
    void Start()
    {
        if (worldPivote) spacePivot = UnityEngine.Space.World;
    }
    
    void Update()
    {
        this.transform.Rotate(xForceDirection * speedMultiplier
                            , yForceDirection * speedMultiplier
                            , zForceDirection * speedMultiplier
                            , spacePivot);
    }
    
}
    

 