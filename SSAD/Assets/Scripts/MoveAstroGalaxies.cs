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

public class MoveAstroGalaxies : MonoBehaviour
{
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

    public string colorDB;
    public int hatDB;

    private float speed = 60.0f;
    private float zoomSpeed = 20.0f;
    public GameObject astro;
    // public Button centre;

    void Start(){
        // centre.onClick.AddListener(TaskOnCentre);
        client = new MongoClient("mongodb+srv://Student:ssad@cluster.dibtk.mongodb.net/GalaxSEdb?retryWrites=true&w=majority");
        database = client.GetDatabase("Student");
        UPcollection = database.GetCollection<userProfile>("userprofile");
        Debug.LogWarning("Connected to db!");
    
        rd = astro.GetComponent<Renderer>();
        Login.UserNameS = "swati";
        var filterForDetails = Builders<userProfile>.Filter.Eq("username", Login.UserNameS);
        var projection = Builders<userProfile>.Projection.Include("color").Include("hatID").Exclude("_id");
        var result = UPcollection.Find<userProfile>(filterForDetails).Project(projection).ToList();
        colorDB = result[0][0].ToString();
        hatDB = result[0][1].ToInt32();
        setFromDB();

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
    public void TaskOnCentre(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);   
    }
    void Update () {
    
            if (Input.GetKey(KeyCode.D)){
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A)){
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.W)){
                transform.position += Vector3.forward * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S)){
                transform.position += Vector3.back * speed * Time.deltaTime;
            }
            float scroll = Input.GetAxis("Mouse ScrollWheel");
		    transform.Translate(0, scroll * zoomSpeed, scroll * zoomSpeed, Space.World);
        }
    public Vector3 makeMove(GameObject g, string keyCode) {
        Debug.Log("We're here");
        if (keyCode=="D")
            g.transform.position += Vector3.right * 1 * 1;
        if (keyCode=="A")
            g.transform.position += Vector3.left * 1 * 1;
        if (keyCode=="W") { 
            g.transform.position += Vector3.forward * 1 * 1;
            Debug.Log("kaod");
        }
        if (keyCode=="S")
            g.transform.position += Vector3.back * 1 * 1;
        return g.transform.position; 
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
}


