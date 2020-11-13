/*  Asteroid Field C# Script (version: 1.5)
	SPACE for UNITY - Space Scene Construction Kit
	https://www.imphenzia.com/space-for-unity
	(c) 2017 Imphenzia AB

    DESCRIPTION:
    This script creates a localized asteroid field around itself. As the object moves
    the asteroids will optionally re-spawn out of range asteroids within range (but out of sight.)

    INSTRUCTIONS:
    Use the AsteroidField prefab and make it a child of an object you wish to spawn asteroids around (e.g. a space ship)
    Alternatively, drag this script onto the game object that should be the center of the asteroid field.

    PARAMETERS:
    range           (radius of asteroid field)
    rotationSpeed	(rotational speed of the asteroid)
    driftSpeed		(drift/movement speed of the asteroid)

    Version History
    1.5     - Changed the way asteroids fade. Instead of using expensive alpha (transparency) fading scaling is used at the perimeter instead.
              The scaling is performed in a vertex shader so the GPU does the work for performance. This also looks better as the previous 
              method of fading in asteroids when there was a light background, like a star or galaxy, looked odd.
              The asteroid shader requires a _AsteroidOrigin parameter to be set so the vertex shader knows where the view center is so it can fade
              at the perimeter. The shader origin is set globally by SU_AsteroidFadeOrigin.cs and the script is added to the main camera at runtime (non-persistent)
              by default. If you want a different object to be the center, e.g. a spaceship or another camera, manually add the SU_AsteroidFadeOrigin component/script
              to a desired object.
            - Performance of asteroids greatly increased by using GPU Vertex shader for fading/scaling and removing alpha transparency.
    1.05    - Removed compiler conditional code, only 5.x supported.
    1.03    - Added compiler conditional code for major versions 4.1, 4.2, 4.3
            - Changed transparent asteroid material to new shader SpaceUnity/AsteroidTransparent located
    		  in a Resources subfolder to ensure it is included during compile (before, transparent asteroids
    		  wouldn't render in 4.x since the shader was not included in the build)
    1.02    - Prefixed with SU_AsteroidField to avoid naming conflicts.
              Added documentation.
    1.01    - Initial Release.
*/

using UnityEngine;
using System.Collections.Generic;

public class SU_AsteroidField : MonoBehaviour {	
	// Poly Count (quality) of the asteroids in the field
	public SU_Asteroid.PolyCount polyCount = SU_Asteroid.PolyCount.HIGH;
	// Poly Count (quality) of the asteroid colliders (LOW = fast, HIGH = slow)
	public SU_Asteroid.PolyCount polyCountCollider = SU_Asteroid.PolyCount.LOW;	

	// Array of prefabs that the asteroid fields should consist of
	public GameObject[] prefabAsteroids;
	
	// Arrays of materials of asteroids in the field
	public Material[] materialVeryCommon;		// 50%
	public Material[] materialCommon;			// 30%
	public Material[] materialRare;				// 15%
	public Material[] materialVeryRare;			// 5%
			
	// Range of asteroid field sphere (when asteroids are beyond this range from the game object  
	// they will respawn (relocate) to within range at distanceSpawn of range.
	public float range = 20000.0f;
	// Maximum number of asteroids in the sphere (configure to your needs for look and performance)
	public int maxAsteroids;	
	// Respawn destroyed asteroids true/false
	public bool respawnDestroyedAsteroids = true;
	// Respawn if out of range (must be true for infinite/endless asteroid fields
	public bool respawnIfOutOfRange = true;
	// Distance percentile of range to relocate/spawn asteroids to
	public float distanceSpawn = 0.95f;		
	// Minimum scale of asteroid	
	public float minAsteroidScale = 0.1f;
	// Maximum scale of asteroid
	public float maxAsteroidScale = 1.0f;	
	// Multiplier of scale
	public float scaleMultiplier = 1.0f;
    // Transform for origin of asteroid field, if null it will use itself
    public Transform asteroidFieldOriginTransform; // null = self
    // Distance percentile of spawn distance to start fading asteroids
    // Visibility = 1.0 at distanceFade*distanceSpawn*range, and 0.0 at distanceSpawn*range
    // (e.g. if range is 20000 and distanceFade = 0.7 asteroids will fade/scale 14000 (visible) -> 20000 (invisible)
    public float distanceFade = 0.7f;
    // Use shader to fade asteroids in/out
    public bool fadeAsteroids = true;
    // Exponent for fading asteroid 1.0 = linear (use 0.125, 0.5, 1 (linear), 2, 4, 8... for different fade curves)
    public float fadeAsteroidsFalloffExponent = 1f;
    // Use vertex shader to scale asteroids in/out
    public bool scaleAsteroids = true;
    // Exponent for scaling asteroid 1.0 = linear (use 0.125, 0.5, 1 (linear), 2, 4, 8... for different scale curves)
    public float scaleAsteroidsFalloffExponent = 1f;

    // Is rigid body or not
    public bool isRigidbody = false;
	
	// NON-RIGIDBODY ASTEROIDS ---
	// Minimum rotational speed of asteroid
	public float minAsteroidRotationSpeed = 0.0f;
	// Maximum rotational speed of asteroid
	public float maxAsteroidRotationSpeed = 1.0f;
	// Rotation speed multiplier
	public float rotationSpeedMultiplier = 1.0f;
	// Minimum drift/movement speed of asteroid
	public float minAsteroidDriftSpeed = 0.0f;
	// Maximum drift/movement speed of asteroid
	public float maxAsteroidDriftSpeed = 1.0f;
	// Multiplier of driftSpeed
	public float driftSpeedMultiplier = 1.0f;
	// ---------------------------
	
	// RIGIDBODY ASTEROIDS -------
	// Mass of asteroid (scaled between minAsterodiScale/maxAsteroidScale)
	public float mass = 1.0f;
	// Minimum angular velocity of asteroid (rotational speed)
	public float minAsteroidAngularVelocity = 0.0f;
	// Maximum angular velocity of asteroid (rotational speed)
	public float maxAsteroidAngularVelocity = 1.0f;
	// Angular velocity (rotational speed) multiplier
	public float angularVelocityMultiplier = 1.0f;
	// Minimum velocity of asteroid (drift/movement speed)
	public float minAsteroidVelocity = 0.0f;
	// Maximum velocity of asteroid (drift/movement speed)
	public float maxAsteroidVelocity = 1.0f;	
	// Velocity (drift/movement speed) multiplier
	public float velocityMultiplier = 1.0f;	
	// ----------------------------
	
    // Private variables
	private float _distanceToSpawn;	
	private Transform _transform;	
	private List<Transform> _asteroidsTransforms = new List<Transform>();
	private SortedList<int, string> _materialList = new SortedList<int, string>(4);    

    void OnEnable () {
		// Cache reference to transform to increase performance
		_transform = transform;			
		
		// Calculate the actual spawn
		_distanceToSpawn = range * distanceSpawn;
				
		// Populate the material list based on probabilty of materials
		if (_materialList.Count == 0) {
			if (materialVeryRare.Length > 0) _materialList.Add(5, "VeryRare");
			if (materialRare.Length > 0) _materialList.Add(5 + 15, "Rare");
			if (materialCommon.Length > 0) _materialList.Add(5 + 15 + 30, "Common");				
			if (materialVeryCommon.Length != 0) { 
				_materialList.Add(5 + 15 + 30 + 50, "VeryCommon");			
			} else {
				Debug.LogError("Asteroid Field must have at least one Material in the 'Material Very Common' Array."); 
			}		
		}


        // Check if there are any asteroid objects that was spawned prior to this script being disabled
        // If there are asteroids in the list, activate the gameObject again.
        for (int i = 0; i < _asteroidsTransforms.Count; i++)
            _asteroidsTransforms[i].gameObject.SetActive(true);

        // Spawn new asteroids in the entire sphere (not just at spawn range, hence the "false" parameter)
		SpawnAsteroids(false);

        // Use transform as origin (center) of asteroid field. If null, use this transform.
        if (asteroidFieldOriginTransform == null) asteroidFieldOriginTransform = transform;

        // Set the parameters for each asteroid for fading/scaling
        for (int i = 0; i < _asteroidsTransforms.Count; i++)
        {
            SU_Asteroid _a = _asteroidsTransforms[i].GetComponent<SU_Asteroid>();
            if (_a != null)
            {
                // When spawning asteroids, set the parameters for the shader based on parameters of this asteroid field
                _a.fadeAsteroids = fadeAsteroids;
                _a.fadeAsteroidsFalloffExponent = 1f; // fadeAsteroidsFalloffExponent;
                _a.distanceFade = distanceFade;
                _a.visibilityRange = range;
            }
        }
    }
	
	void OnDisable () {
		// Asteroid field game object has been disabled, disable all the asteroids as well
		for (int i = 0; i < _asteroidsTransforms.Count; i++) {
			// If the transform of the asteroid exists (it won't be upon application exit for example)...			
			if (_asteroidsTransforms[i] != null) {
				// deactivate the asteroid gameObject
				_asteroidsTransforms[i].gameObject.SetActive(false);
			}
		}
	}		
	
	void OnDrawGizmosSelected () {
	    // Draw a yellow wire gizmo sphere at the transform's position with the size of the asteroid field
	    Gizmos.color = Color.yellow;
	    Gizmos.DrawWireSphere (transform.position, range);
	}	
	
	void Update () {
		// Iterate through asteroids and relocate them as parent object moves		
		for (int i = _asteroidsTransforms.Count - 1; i >= 0; i--) {
			// Cache the reference to the Transform of the asteroid in the list
			Transform _asteroid = _asteroidsTransforms[i];
			
			// If the asteroid in the list has a Transform...
			if (_asteroid != null) {				
				// Calculate the distance of the asteroid to the center of the asteroid field
				float _distance = Vector3.Distance(_asteroid.position, _transform.position);
				
				// If the distance is greater than the range variable...
				if (_distance > range && respawnIfOutOfRange) {
					// Relocate ("respawn") the asteroid to a new position at spawning distance					
					_asteroid.position = (Random.onUnitSphere * _distanceToSpawn) + _transform.position;
					// Give the asteroid a new scale within the min/max scale range
					float _newScale = Random.Range(minAsteroidScale,maxAsteroidScale) * scaleMultiplier;
					_asteroid.localScale = new Vector3(_newScale, _newScale, _newScale);
					// Give the asteroid a new random rotation
					Vector3 _newRotation = new Vector3(Random.Range(0,360), Random.Range(0,360), Random.Range(0,360));					
					_asteroid.eulerAngles = _newRotation;
					// Recalculate the distance since it has been relocated
					//_distance = Vector3.Distance(_asteroid.position, _cacheTransform.position);					
				}											
			} else {
				// Asteroid transform must have been been destroyed for some reason (from another script?), remove it from the lists
				_asteroidsTransforms.RemoveAt(i);
			}
			
			// 	If respawning is enabled and asteroid count is lower than Max Asteroids...		
			if (respawnDestroyedAsteroids && _asteroidsTransforms.Count < maxAsteroids) {
				// Spawn new asteroids (where true states that they are to be spawned at spawn distance rather than anywhere in range)
				SpawnAsteroids(true);
			}
		}
    }


    /// <summary>
    /// Spawns the number of asteroids needed to reach maxAsteroids
    /// </summary>
    /// <param name='atSpawnDistance'>
    /// true = spawn on sphere at distanceSpawn * range (used for respawning asteroids)
    /// false = spawn in sphere within distanceSpawn * range (used for brand new asteroid fields)
    /// </param>
    void SpawnAsteroids(bool atSpawnDistance) {
		// Spawn new asteroids at a distance if count is below maxAsteroids (e.g. asteroids were destroyed outside of this script)
		while (_asteroidsTransforms.Count < maxAsteroids) {
			// Select a random asteroid from the prefab array			
			GameObject _newAsteroidPrefab = prefabAsteroids[Random.Range(0, prefabAsteroids.Length)];
			
			Vector3 _newPosition = Vector3.zero;			
			if (atSpawnDistance) {
				// Spawn asteroid at spawn distance (this is used for existing asteroid fields so it spawns out of visible range)
				_newPosition = _transform.position + Random.onUnitSphere * _distanceToSpawn;
			} else {
				// Spawn asteroid anywhere within range (this is used for new asteroid fields before it becomes visible)
				_newPosition = _transform.position + Random.insideUnitSphere * _distanceToSpawn;
			}
			
			// Instantiate the new asteroid at a random location
			GameObject _newAsteroid = Instantiate(_newAsteroidPrefab, _newPosition, _transform.rotation);
            Renderer _renderer = _newAsteroid.GetComponent<Renderer>();
            SU_Asteroid _asteroid = _newAsteroid.GetComponent<SU_Asteroid>();

            // Add the asteroid to a list used to keep track of them
            _asteroidsTransforms.Add(_newAsteroid.transform);

			// Set a random material of the asteroid based on the weighted probabilty list
			switch (WeightedRandom(_materialList)) {
			case "VeryCommon":
				_renderer.sharedMaterial = materialVeryCommon[Random.Range(0, materialVeryCommon.Length)];
				break;
			case "Common":
                    _renderer.sharedMaterial = materialCommon[Random.Range(0, materialCommon.Length)];
				break;
			case "Rare":
                    _renderer.sharedMaterial= materialRare[Random.Range(0, materialRare.Length)];
				break;
			case "VeryRare":
                    _renderer.sharedMaterial = materialVeryRare[Random.Range(0, materialVeryRare.Length)];
				break;
			}
			
			// Add the asteroid to a list used to keep track of them
			_asteroidsTransforms.Add(_newAsteroid.transform);
			
			// If the asteroid has the Asteroid script attached to it...
			if (_asteroid != null) {
				// Set the mesh of the asteroid based on chosen polycount
				_asteroid.SetPolyCount(polyCount);
				// If the asteroid has a collider...
				if (_newAsteroid.GetComponent<Collider>() != null) {
                    _asteroid.SetPolyCount(polyCountCollider, true);
				}
			}
			
			// Set scale of asteroid within min/max scale * scaleMultiplier
			float _newScale = Random.Range(minAsteroidScale,maxAsteroidScale) * scaleMultiplier;
			_newAsteroid.transform.localScale = new Vector3(_newScale, _newScale, _newScale);
			
			// Set a random orientation of the asteroid			
			_newAsteroid.transform.eulerAngles = new Vector3(Random.Range(0,360), Random.Range(0,360), Random.Range(0,360));


            Rigidbody _rigidbody = _newAsteroid.GetComponent<Rigidbody>();
            if (isRigidbody) {                
                // RIGIDBODY ASTEROIDS
                // If the asteroid prefab has a rigidbody...
                if (_rigidbody != null) {
                    // Set the mass to mass specified in AsteroidField mutiplied by scale
                    _rigidbody.mass = mass * _newScale;
                    // Set the velocity (speed) of the rigidbody to within the min/max velocity range multiplier by velocityMultiplier
                    _rigidbody.velocity = _newAsteroid.transform.forward * Random.Range(minAsteroidVelocity, maxAsteroidVelocity) * velocityMultiplier;
                    // Set the angular velocity (rotational speed) of the rigidbody to within the min/max velocity range multiplier by velocityMultiplier
                    _rigidbody.angularVelocity = new Vector3(Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f)) * Random.Range(minAsteroidAngularVelocity, maxAsteroidAngularVelocity) * angularVelocityMultiplier;
                } else {
                    Debug.LogWarning("AsteroidField is set to spawn rigidbody asterodids but one or more asteroid prefabs do not have rigidbody component attached.");
                }
            } else {
                // NON-RIGIDBODY ASTEROIDS

                // If the asteroid prefab has a rigidbody...
                if (_rigidbody != null) {
                    // Destroy the rigidbody since the asteroid field is spawning non-rigidbody asteroids
                    Destroy(_rigidbody);
                }
                // If the asteroid has the Asteroid script attached to it...
                if (_asteroid != null) {
                    // Set rotation and drift axis and speed				
                    _asteroid.rotationSpeed = Random.Range(minAsteroidRotationSpeed, maxAsteroidRotationSpeed);
                    _asteroid.rotationalAxis = new Vector3(Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
                    _asteroid.driftSpeed = Random.Range(minAsteroidDriftSpeed, maxAsteroidDriftSpeed);
                    _asteroid.driftAxis = new Vector3(Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
                }

            }

        }

    }
	
	// Internal function to allow weighted random selection of materials
	static T WeightedRandom<T>(SortedList<int, T> _list) {
		int _max = _list.Keys[_list.Keys.Count-1];
		int _random = Random.Range(0, _max);
		foreach (int _key in _list.Keys) {
			if (_random <= _key) {
				return _list[_key];
			}
		}
   		return default(T);
	}
}
