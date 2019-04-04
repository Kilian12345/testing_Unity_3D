using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Math_Surfaces : MonoBehaviour
{

    	public Transform pointPrefab;
        Transform[] points;
        static Math_Surfaces_Delegate[] functions = 
        { SineFunction, MultiSineFunction, CustomSineFunction, Sine2DFunction, MultiSine2DFunction, Ripple, KilianTest };

        [Range(10, 100)] public int resolution = 10;
        public Math_Surface_Delegate_Name function;

        const float pi = Mathf.PI;
      
      #region CurveFunction
        static float SineFunction (float x, float z, float t)
        {
		    return Mathf.Sin(pi * (x + t));
	    }

        static float MultiSineFunction (float x, float z, float t) 
        {
		    float y = Mathf.Sin(pi * (x + t));
            y += Mathf.Sin(2f * pi * (x + t)) / 2f;
            y *= 2f / 3f;
		    return y;
	    }

        static float CustomSineFunction (float x, float z, float t) 
        {
		    float y = Mathf.Cos(pi * (x + t)) / 3f;
		    return y;
	    }

        static float Sine2DFunction  (float x, float z, float t) 
        {
		    float y = Mathf.Sin(pi * (x + t));
		    y += Mathf.Sin(pi * (z + t));
		    y *= 0.5f;
		    return y;
	    }

        static float MultiSine2DFunction (float x, float z, float t) 
        {
		    float y = 4f * Mathf.Sin(pi * (x + z + t * 0.5f));
	    	y += Mathf.Sin(pi * (x + t));
	    	y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
	    	y *= 1f / 5.5f;
	    	return y;
	    }

        static float Ripple (float x, float z, float t)
        {
		    float d = Mathf.Sqrt(x * x + z * z);
		    float y = Mathf.Sin(pi * (4.0f * d - t));
            y /= 1.0f + 10.0f * d;
		    return y;
	    }

        static float KilianTest (float x, float z, float t)
        {
		    float d = Mathf.Sqrt(x * x + z * z);
		    float y = Mathf.Acos(pi * (4.0f * d - t));
            y /= 1.0f + 10.0f * d;
		    return y;
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
            Math_Surfaces_Delegate f = functions[(int)function];

        	for (int i = 0; i < points.Length; i++) 
            {
			    Transform point = points[i];
			    Vector3 position = point.localPosition;

			    position.y = f(position.x, position.z, t);

                point.localPosition = position;              
            }
    }
}
