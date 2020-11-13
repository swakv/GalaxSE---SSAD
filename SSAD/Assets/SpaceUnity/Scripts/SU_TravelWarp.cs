/*  SU_TravelWarp C# Script (version: 1.5)
	SPACE for UNITY - Space Scene Construction Kit
	https://www.imphenzia.com/space-for-unity
	(c) 2017 Imphenzia AB

    This component can be added to an object that should be able to warp within a space scene and it adds a visual distortion warp effect
    at the same time as it moves an object relative within the DeepSpace space scene layer.

    The component procedually generates a tube that is cone-shaped at both ends. The tube is then bent using in the vertex function of the SU_WarpDistortion shader.
    The bend will only work if the object to which this component is attached has a rigid body with angular velocity.

    NOTE: There is 0 scientific accuracy of this - all movement speeds and multipliers can be customized to achieve a desired effect but it has no relation to
    actual travel speed in terms of light speed, meters per second etc.

    DISCLAIMER: It can be tricky to use this component because moving relative within a scene and it requires you to manage movement of all objects relative to one another.
    This script can be used and modified and you will likely need to understand programming to a level of being able to modify this script or create similar components
    to manage all objects within a scene to make them appear being in a much larger place. A unity scene only allows for about 20km x 20km with decent accuracy, anything
    beyond 100km in a scene will start to jitter due to floating point accuracy. This can be addressed using a "floating origin" approach but this component is not such a script.
    
    This component will move/translate objects that are explicitly not excluded (see the public variables) by a relative amount in the opposity direction of travel. If this was not
    performed you would enter warp speed but any objects around you (e.g. other spaceships, asteroids, etc.) would still be right next to you while you warp which sort of defeats
    the object of warping =)

    The objects that are translated/moved will have their colliders disabled during the warp (and then re-enabled after the warp) and objects beyond a certain distance are disabled
    until warp finishes when they are enabled again.

    HOW TO USE:
    1. Read the above disclaimer first.
    2. Add SU_TravelWarp component to an object that you want to be able to travel fast with a warp effect within a scene.
    3. Configure the exclusion parameters in the inspector for the public variables
    4. Set the bool property Warp to true (when you want to warp) or false (when you don't want to warp), e.g. gameObject.GetComponent<SU_TravelWarp>().Warp = true;

    Version History
    1.5     - New component for version 1.5.
*/
using UnityEngine;
using System.Collections.Generic;

public class SU_TravelWarp : MonoBehaviour
{
    // Texture that uses RGB values to add a distortion warp-like effect when traveling
    public Texture2D warpTexture;
    // Tiling values of the texture
    public Vector2 textureTiling = new Vector2(1, 10);

    // Option to exclude objects from being move relatively away during a warp, most of these should be excluded (as default)
    public bool excludeDeepSpaceLayer = true;
    public bool excludeCameras = true;
    public bool excludeLights = true;
    public bool excludeSelf = true;
    public bool excludeSpaceParticles = true;
    // You can customize your own layers to be excluded in this array
    public int[] excludedLayers;
    // You can customize your own gameobjects that should be excluded in this array
    public GameObject[] excludedGameObjects;
    // You can customize your own tags that should be excluded in this array
    public string[] excludedTags;
    // Acceleration, this is not scientific, it's just a value to be multiplied by Time.deltaTime to increase the warp speed for a smooth acceleration
    public float acceleration = 10f;
    // Max speed of warp - the speed dictates how fast relative movement should be when calling the Move method of SU_SpaceCamera. 
    public float maxSpeed = 20f;

    // A multiplier that affects how fast objects are translated away from the warping object
    public float surroundingObjectMultiplier = 20000f;
    // Magnitude of the warp shader distortion effect, can be customized for a stronger/weaker effect
    public float visualWarpEffectMagnitude = 0.2f;
    // Visual scrolling of warp texture, can be customized for a faster/slower appearance.
    public float visualTextureSpeed = 0.5f;
    // Distance that relatively moved game objects will be disabled
    public float warpSpeedHideDistance = 20000f;
    // The property of the actual current warp speed
    public float WarpSpeedMultiplier { get; private set; }
    // Audio sources for warp sound effects
    public AudioSource audioSourceWarp;

    // Public property that can be set from external scripts to control warp. SU_SpaceShip demo script uses this to warp while holding the Space key.
    public bool Warp { get; set;  }

    // Warp states
    public enum WarpState { NONE, ACCELERATE, WARP, DECELERATE }
    [HideInInspector]
    public WarpState warpState = WarpState.NONE;    

    // Private Variables
    private Transform _warpEffectTransform;
    private MeshFilter _meshFilter;
    private Mesh _mesh;
    private Material _material;
    private Rigidbody _rigidbody;
    private Dictionary<GameObject, WarpObject> _warpObjects = new Dictionary<GameObject, WarpObject>();
    private SU_SpaceParticles[] _spaceParticles;
    private Vector3 _previousPosition;
    private SU_SpaceSceneCamera _spaceCamera;
    private float _ultraSpeedAddon;
    // shader property caching
    private int _shaderIDWarpBend;
    private int _shaderIDWarpRadiusMultiplier;
    private int _shaderIDWarpStrength;
    private int _shaderIDWarpBrightness;
    private int _shaderIDWarpSpeed;

    // Private class to keep track of objects that you have warped away from
    private class WarpObject
    {
        public GameObject gameObject;
        public Transform transform;
        public Vector3 position;
        public bool colliderEnabled;
    }

    void Awake()
    {
        // Create a new game object
        GameObject _go = new GameObject("WarpEffect");
        _warpEffectTransform = _go.transform;
        // Set the transform parent to the object that this component is attached to
        _warpEffectTransform.parent = transform;
        // Ensure the local position is 0
        _warpEffectTransform.localPosition = Vector3.zero;
        // Rotate the object 90 degrees around the x axis, we want to travel through the length of the tube
        _warpEffectTransform.localEulerAngles = new Vector3(90, 0, 0);
        // Add mesh filter component
        _meshFilter = _warpEffectTransform.gameObject.AddComponent<MeshFilter>();
        // Add mesh renderer component
        _warpEffectTransform.gameObject.AddComponent<MeshRenderer>();
        // Procedurally generate an inverted tube mesh (normals facing inwards) with cone shaped caps
        _mesh = CreateMesh();
        // Set the mesh filter mesh to the newly created mesh
        _meshFilter.mesh = _mesh;

        // Create a material using the SU_WarpFXDistortion shader
        _material = new Material(Shader.Find("SpaceUnity/SU_WarpFXDistortion"));
        // Set the noise texture
        _material.SetTexture("_NoiseTex", warpTexture);
        // Set the noise texture scale
        _material.SetTextureScale("_NoiseTex", textureTiling);
        // Set the renderer to use this new material
        _warpEffectTransform.GetComponent<Renderer>().material = _material;

        // Ensure that there is a space scene camera in the scene
        if (GameObject.FindGameObjectWithTag("SpaceScene_Camera") == null)
            Debug.Log("There is no Space Camera with the tag 'SpaceScene_Camera' in the scene. Add it from the Prefabs/SpaceSceneElements folder.");
        else
            _spaceCamera = GameObject.FindGameObjectWithTag("SpaceScene_Camera").GetComponent<SU_SpaceSceneCamera>();

        // Disable the warp effect renderer by default
        DisableRenderer();
        SetStrength(0);

        // Get the component of the rigid body
        _rigidbody = GetComponent<Rigidbody>();

        // Set the shader properties for bending and expansion/shrink of the tube and the effect strength
        _shaderIDWarpBend = Shader.PropertyToID("_WarpBend");
        _shaderIDWarpRadiusMultiplier = Shader.PropertyToID("_WarpRadiusMultiplier");
        _shaderIDWarpStrength = Shader.PropertyToID("_WarpStrength");
        _shaderIDWarpBrightness = Shader.PropertyToID("_WarpBrightness");
        _shaderIDWarpSpeed = Shader.PropertyToID("_WarpSpeed");

        // Set the shader property for the WarpSeed (the multiplier will then be realtive to this value)
        _material.SetFloat("_WarpSpeed", visualTextureSpeed);

    }


    void LateUpdate()
    {
        // If the space scene camera is not set for some reason (maybe recreated?)...
        if (_spaceCamera == null)
        {
            // Find the space scene camera and return
            _spaceCamera = FindObjectOfType<SU_SpaceSceneCamera>();
            return;
        }
            
        // Calculate the delta position of the object that's warping
        Vector3 _deltaPosition = transform.position - _previousPosition;
        _previousPosition = transform.position;

        if (WarpSpeedMultiplier > maxSpeed + _ultraSpeedAddon)
            WarpSpeedMultiplier = maxSpeed + _ultraSpeedAddon;

        // WARP BEGIN
        // If Warp is set to enabled from external script and WarpState is set to NONE - start initiating the warp
        if (Warp && warpState == WarpState.NONE)
        {
            // Clear the dictionary containing all objects that are affected by the warp
            _warpObjects.Clear();

            // Build a dictionary of objects that are affected by the warp (those objects will need to be moved to simulate warping away from them)
            foreach (GameObject _g in FindObjectsOfType<GameObject>())
            {
                // Exclude objects from being offset by the warp
                bool _exclude = false;

                // Exclude cameras if set in the inspector (excluded by default)
                if (excludeCameras && _g.GetComponent<Camera>() != null)
                    _exclude = true;

                // Exclude lights if set in the inspector (excluded by default)
                if (excludeLights && _g.GetComponent<Light>() != null) 
                    _exclude = true;

                // Exclude objects in the deep space layer (we don't want simulate moving away from large objects like stars and planets) (excluded by default)
                if (!_exclude && excludeDeepSpaceLayer && _g.layer == 20)
                    _exclude = true;

                // Exclude space particles (excluded by default)
                if (!_exclude && _g.GetComponent<SU_SpaceParticles>() != null)
                    _exclude = true;

                // Exclude specific layers as configured in the array of layers in the inspector 
                if (!_exclude && excludedLayers.Length > 0)
                    foreach (int _i in excludedLayers)
                        if (_g.layer == _i) _exclude = true;

                // Exclude specific tags as configured in the array of tags in the inspector
                if (!_exclude && excludedTags.Length > 0)
                    foreach (string _s in excludedTags)
                        if (_g.tag == _s) _exclude = true;

                // Exclude specific gameobjects as configured in the array of objects in the inspector
                if (!_exclude && excludedGameObjects.Length > 0)
                    foreach (GameObject _xg in excludedGameObjects)
                        if (_g == _xg) _exclude = true;

                // Exclude self (this object) and any children of this gameobject
                if (!_exclude && excludeSelf)
                    foreach (Transform _t in transform.GetComponentsInChildren<Transform>())
                        if (_g == _t.gameObject) _exclude = true;

                // If the game object has not been excluded, add it to the dictionary so it can be moved during the warp phase to simulate warping away from it
                // Unity only supports float values for vector3 so we can't make a huge universe in a scene. This is why we need to identify gameobjects that we want
                // to appear to be rapidly moving away from them and move them manually.               
                if (!_exclude && _g.transform != null)
                {
                    // Create a new warp object
                    WarpObject _wo = new WarpObject();
                    // Remember the gameobject and transform for increased perofmance when translating the objects
                    _wo.gameObject = _g;
                    _wo.transform = _g.transform;
                    // Remember the original position of the gameobject
                    _wo.position = _g.transform.position;                                  
                    // If the object has a collider...
                    if (_g.GetComponent<Collider>() != null)
                    {
                        // Inspect whether the collider is enabled or not (so we can return it to the same state later)
                        _wo.colliderEnabled = _g.GetComponent<Collider>().enabled;
                        // Disable the collider, we don't want to bump into this object during warp - it'll abruptly halt the warp. Better to just warp through it.
                        _g.GetComponent<Collider>().enabled = false;
                    }
                    // Add the warp object to a the dictionary
                    _warpObjects.Add(_g, _wo);
                }                    
            }

            // Find all instances of SU_SpaceParticles component (both particles and fog uses this)
            _spaceParticles = FindObjectsOfType<SU_SpaceParticles>();

            // Set the previous position for delta movement detection to current position
            _previousPosition = transform.position;

            // Enable the warp renderer
            EnableRenderer();

            // Set state to ACCELERATE
            warpState = WarpState.ACCELERATE;
        }

        // ACCELERATE
        if (warpState == WarpState.ACCELERATE)
        {
            // While we have not achieved max speed with the warp speed multiplier...
            if (WarpSpeedMultiplier < maxSpeed + _ultraSpeedAddon)
            {
                // Increase the warp speed multiplier by acceleration
                WarpSpeedMultiplier += acceleration * Time.deltaTime;
            }
            else
            {
                warpState = WarpState.WARP;
            }

            // If no longer warping (Warp is set to false from external script) switch to DECELERATE state
            if (!Warp) warpState = WarpState.DECELERATE;
        }

        // WARPING
        if (warpState == WarpState.WARP)
        {
            // Loop through warp objects and hide any object that is beyond warp hide distance
            foreach (KeyValuePair<GameObject, WarpObject> _g in _warpObjects)
                if (_g.Key != null)
                    if (_g.Key.activeSelf)
                        if (Vector3.Distance(_g.Key.transform.position, transform.position) > warpSpeedHideDistance)
                            _g.Key.SetActive(false);

            // If no longer warping (Warp is set to false from external script) switch to DECELERATE state
            if (!Warp) warpState = WarpState.DECELERATE;

            if (WarpSpeedMultiplier < maxSpeed + _ultraSpeedAddon)
                warpState = WarpState.ACCELERATE;
        }

        // DECELERATION
        if (warpState == WarpState.DECELERATE)
        {
            // If warping...
            if (WarpSpeedMultiplier > 0.0001f)
            {
                // Decreas the speed
                WarpSpeedMultiplier -= acceleration * Time.deltaTime;
                // Set all objects that were disabled (that you warped away from) to active again
                foreach (KeyValuePair<GameObject, WarpObject> _g in _warpObjects)
                    if (_g.Key != null)
                        if (!_g.Key.activeSelf) _g.Key.SetActive(true);
            }
            else
            {
                // If not warping, make sure speed is 0 
                WarpSpeedMultiplier = 0f;
                // Disable the warp renderer
                DisableRenderer();
                // Iterate though all warp objects that were translated/moved away from you during warp
                foreach (KeyValuePair<GameObject, WarpObject> _g in _warpObjects)
                {
                    // If gameobject still exists...
                    if (_g.Key != null)
                        // And it has a collider
                        if (_g.Key.GetComponent<Collider>() != null)
                            {
                                // Re-enable the collider
                                _g.Value.colliderEnabled = _g.Key.GetComponent<Collider>().enabled;
                                _g.Key.GetComponent<Collider>().enabled = true;
                            }
                }
                // Set warpstate to none
                warpState = WarpState.NONE;
            }
        }

        // If warp state is not none...
        if (warpState != WarpState.NONE)
        {
            if (audioSourceWarp != null)
            { 
                // Set the volume of the warp sound effect relative to the warp speed
                audioSourceWarp.volume = Mathf.Clamp01(WarpSpeedMultiplier / maxSpeed);
                // Set the pitch of the warp sound effect relative to the warp speed            
                audioSourceWarp.pitch = 0.5f + (WarpSpeedMultiplier / maxSpeed);
            }
            // If you are warping moving...
            if (_deltaPosition.magnitude > 0.1f)
            {
                // Iterate though all warp objects and move them relatively away from you(!) you are not actually flying fast in the scene, we are simulating it by moving other objects away from you
                foreach (KeyValuePair<GameObject, WarpObject> _g in _warpObjects)
                    if (_g.Key != null)
                        _g.Key.transform.Translate(-_deltaPosition.normalized * WarpSpeedMultiplier * Time.deltaTime * surroundingObjectMultiplier, Space.World);
                // Offset all space particles so they respawn appropriately when you stop warping as they are then out of distance
                foreach (SU_SpaceParticles _s in _spaceParticles)
                    _s.WarpParticles = -_deltaPosition * WarpSpeedMultiplier * Time.deltaTime * surroundingObjectMultiplier;
                // Set the strength of the warp shader effect
                SetStrength(Mathf.Clamp01((((WarpSpeedMultiplier / maxSpeed) * WarpSpeedMultiplier * visualWarpEffectMagnitude))));               
                // Move relatively within the space scene - this is what actually moves the SU_SpaceCamera camera component to achieve movement within the space scene
                _spaceCamera.Move(_deltaPosition.normalized * WarpSpeedMultiplier * 0.01f);
            }
            else
            {
                // Not warping, make sure warp shader effect strength is 0
                SetStrength(0f);
            }

            // If the gameobject has a rigidbody...
            if (_rigidbody != null)
            {
                // Calculate the *local* angular velocity of the object
                Vector3 localangularvelocity = transform.InverseTransformDirection(_rigidbody.angularVelocity).normalized * _rigidbody.angularVelocity.magnitude;
                // Send the angular vector to the shader so the warp distortion tube mesh can be bent by the vertex shader
                _material.SetVector(_shaderIDWarpBend, localangularvelocity);                
            }
            // Expand/shrink the tube in the vertex shader based distance between the main camera and the object that's warping (so the camera doesn't move out of the tube)
            _material.SetFloat(_shaderIDWarpRadiusMultiplier, (Vector3.Distance(Camera.main.transform.position, transform.position) / 500f) * 2.1f);
        } 
        else
        {
            // If we are not warping... set the speed of the warp particles to 0
            if (_spaceParticles != null)
                foreach (SU_SpaceParticles _s in _spaceParticles)
                    _s.WarpParticles = Vector3.zero;                    
        }
    }

    //  Method to disable the warp effect renderer
    public void DisableRenderer()
    {
        if (!_warpEffectTransform.gameObject.GetComponent<Renderer>().enabled) return;
        _warpEffectTransform.gameObject.GetComponent<Renderer>().enabled = false;
    }

    //  Method to enable the warp effect renderer
    public void EnableRenderer()
    {
        if (_warpEffectTransform.gameObject.GetComponent<Renderer>().enabled) return;
        _warpEffectTransform.gameObject.GetComponent<Renderer>().enabled = true;

    }

    // Public method to set the strength of the warp effect
    public void SetStrength(float _strength)
    {
        if (_strength > 0.0001f)
            _material.SetFloat(_shaderIDWarpStrength, _strength);
        else
            _material.SetFloat(_shaderIDWarpStrength, 0);
    }

    public void SetBrightness(float _brightness)
    {
        _material.SetFloat(_shaderIDWarpBrightness, _brightness);        
    }

    public void SetSpeed(float _speed)
    {
        _material.SetFloat(_shaderIDWarpSpeed, _speed);
    }

    public void SetUltraSpeedAddon(float _ultraSpeedAdd)
    {
        _ultraSpeedAddon = _ultraSpeedAdd;
    }

    // Procedural inverted cone capped tube mesh - *modified* version of tube: http://wiki.unity3d.com/index.php/ProceduralPrimitives
    Mesh CreateMesh()
    {
        Mesh _newMesh = new Mesh();
        _newMesh.Clear();

        float height = 100000f;
        int nbSides = 16;
        int segments = 8;

        float radius = 500f;

        int nbVerticesSides = (nbSides * (segments + 1)) + segments + 1;

        #region Vertices

        // sides
        Vector3[] vertices = new Vector3[nbVerticesSides];
        int vert = 0;
        float _2pi = Mathf.PI * 2f;

        int sideCounter = 0;
                
        while (vert < vertices.Length)
        {
            sideCounter = sideCounter == nbSides ? 0 : sideCounter;

            float r1 = (float)(sideCounter++) / nbSides * _2pi;            
            float cos = Mathf.Cos(r1);
            float sin = Mathf.Sin(r1);
            for (int _i = 0; _i < segments + 1; _i++)
            {
                float _f = 1.0f;
                if (_i == 0 || _i == segments) _f = 0f;
                vertices[vert++] = new Vector3(cos * (radius * .5f * _f), height / 2f - (((height / segments) * _i)), sin * (radius * .5f * _f));
            }
        }
        #endregion

        #region Normals

        // sides
        Vector3[] normales = new Vector3[vertices.Length];
        vert = 0;

        sideCounter = 0;
        while (vert < vertices.Length)
        {
            sideCounter = sideCounter == nbSides ? 0 : sideCounter;

            float r1 = (float)(sideCounter++) / nbSides * _2pi;

            for (int _i = 0; _i < segments + 1; _i++)
            {
                normales[vert++] = -(new Vector3(Mathf.Cos(r1), 0f, Mathf.Sin(r1)));
            }
        }
        #endregion

        #region UVs
        Vector2[] uvs = new Vector2[vertices.Length];

        vert = 0;

        // Sides 
        sideCounter = 0;
        while (vert < vertices.Length)
        {
            for (int _i = 0; _i < segments + 1; _i++)
            {
                float t = (float)(sideCounter++) / nbSides;
                uvs[vert++] = new Vector2(t, ((1f / (segments + 1f))) * _i);
            }
        }
        #endregion

        #region Triangles
        int nbFace = nbSides * segments;
        int nbTriangles = nbFace * 2;
        int nbIndexes = nbTriangles * 3;
        int[] triangles = new int[nbIndexes];
        sideCounter = 0;
        int n = 0;
        int neg = 0;
        // Sides 

        for (int _s = 0; _s < nbSides; _s++)
        {
            for (int _i = 0; _i < segments; _i++)
            {
                triangles[n++] = segments + neg + 1;
                triangles[n++] = neg + 1;
                triangles[n++] = neg;
                
                triangles[n++] = segments + neg + 2;
                triangles[n++] = neg + 1;
                triangles[n++] = segments + neg + 1;

                neg++;
            }
            neg++;
        }
        #endregion

        _newMesh.vertices = vertices;
        _newMesh.normals = normales;
        _newMesh.uv = uvs;
        _newMesh.triangles = triangles;

        _newMesh.RecalculateBounds();
        ;

        return _newMesh;
    }
}
