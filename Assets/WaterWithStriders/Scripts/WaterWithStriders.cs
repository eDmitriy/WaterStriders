using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaterWithStriders : MonoBehaviour, IStridersVisualizer
{
    [Header("Ripples")]
    public float rippleMaxRadius = 2f;
    public float rippleDieSpeed = 0.5f;
    public int maxRipplesNum = 200;

    [Header("Waves")]
    float WavesWeightValue = 0.03f;
    float WaveSizeZ = 2;
    float WaveSizeX = 2;

    [Header("Debug")]
    public bool drawDebug = false;

    private List<Vector4> ripplePoints = new List<Vector4>(0);
    private int lastRippleIndex = 0;


    private Material materialRef;
    private Material MaterialRef
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

    private static readonly int RipplePointsPropertyId = Shader.PropertyToID("_RipplePoints");
    private static readonly int RipplePointsNumPropertyId = Shader.PropertyToID("_RipplePointsNum");
    private static readonly int RippleMaxRadiusPropertyId = Shader.PropertyToID("_RippleMaxRadius");
    private static readonly int WavesWeightPropertyId = Shader.PropertyToID("_WavesWeight");
    private static readonly int WavesZSizePropertyId = Shader.PropertyToID("_WavesZSize");
    private static readonly int WavesXSizePropertyId = Shader.PropertyToID("_WavesXSize");


    public void SetStridersPositions(List<Vector3> striderPositions)
    {
        if (ripplePoints.Count == 0)
        {
            ripplePoints = new List<Vector4>(maxRipplesNum);

            for (int i = 0; i < maxRipplesNum; i++)
            {
                ripplePoints.Add(new Vector4());
            }
        }

        foreach (Vector3 ripplePosition in striderPositions)
        {
            Vector4 ripple = new Vector4(ripplePosition.x, ripplePosition.y, ripplePosition.z, 1);
            ripplePoints[lastRippleIndex] = ripple;
            
            lastRippleIndex++;
            if (lastRippleIndex >= ripplePoints.Count)
                lastRippleIndex = 0;

        }

    }

    private void SetRipplePointsToMaterial()
    {
        if (MaterialRef == null || ripplePoints.Count == 0)
            return;

        MaterialRef.SetVectorArray(RipplePointsPropertyId, ripplePoints);
        MaterialRef.SetInt(RipplePointsNumPropertyId, ripplePoints.Count);
        MaterialRef.SetFloat(RippleMaxRadiusPropertyId, rippleMaxRadius);

        MaterialRef.SetFloat(WavesWeightPropertyId, WavesWeightValue);
        MaterialRef.SetFloat(WavesZSizePropertyId, WaveSizeZ);
        MaterialRef.SetFloat(WavesXSizePropertyId, WaveSizeX);
    }


    void Update()
    {
        if (ripplePoints.Count == 0)
            return;

        //process ripple points lifecycle
        for (var i = 0; i < ripplePoints.Count; i++)
        {
            Vector4 ripplePoint = ripplePoints[i];
            ripplePoint.w -= Time.deltaTime * rippleDieSpeed;
            ripplePoint.w = Mathf.Max(0, ripplePoint.w);

            ripplePoints[i] = ripplePoint;

            if (drawDebug)
            {
                Vector3 pos = new Vector3(ripplePoint.x, ripplePoint.y, ripplePoint.z);
                Debug.DrawRay(pos, Vector3.up * ripplePoint.w, Color.yellow);
            }
        }
        

        SetRipplePointsToMaterial();
    }


}
