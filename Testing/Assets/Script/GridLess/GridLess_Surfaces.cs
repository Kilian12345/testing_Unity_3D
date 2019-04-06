using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLess_Surfaces : MonoBehaviour
{

    	public Transform pointPrefab;
        Transform[] points;
        static GridLess_Surface_Delegate[] functions = 
        { SineFunction, 
        MultiSineFunction, 
        CustomSineFunction, 
        Sine2DFunction, 
        MultiSine2DFunction, 
        Ripple,  
        KilianTest,
        Cylinder,
        Sphere,
        Torus
        };

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
            p.x = Mathf.Sin(pi * (x + t/2f)) / 3f;
		    p.y = Mathf.Cos(pi * (x + t/2f)) / 3f;
            p.z = Mathf.Tan(pi * (x + t/2f)) / 3f;
            p *= 3;
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

        static Vector3 KilianTest (float x, float z, float t)
        {
            Vector3 p;
		    float d = 15 / Mathf.Sqrt(x * x + z * z);
		    float y = 200 * Mathf.Floor(pi * 4.0f * d);
            float w = 100 / Mathf.Tan ( y*0.25f - t);
            w /= 1.0f +  d;

            p.x = x;
		    p.y = w;
		    p.z = z;
		    return p;
	    }

        static Vector3 Cylinder (float u, float v, float t)
        {
            Vector3 p;
		    float r = 0.8f + Mathf.Sin(pi * (6f * u + 2f * v + t)) * 0.2f;
	        p.x = r * Mathf.Sin(pi * u );
		    p.y = v;
		    p.z = r * Mathf.Cos(pi * u);

		    return p;
        }

        static Vector3 Sphere (float u, float v, float t) 
        {
		    Vector3 p;
		    float r = Mathf.Cos(pi * 0.5f * v);
		    p.x = r * Mathf.Sin(pi * u  + t);
		    p.y = Mathf.Sin(pi * 0.5f * v );
		    p.z = r * Mathf.Cos(pi * u + t);
		    return p;
	    }

        static Vector3 Torus (float u, float v, float t)
        {
		    Vector3 p;
	    	float r1 = 0.65f + Mathf.Sin(pi * (10f * u + t)) * 0.1f;
	    	float r2 = 0.2f + Mathf.Sin(pi * (4f * v + t)) * 0.1f;
		    float s = r2 * Mathf.Cos(pi * v) + r1;
		    p.x = s * Mathf.Sin(pi * u + t * 0.1f);
	    	p.y = r2 * Mathf.Tan(pi * v) * 2f;
	    	p.z = s * Mathf.Cos(pi * u  + t* 0.1f);

		    return p;
    	}
     #endregion

	void Awake () 
    {
		float step = 2f / resolution;
		Vector3 scale = Vector3.one * step;
		points = new Transform[resolution * resolution];

		for (int i = 0; i < points.Length; i++)
        {
			Transform point = Instantiate(pointPrefab);
			point.localScale = scale;
			point.SetParent(transform, false);
			points[i] = point;
		}
	}

	


    // Update is called once per frame
	void Update () {
		float t = Time.time;
		GridLess_Surface_Delegate f = functions[(int)function];

		float step = 2f / resolution;
		for (int i = 0, z = 0; z < resolution; z++) {
			float v = (z + 0.5f) * step - 1f;
			for (int x = 0; x < resolution; x++, i++) {
				float u = (x + 0.5f) * step - 1f;
				points[i].localPosition = f(u, v, t);
			}
		}
	}
}
