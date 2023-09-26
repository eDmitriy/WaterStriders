using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class WaterWithStriders : MonoBehaviour
{
    public List<Vector4> ripplePoints = new List<Vector4>()
    {
        new Vector4(2.74f, 0,0,0),
        new Vector4(-2.4f, 0,0,0),
    };

    private Material materialRef;

    private static readonly int RipplePointsPropertyId = Shader.PropertyToID("_RipplePoints");
    private static readonly int RipplePointsNumPropertyId = Shader.PropertyToID("_RipplePointsNum");


    public Material MaterialRef
    {
        get
        {
            if (materialRef == null)
            {
                MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                    materialRef = meshRenderer.sharedMaterial;
            }

            return materialRef;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {


        if (MaterialRef != null)
        {
            MaterialRef.SetVectorArray(RipplePointsPropertyId, ripplePoints);
            MaterialRef.SetInt(RipplePointsNumPropertyId, ripplePoints.Count);
        }
    }
}
