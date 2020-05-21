using System;
using UnityEngine;
[System.Serializable]
public class JsonPathConverter
{
    public float[] x;
    public float[] y;
    public float[] z;

    public Vector3[] Points
    {
        get
        {
            Vector3[] result = new Vector3[x.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new Vector3(x[i], y[i], z[i]);
            }
            return result;
        }
    }
}
