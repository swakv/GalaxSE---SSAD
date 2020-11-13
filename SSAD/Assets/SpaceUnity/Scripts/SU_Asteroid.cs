/*  SU_Asteroid C# Script (version: 1.5)
    SPACE for UNITY - Space Scene Construction Kit
    https://www.imphenzia.com/space-for-unity
    (c) 2017 Imphenzia AB

    DESCRIPTION:
    This script handles an asteroid in terms of rotation and movement.

    INSTRUCTIONS:
    Drag this script onto an asteroid (or use the existing Asteroid prefab) and configure properties.

    PARAMETERS:
        rotationSpeed	(rotational speed of the asteroid)
        driftSpeed		(drift/movement speed of the asteroid)
        visibilityRange (visibility range of asteroids - invisible beyond the range)
        distanceFade    (distance percentile at which the asteroid will start to fade/scale, 0.5 = half way between origin and visibilityRange)
        fadeAsteroids   (if the asteroids should fade/scale or not)
        fadeAsteroidsFalloffExponent (customizable exponent for fading 1.0 = linear (default) - use 0.25, 0.5, 2, 4, 8 for curved fading/scaling

    Version History
    1.5     - Changed the way asteroids fade. Instead of using expensive alpha (transparency) fading scaling is used at the perimeter instead.
              The scaling is performed in a vertex shader so the GPU does the work for performance. This also looks better as the previous 
              method of fading in asteroids when there was a light background, like a star or galaxy, looked odd.
              The asteroid shader requires a _AsteroidOrigin parameter to be set so the vertex shader knows where the view center is so it can fade
              at the perimeter. The shader origin is set globally by SU_AsteroidFadeOrigin.cs and the script is added to the main camera at runtime (non-persistent)
              by default. If you want a different object to be the center, e.g. a spaceship or another camera, manually add the SU_AsteroidFadeOrigin component/script
              to a desired object.
            - Performance of asteroids greatly increased by using GPU Vertex shader for fading/scaling and removing alpha transparency.
            - Asteroids have a material with a Render Queue of 1900, therefore they are included in the effect of SU_TravelWarp (which warps everything with a RenderQueue < 1990)

    1.02    - Prefixed with SU_Asteroid to avoid naming conflicts.
            - Added documentation.
    1.01    - Initial Release.
*/

using UnityEngine;
using System.Collections;

public class SU_Asteroid : MonoBehaviour {
	// Enum to present choise of high, medium, or low quality mesh
	public enum PolyCount { HIGH, MEDIUM, LOW }
	// Variable to set the poly count (quality) of the asteroid, defualt is High quality
	public PolyCount polyCount = PolyCount.HIGH;
	// Variable to set the poly count for the collider (MUCH faster to use the low poly version)
	public PolyCount polyCountCollider = PolyCount.LOW;
	
	// Reference to different quality meshes
	public MeshFilter meshLowPoly;
	public MeshFilter meshMediumPoly;
	public MeshFilter meshHighPoly;
	
	// Rotation speed
	public float rotationSpeed = 0.0f;
	// Vector3 axis to rotate around
	public Vector3 rotationalAxis = Vector3.up;	
	// Drift/movement speed
	public float driftSpeed = 0.0f;
	// Vector3 direction for drift/movement
	public Vector3 driftAxis = Vector3.up;
    // Visibility range is used to fade/scale in/out asteroids at a distance
    public float visibilityRange = 20000f;
    // Distance percentile of spawn distance to start fading/scaling asteroids
    // Visibility = 1.0 at distanceFade*distanceSpawn*visibilityRange, and 0.0 at distanceSpawn*visibilityRange
    // (e.g. if visibilityRange is 20000 and distanceFade = 0.7 asteroids will fade/scale 14000 (fullly visible) -> 20000 (invisible)
    public float distanceFade = 0.7f;
    // Use shader to fade asteroids in/out
    public bool fadeAsteroids = true;
    // Exponent for fading asteroid 1.0 = linear (use 0.125, 0.5, 1 (linear), 2, 4, 8... for different fade curves)
    public float fadeAsteroidsFalloffExponent = 1f;
    // Private variables
    private Transform _transform;

    // Material of asteroid, needed to send parameters to shader for distance fade/scale effect
    private Material _material;
    
    void Start () {
		// Cache transforms to increase performance
		_transform = transform;
		// Set the mesh based on poly count (quality)
		SetPolyCount(polyCount);
        
        // Material of asteroid, needed to send parameters to shader for distance fade/scale effect
        _material = GetComponent<Renderer>().material;

        // Set asteroid material shader fade/scale settings
        if (fadeAsteroids)
        {
            // Fading (or scaling) of asteroids is enabled, set the shader of the asteroid to "SU_AsteroidFade"
            _material.shader = Shader.Find("SpaceUnity/SU_AsteroidFade");
            // Set the shader parameters falloff, inner, and outer radius. Asteroids will fade/scale in the region between inner and outer radius.
            _material.SetFloat("_FadeFalloffExp", fadeAsteroidsFalloffExponent);        
            _material.SetFloat("_InnerRadius", visibilityRange * distanceFade);
            _material.SetFloat("_OuterRadius", visibilityRange);

            // If there is no fade origin in the scene, add one to the main camera object during run time....
            if (FindObjectOfType<SU_AsteroidFadeOrigin>() == null)                
                // Assign the component to a game object manually if you desire another origin for the asteroids to fade/in out relative to.
                Camera.main.gameObject.AddComponent<SU_AsteroidFadeOrigin>();
        }
        else
        {
            // If fading is not used, use the SU_Asteroid shader instead - it does not contain the vertex transormation requierd for scaling.
            _material.shader = Shader.Find("SpaceUnity/SU_Asteroid");
        }

    }
	
	void Update () {						
		if (_transform != null) {
			// Rotate around own axis
			_transform.Rotate(rotationalAxis * rotationSpeed * Time.deltaTime);
			// Move in world space according to drift speed
			_transform.Translate(driftAxis * driftSpeed * Time.deltaTime, Space.World);
		}            
    }
	
	// Set the mesh based on the poly count (quality)
	public void SetPolyCount(PolyCount _newPolyCount) { SetPolyCount(_newPolyCount, false); }
	public void SetPolyCount(PolyCount _newPolyCount, bool _collider) {
		// If this is not the collider...
		if (!_collider) {
			// This is the actual asteroid mesh.. so specify which poly count we want
			polyCount = _newPolyCount;
			switch (_newPolyCount) {
			case PolyCount.LOW:
				// access the MeshFilter component and change the sharedMesh to the low poly version
				transform.GetComponent<MeshFilter>().sharedMesh = meshLowPoly.sharedMesh;				
				break;
			case PolyCount.MEDIUM:
				// access the MeshFilter component and change the sharedMesh to the medium poly version
				transform.GetComponent<MeshFilter>().sharedMesh = meshMediumPoly.sharedMesh;
				break;
			case PolyCount.HIGH:
				// access the MeshFilter component and change the sharedMesh to the high poly version
				transform.GetComponent<MeshFilter>().sharedMesh = meshHighPoly.sharedMesh;			
				break;
			}
		} else {
			// This is the collider mesh we set this time
			polyCountCollider = _newPolyCount;
			switch (_newPolyCount) {
			case PolyCount.LOW:
				// access the MeshFilter component and change the sharedMesh to the low poly version
				transform.GetComponent<MeshCollider>().sharedMesh = meshLowPoly.sharedMesh;				
				break;
			case PolyCount.MEDIUM:
				// access the MeshFilter component and change the sharedMesh to the medium poly version
				transform.GetComponent<MeshCollider>().sharedMesh = meshMediumPoly.sharedMesh;
				break;
			case PolyCount.HIGH:
				// access the MeshFilter component and change the sharedMesh to the high poly version
				transform.GetComponent<MeshCollider>().sharedMesh = meshHighPoly.sharedMesh;			
				break;
			}			
		}
	}
			
}
