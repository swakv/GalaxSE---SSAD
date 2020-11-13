/*  Space Scene Construction Kit Editor C# Script (version: 1.5)
	SPACE for UNITY - Space Scene Construction Kit
	https://www.imphenzia.com/space-for-unity
	(c) 2017 Imphenzia AB

    DESCRIPTION:
    Thruster, steering and weapon control script for Spaceship prefab.

    INSTRUCTIONS:
    This script is attached to the Spaceship demo prefab. Configure parameters to suit your needs.

    PARAMETERS:
    thrusters			(Thruster array containing reference to thrusters prefabs attached to the ship for propulsion)
    rollRate			(multiplier for rolling the ship when steering left/right)	
    yawRate				(multiplier for rudder/steering the ship when steering left/right)
    pitchRate			(multiplier for pitch when steering up/down)
    weaponMountPoints	(Vector3 array for mount points relative to ship where weapons will fire from)
    laserShotPrefab		(reference to Laser Shot prefab, i.e. the laser bullet prefab to be instanitated)
    soundEffectFire		(sound effect audio clip to be played when firing weapon)

    HINTS:
    The propulsion force of the thruster is configured for each attached thruster in the Thruster script.

    Version History
    1.5     - Added support for SU_TravelWarp for the spaceship to travel fast in a scene with a visual warp effect.
    1.06    - Updated for Unity 5.5, removed deprecated code. 
    1.02    - Prefixed with SU_Spaceship to avoid naming conflicts.
    1.01    - Initial Release.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SU_Spaceship : MonoBehaviour {
	
	// Array of thrusters attached to the spaceship
	public SU_Thruster[] thrusters;
	// Specify the roll rate (multiplier for rolling the ship when steering left/right)	
	public float rollRate = 100.0f;
	// Specify the yaw rate (multiplier for rudder/steering the ship when steering left/right)
	public float yawRate = 30.0f;
	// Specify the pitch rate (multiplier for pitch when steering up/down)
	public float pitchRate = 100.0f;
	// Weapon mount points on ship (this is where lasers will be fired from)
	public Vector3[] weaponMountPoints;	
	// Laser shot prefab
	public Transform laserShotPrefab;
	// Laser shot sound effect
	public AudioClip soundEffectFire;
    // Audio source to play weapon
    public AudioSource audioSourceLaser;
    // Array of particle systems to play during warp speed
    public ParticleSystem[] warpFlames;
    // Let's go ultra speed =) particle system with speed lines around the ship
    public ParticleSystem warpUltra;    
    public AudioSource audioSourceWarpUltra;

	// Private variables
	private Rigidbody _cacheRigidbody;
    private SU_TravelWarp _travelWarp;
    private float _orgWarpSpeed;
    private float _orgWarpStrength;

    void Start () {		
		// Ensure that the thrusters in the array have been linked properly
		foreach (SU_Thruster _thruster in thrusters)
			if (_thruster == null)
				Debug.LogError("Thruster array not properly configured. Attach thrusters to the game object and link them to the Thrusters array.");
		
		// Cache reference to rigidbody to improve performance
		_cacheRigidbody = GetComponent<Rigidbody>();
		if (_cacheRigidbody == null)
			Debug.LogError("Spaceship has no rigidbody - the thruster scripts will fail. Add rigidbody component to the spaceship.");

        // If there is a SU_TravelWarp component on this ship, grab the reference to it
        if (gameObject.GetComponent<SU_TravelWarp>() != null)
            _travelWarp = gameObject.GetComponent<SU_TravelWarp>();

        // Remember the original parameters to return to when exiting ultra warp (demo)
        if (_travelWarp)
        {
            _orgWarpSpeed = _travelWarp.visualTextureSpeed;
            _orgWarpStrength = _travelWarp.visualWarpEffectMagnitude;
        }
    }
	
	void Update ()
    {
		// Start all thrusters when pressing Fire 1
		if (Input.GetButtonDown("Fire1")) {		
			foreach (SU_Thruster _thruster in thrusters)
				_thruster.StartThruster();
            
		}
		// Stop all thrusters when releasing Fire 1
		if (Input.GetButtonUp("Fire1")) {		
			foreach (SU_Thruster _thruster in thrusters)
				_thruster.StopThruster();
		}
		
		if (Input.GetButtonDown("Fire2")) {
			// Itereate through each weapon mount point Vector3 in array
			foreach (Vector3 _wmp in weaponMountPoints) {
				// Calculate where the position is in world space for the mount point
				Vector3 _pos = transform.position + transform.right * _wmp.x + transform.up * _wmp.y + transform.forward * _wmp.z;
				// Instantiate the laser prefab at position with the spaceships rotation
				Transform _laserShot = (Transform) Instantiate(laserShotPrefab, _pos, transform.rotation);
				// Specify which transform it was that fired this round so we can ignore it for collision/hit
				_laserShot.GetComponent<SU_LaserShot>().firedBy = transform;
				
			}
			// Play sound effect when firing
			if (soundEffectFire != null) {
				audioSourceLaser.PlayOneShot(soundEffectFire);
			}
		}

        
        // If space key is held down...
        if (Input.GetKey(KeyCode.Space))
        {
            // Play the particle systems in the warpFlames array - these are for visuals only, not proper thrusters
            foreach (ParticleSystem _ps in warpFlames)
                _ps.Play();
            // Ensure that the normal thrusters are on
            foreach (SU_Thruster _thruster in thrusters)
                _thruster.StartThruster();

            // Set the Warp property of the SU_TravelWarp script to true so it knows we want to warp
            if (_travelWarp != null)
                _travelWarp.Warp = true;

            // Demo of modifying properties of TravelWarp - this example changes speed and strength of effect to simulte overdrive / ultra fast warping
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                if (!warpUltra.isPlaying)
                    warpUltra.Play();

                if (_travelWarp != null)
                {
                    _travelWarp.SetBrightness(2f);
                    _travelWarp.SetSpeed(4f);
                    _travelWarp.SetStrength(1f);
                    _travelWarp.SetUltraSpeedAddon(60f);
                    audioSourceWarpUltra.Play();

                }
            }
            if (Input.GetKeyUp(KeyCode.RightShift))
            {
                if (_travelWarp != null)
                {
                    _travelWarp.SetBrightness(1f);
                    _travelWarp.SetSpeed(_orgWarpSpeed);
                    _travelWarp.SetStrength(_orgWarpStrength);
                    _travelWarp.SetUltraSpeedAddon(0f);
                }
                if (warpUltra.isPlaying)
                    warpUltra.Stop();
            }


        }
        else
        {
            // If key is not held down...
            // Stop the particle systems in the array warpFlames
            foreach (ParticleSystem _ps in warpFlames)
                _ps.Stop();
            // If Fire1 is not pressed down, also stop the thrusters of the spaceship
            if (!Input.GetButton("Fire1"))
                foreach (SU_Thruster _thruster in thrusters)
                    _thruster.StopThruster();
            // Set the Warp property of SU_TravelWarp to false so we don't warp anymore
            if (_travelWarp != null)
            {
                _travelWarp.Warp = false;
                _travelWarp.SetBrightness(1f);
                _travelWarp.SetSpeed(0.5f);
                _travelWarp.SetStrength(0.2f);
                _travelWarp.SetUltraSpeedAddon(0);                
            }
            if (warpUltra.isPlaying) warpUltra.Stop();
        }

    }

    
    void FixedUpdate () {
		// In the physics update...
		// Add relative rotational roll torque when steering left/right
		_cacheRigidbody.AddRelativeTorque(new Vector3(0,0,-Input.GetAxis("Horizontal")*rollRate*_cacheRigidbody.mass));
		// Add rudder yaw torque when steering left/right
		_cacheRigidbody.AddRelativeTorque(new Vector3(0,Input.GetAxis("Horizontal")*yawRate*_cacheRigidbody.mass,0));
		// Add pitch torque when steering up/down
		_cacheRigidbody.AddRelativeTorque(new Vector3(Input.GetAxis("Vertical")*pitchRate*_cacheRigidbody.mass,0,0));	
	}


}
