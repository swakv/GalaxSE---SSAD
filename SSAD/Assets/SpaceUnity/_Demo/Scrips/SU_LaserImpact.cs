// Laser Impact C# Script (version: 1.06)
// SPACE UNITY - Space Scene Construction Kit
// http://www.spaceunity.com
// (c) 2013 Stefan Persson

// DESCRIPTION:
// Small script to animate light intensity for impact effects.

// INSTRUCTIONS:
// This script is attached to the LaserImpact prefab for demo purposes.

// Version History
// 1.06 - Updated for Unity 5.5, removed deprecated code. Changed light cache from transform to component.
// 1.02 - Prefixed with SU_LaserImpact to avoid naming conflicts.
//        Added documentation. Removed unused variables. Introduced simple error checking.
// 1.01 - Initial Release.

using UnityEngine;
using System.Collections;

public class SU_LaserImpact : MonoBehaviour {
	// Cache light to improve performance
	private Light _cacheLight;

	void Awake () {
		// If the child light exists...
        if (gameObject.GetComponentInChildren<Light>() != null )
        { 
			// Cache the light component to improve performance
			_cacheLight = gameObject.GetComponentInChildren<Light>();
			// Find the child light and set intensity to 1.0
			_cacheLight.intensity = 1.0f;
			// Move the transform 5 units out so it's not spawn at impact point of the collider/mesh it just hit
			// which will light up the object better.
			_cacheLight.transform.Translate(Vector3.up*5, Space.Self);
		} else {
			Debug.LogWarning("Missing required child light. Impact light effect won't be visible");
		}

        // Destroy after a second
        Destroy(gameObject, 1.0f);
		
	}
		
	void Update () {
		// If the light exists...
		if (_cacheLight != null) {
			// Set the intensity depending on the number of particles visible
			_cacheLight.intensity = (float) (transform.GetComponent<ParticleSystem>().particleCount / 50.0f);
		}
	}
}
