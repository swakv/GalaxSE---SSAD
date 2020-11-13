/*  SU_SpaceSceneCamera C# Script (version: 1.5)
	SPACE for UNITY - Space Scene Construction Kit
	https://www.imphenzia.com/space-for-unity
	(c) 2017 Imphenzia AB

    DESCRIPTION:
    This script is attached to the Space Scene camera and it renders the space scene which is then used as a background
    to the main camera.

    INSTRUCTIONS:
    This script is attached to the SpaceCamera prefab. Normally you do not need to configure anything if you use the prefab.

    But just in case: It is important that the SpaceScene camera has a depth value lower than the main camera of the scene. 
    The rendering path should be set to "Forward" and the culling mask should be set to "DeepSpace" layer (if the DeepSpace 
    layer does not exist, create it manually and name it "DeepSpace" and it should show up - Unity does not export layer
    names which is why it goes missing.) It is also important that the Main Camera (or whatever camera you use) is set to a
    depth valua higher than the space camera and that the Clear Flags to Depth Only, and finally that the Culling Mask
    ignores the DeepSpace layer.

    PARAMETERS:
     parentCamera	(reference to the parent (usually Main) camera so the space camera can rotate in sync)
                    (if no camera is specified the script will assume and try to att mainCamera)
     inheritFOV		(true/false if FOV (field of view) of space camera should change with the parent camera
     relativeSpeed  (relative speed if you wish to travel through the space scene)
                    (use with caution as you will go through planets and beyond nebulas unless you create boundaries yourself.)

    Version History
    1.5     - Changed relative speed method. Now you can set a relativeFocusObject (before it was always the camera itself which it still defaults to) 
            - Added the public Move method. This can be called from other scripts, for example SU_TravelWarp does this, for a delta movement of the space camera.
              The Move method is better because it does not affect excessive movement when rotating a camera around an object. 
    1.03    - Fixed deprecated code (changed Camera.mainCamera to Camera.main, and Camera.fov to Camera.fieldOfView)
    1.02    - Prefixed with SU_SpaceSceneCamera to avoid naming conflicts.
    1.01    - Initial Release.
*/

using UnityEngine;
using System.Collections;

public class SU_SpaceSceneCamera : MonoBehaviour {
	// Reference to parent camera to sync rotation and FOV (field of view)
	public Camera parentCamera;
	// Whether or not SpaceCamera should change FOV if parent camera FOV is changed
	public bool inheritFOV = true;
	// Relative speed if you wish to move within the space scene, 
	// use with caution as you will go through planets and beyond nebulas unless you create boundaries yourself.
	public float relativeSpeed = 0.0f;
    public Transform relativeFocusObject = null;

	// Private variables
	private Vector3 _originalPosition;
	private Transform _transform;
	private Transform _transformParentCamera;
    private Camera _camera;
    private Vector3 _relativeObjectPreviousPosition;
    

	// The space camera must have a reference to a parent camera so it knows how to rotate the background
	// This script allows you to specify a parent camera (parentCamera) which will act as reference
	// If you do not specify a camera, the script will assume you are using the main camera and select that as reference	
	void Start () {
		// Cache the transform to increase performance
		_transform = transform;
		
		if (parentCamera == null) {
			// No parent camera has been set, assume that main camera is used
			if (Camera.main != null) {
				// Set parent camera to main camera.
				parentCamera = Camera.main;							
			} else {				
				// No main camera found - throw a fit...
				Debug.LogWarning ("You have not specified a parent camera to the space background camera and there is no main camera in your scene. " +
								  "The space scene will not rotate properly unless you set the parentCamera in this script.");
			}
		}
		
		if (parentCamera != null) {
			// Cache the transform to the parent camera to increase performance
			_transformParentCamera = parentCamera.transform;							
		}

        // Cache the Camera component so we don't have to do it frequently in the update method
        _camera = gameObject.GetComponent<Camera>();
        if (relativeFocusObject == null) relativeFocusObject = _transformParentCamera;
        _relativeObjectPreviousPosition = relativeFocusObject.position;
    }
	
	void LateUpdate () {		
		if (_transformParentCamera != null) {
			// Update the rotation of the space camera so the background rotates		
			_transform.rotation = _transformParentCamera.rotation;

            // Inherit field of view of the main camera if applicable
			if (inheritFOV) _camera.fieldOfView = parentCamera.fieldOfView;			
		}

        // Update the relative position of the space camera so you can travel in the space scene if necessary
        // Note! You will fly out of bounds of the space scene if your relative speed is high unless you restrict the movement in your own code.
        Vector3 _relativeDetla = relativeFocusObject.position - _relativeObjectPreviousPosition;
        _relativeObjectPreviousPosition = relativeFocusObject.position;
        
        if (relativeSpeed > 0.00001f)
        {
            Move(_relativeDetla * relativeSpeed);
        }
    }

    // Public method that you can call to move the space scene camera by a vector. SU_TravelWarp does this do move within the virtual space scene.
    public void Move(Vector3 _vector)
    {
        _transform.position += _vector;
    }



    
}
