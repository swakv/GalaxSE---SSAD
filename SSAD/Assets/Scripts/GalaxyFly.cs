using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Core.Misc;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
 using UnityEngine.SceneManagement;


using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]

public class GalaxyFly : MonoBehaviour
{
    public float normalSpeed = 25f;
    public float accelerationSpeed = 35f;
    public Transform cameraPosition;
    public Camera mainCamera;
    public Transform astro;
    public float rotationSpeed = 2.0f;
    public float cameraSmooth = 4f;

    float speed;
    Rigidbody r;
    Quaternion lookRotation;
    float rotationZ = 0;
    float mouseXSmooth = 0;
    float mouseYSmooth = 0;
    Vector3 defaultShipRotation;
    public Material[] materials = new Material[5];
    Renderer rd;
    
    //private Vector3 myPosition = new Vector3((float)0,(float)2.2,(float)0); 
    private Vector3 myPosition = new Vector3((float)0,(float)-0.6,(float)2.2); 

    private Quaternion myQua = new Quaternion((float)0, (float)0, (float)0, (float)0);
    public GameObject cowboy;
    public GameObject crown;
    public GameObject mag;
    public GameObject miner;
    private GameObject hat;
    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<userProfile> UPcollection;
    private Vector3 homePos = new Vector3(0,0,0);
    private Quaternion homeQua = new Quaternion((float)0, (float)0, (float)0, (float)0);

    public string colorDB;
    public int hatDB;
    public Button centre;

    // Start is called before the first frame update
    void Start()
    {
        centre.onClick.AddListener(TaskOnCentre);
        client = new MongoClient("mongodb+srv://Student:ssad@cluster.dibtk.mongodb.net/GalaxSEdb?retryWrites=true&w=majority");
        database = client.GetDatabase("Student");
        UPcollection = database.GetCollection<userProfile>("userprofile");
        Debug.LogWarning("Connected to db!");
    
        //Transform currentObject = null;
        rd = astro.GetComponent<Renderer>();
        r = GetComponent<Rigidbody>();
        r.useGravity = false;
        lookRotation = transform.rotation;
        defaultShipRotation = astro.localEulerAngles;
        rotationZ = defaultShipRotation.z;

        Login.UserNameS = "swati";
        var filterForDetails = Builders<userProfile>.Filter.Eq("username", Login.UserNameS);
        var projection = Builders<userProfile>.Projection.Include("color").Include("hatID").Exclude("_id");
        var result = UPcollection.Find<userProfile>(filterForDetails).Project(projection).ToList();
        colorDB = result[0][0].ToString();
        hatDB = result[0][1].ToInt32();
        setFromDB();
        // if(Input.GetKeyDown("space")){
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false; 
        // } 
    }
    public void TaskOnCentre(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);   
    }
    public class userProfile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string username { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string color { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public int hatID { get; set; }
    }
     void FixedUpdate()
    {
        //press space to toggle cursor
        // if(Input.GetKeyDown("space")){
        //     if (isLocked==0){
        //         Cursor.lockState = CursorLockMode.Locked;
        //         Cursor.visible = false; 
        //         isLocked=1;
        //     }
        //     else{
        //         Cursor.lockState = CursorLockMode.None;
        //         Cursor.visible = true; 
        //         isLocked=0;
        //     }
        
        // } 

        //Press Right Mouse Button to accelerate
        if (Input.GetMouseButton(1))
        {
            speed = Mathf.Lerp(speed, accelerationSpeed, Time.deltaTime * 3);
        }
        else
        {
            speed = Mathf.Lerp(speed, normalSpeed, Time.deltaTime * 10);
        }

        //Set moveDirection to the vertical axis (up and down keys) * speed
        Vector3 moveDirection = new Vector3(0, 0, speed);
        //Transform the vector3 to local space
        moveDirection = transform.TransformDirection(moveDirection);
        //Set the velocity, so you can move
        r.velocity = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);

        //Camera follow
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPosition.position, Time.deltaTime * cameraSmooth);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, cameraPosition.rotation, Time.deltaTime * cameraSmooth);

        //Rotation
        float rotationZTmp = 0;
        if (Input.GetKey(KeyCode.A))
        {
            rotationZTmp = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotationZTmp = -1;
        }
        mouseXSmooth = Mathf.Lerp(mouseXSmooth, Input.GetAxis("Horizontal") * rotationSpeed, Time.deltaTime * cameraSmooth);
        mouseYSmooth = Mathf.Lerp(mouseYSmooth, Input.GetAxis("Vertical") * rotationSpeed, Time.deltaTime * cameraSmooth);
        Quaternion localRotation = Quaternion.Euler(-mouseYSmooth, mouseXSmooth, rotationZTmp * rotationSpeed);
        lookRotation = lookRotation * localRotation;
        transform.rotation = lookRotation;
        rotationZ -= mouseXSmooth;
        rotationZ = Mathf.Clamp(rotationZ, -45, 45);
        astro.transform.localEulerAngles = new Vector3(defaultShipRotation.x, defaultShipRotation.y, rotationZ);
        rotationZ = Mathf.Lerp(rotationZ, defaultShipRotation.z, Time.deltaTime * cameraSmooth);
    
    }

    public void setFromDB(){
        //colorDB = "pink";
        //hatDB = 3;
        if (hatDB==1){
            hat = Instantiate(miner, myPosition, myQua);
            hat.transform.parent = GameObject.FindWithTag("Astro").transform;
        } else if (hatDB==2){
            hat = Instantiate(cowboy, myPosition, myQua);
            hat.transform.parent = GameObject.FindWithTag("Astro").transform;
        } else if (hatDB==3){
            hat = Instantiate(crown, myPosition, myQua);
            hat.transform.parent = GameObject.FindWithTag("Astro").transform;
        } else if (hatDB==4){
            hat = Instantiate(mag, myPosition, myQua);
            hat.transform.parent = GameObject.FindWithTag("Astro").transform;
        } else {Debug.LogWarning("No entry in DB");}

        if (colorDB=="Pink"){
            rd.material= materials[0];
        } else if (colorDB=="Blue"){
            rd.material= materials[1];
        } else if (colorDB=="Green"){
            rd.material= materials[2];
        } else if (colorDB=="Red"){
            rd.material= materials[3];  
        } else if (colorDB=="Black"){
            rd.material= materials[4];  
        } else {Debug.LogWarning("No entry in DB");}
    }

    


    // Update is called once per frame
    void Update()
    {
        
    }
}
