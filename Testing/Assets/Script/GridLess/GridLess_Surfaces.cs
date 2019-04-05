using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLess_Surfaces : MonoBehaviour
{

    	public Transform pointPrefab;
        Transform[] points;
        static GridLess_Surface_Delegate[] functions = 
        { SineFunction, MultiSineFunction, CustomSineFunction, Sine2DFunction, MultiSine2DFunction, Ripple, KilianTest };

        [Range(10, 100)] public int resolution = 10;
        public GridLess_Surface_Name function;

        const float pi = Mathf.PI;
      
      #region CurveFunction
        static Vector3 SineFunction (float x, float z, float t)
        {
	    	Vector3 p;
	    	p.x = x;
	    	p.y = Mathf.Sin(pi * (x + t));
	    	p.z = z;
	    	return p;
	    }

        static Vector3 MultiSineFunction (float x, float z, float t) 
        {
		    Vector3 p;
		    p.x = x;
		    p.y = Mathf.Sin(pi * (x + t));
		    p.y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
		    p.y *= 2f / 3f;
		    p.z = z;
		    return p;
	    }

        static Vector3 CustomSineFunction (float x, float z, float t) 
        {
            Vector3 p;
            p.x = x;
		    p.y = Mathf.Cos(pi * (x + t)) / 3f;
            p.z = z;
		    return p;
	    }

        static Vector3 Sine2DFunction  (float x, float z, float t) 
        {
	    	Vector3 p;
		    p.x = x;
		    p.y = Mathf.Sin(pi * (x + t));
		    p.y += Mathf.Sin(pi * (z + t));
	    	p.y *= 0.5f;
	    	p.z = z;
		    return p;
	    }

        static Vector3 MultiSine2DFunction (float x, float z, float t) 
        {
	    	Vector3 p;
		    p.x = x;
		    p.y = 4f * Mathf.Sin(pi * (x + z + t / 2f));
		    p.y += Mathf.Sin(pi * (x + t));
		    p.y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
	    	p.y *= 1f / 5.5f;
		    p.z = z;
		    return p;
	    }

        static Vector3 Ripple (float x, float z, float t)
        {
	    	Vector3 p;
		    float d = Mathf.Sqrt(x * x + z * z);
		    p.x = x;
		    p.y = Mathf.Sin(pi * (4f * d - t));
		    p.y /= 1f + 10f * d;
		    p.z = z;
		    return p;
	    }

        static float KilianTest (float x, float z, float t)
        {
		    float d = 15 / Mathf.Sqrt(x * x + z * z);
		    float y = 200 * Mathf.Floor(pi * 4.0f * d);
            float w = 100 / Mathf.Tan ( y*0.25f - t);
            w /= 1.0f +  d;
		    return w;
	    }
     #endregion

  void Awake () 
  {
        float step = 2f / resolution;
		Vector3 scale = Vector3.one * step;
		Vector3 position;
        position.y = 0f;
        position.z = 0f;
        points = new Transform[resolution * resolution];


		for (int i = 0, z = 0; z < resolution; z++) 
        {
            position.z = (z + 0.5f) * step - 1f;

			for (int x = 0; x < resolution; x++, i++) 
            {

			    Transform point = Instantiate(pointPrefab);
			    position.x = (x + 0.5f) * step - 1f;
			    point.localPosition = position;
                point.localScale = scale;
                point.SetParent(transform, false);

                points[i] = point;
            }
		}

    }

	


    // Update is called once per frame
    void Update()
    {
        	float t = Time.time;
            GridLess_Surface_Delegate f = functions[(int)function];

        	for (int i = 0; i < points.Length; i++) 
            {
			    Transform point = points[i];
			    Vector3 position = point.localPosition;

			    position.y = f(position.x, position.z, t);

                point.localPosition = position;              
            }
    }
}
