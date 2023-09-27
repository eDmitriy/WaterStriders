using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[ExecuteAlways]
public class StriderSim : MonoBehaviour
{
    public float targetReachDistance = 0.5f;
    public float visualizerUpdateRate = 0.5f;

    public List<Strider> striders = new List<Strider>()
    {
        new Strider()
    };

    public List<Vector3> targetPoints = new List<Vector3>()
    {
        new Vector3(0,0,0),
        new Vector3(2,0,0),
    };

    private IStridersVisualizer striderVisualizer;
    public IStridersVisualizer StridersVisualizer
    {
        get
        {
            if (striderVisualizer == null)
                striderVisualizer = GetComponent<IStridersVisualizer>();

            return striderVisualizer;
        }
    }

    private float lastVisualisationTime = 0;

    // Start is called before the first frame update
    void Start()
    {
    }


    private Vector3 GetRandomTargetPoint()
    {
        return targetPoints[Random.Range(0, targetPoints.Count)];
    }
    private void StridersVisualisation()
    {
        if (Mathf.Abs(lastVisualisationTime - Time.timeSinceLevelLoad) > visualizerUpdateRate)
            lastVisualisationTime = Time.timeSinceLevelLoad;
        else
            return;
        

        if (StridersVisualizer == null || striders.Count <= 0) 
            return;

        //todo: linq is slow, temporary solution
        List<Vector3> stridersPositions = striders.Select(v=>v.CurrPosition).ToList();
        StridersVisualizer.SetStridersPositions(stridersPositions);
    }

    private void StridersSimulation()
    {
        foreach (Strider strider in striders)
        {
            if (strider.IsNearTarget(ref targetReachDistance))
                strider.SetNewTarget(GetRandomTargetPoint());

            strider.Move();
        }
    }

    void Update()
    {
        StridersSimulation();

        StridersVisualisation();

    }



}
