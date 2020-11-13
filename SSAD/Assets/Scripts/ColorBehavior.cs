using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Core.Misc;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Threading;

public class ColorBehavior : MonoBehaviour
{
     // Database access functions
    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<userProfile> UPcollection;

    public string colorSelected="Default";
    public int hatSelected=0;
    


    public Transform astronaut;
    public Material[] materials = new Material[5]; //for 5 materials
    Renderer rd;
    public Button pinkBut;
    public Button blueBut, greenBut, redBut, blackBut, saveBut, minerBut, cowBut, crownBut, magBut;
    
    public GameObject minerHat;
    public TextMeshProUGUI console;
    public GameObject cowboy;
    public GameObject crown;
    public GameObject mag;
    public int width = 10;
    public int height = 4;
    public string colorDB;
    public int hatDB;
    private Vector3 myPosition = new Vector3((float)-0.75,(float)5,(float)3.5); 
    private Quaternion myQua = new Quaternion((float)0, (float)-0.8, (float)-0.1, (float)0.01);
    // Start is called before the first frame update
    void Start()
    {
        client = new MongoClient("mongodb+srv://Student:ssad@cluster.dibtk.mongodb.net/GalaxSEdb?retryWrites=true&w=majority");
        database = client.GetDatabase("Student");
        UPcollection = database.GetCollection<userProfile>("userprofile");
        Debug.LogWarning("Connected to db!");
        //Transform currentObject = null;
        rd = astronaut.GetComponent<Renderer>();
        
        saveBut.onClick.AddListener(TaskSave);
        pinkBut.onClick.AddListener(TaskOnPinkClick);
        blueBut.onClick.AddListener(TaskOnBlueClick);
        greenBut.onClick.AddListener(TaskOnGreenClick);
        redBut.onClick.AddListener(TaskOnRedClick);
        blackBut.onClick.AddListener(TaskOnBlackClick);

        minerBut.onClick.AddListener(TaskOnMinerClick);
        cowBut.onClick.AddListener(TaskOnCowboyClick);
        crownBut.onClick.AddListener(TaskOnCrownClick);
        magBut.onClick.AddListener(TaskOnMagClick);

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

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TaskOnPinkClick()
    {
        colorSelected="Pink";
        Debug.Log("You have clicked the pink button!");
        rd.material= materials[0];
    }
    public void TaskOnBlueClick()
    {
        colorSelected="Blue";
        Debug.Log("You have clicked the blue button!");
        rd.material= materials[1];
    }
    public void TaskOnGreenClick()
    {
        colorSelected="Green";
        Debug.Log("You have clicked the green button!");
        rd.material= materials[2];
    }
    public void TaskOnRedClick()
    {
        colorSelected="Red";
        Debug.Log("You have clicked the red button!");
        rd.material= materials[3];
    }
    public void TaskOnBlackClick()
    {
        colorSelected="Black";
        Debug.Log("You have clicked the black button!");
        rd.material= materials[4];
    }

    // adding hats:
    public void TaskOnMinerClick()
    {
        InteractableAll();
        destroyHat();
        hatSelected=1;
        //Vector3 position = new Vector3((float)-5.317549e-14, (float)20.001009145, (float)-1.903477e-15);
        //y=.001252363
        Debug.Log("You have clicked the miner hat button!");
        Vector3 minerPosition = new Vector3((float)-0.75,(float)5,(float)3.5); 
        Quaternion minerQua = new Quaternion((float)0, (float)-0.8, (float)-0.1, (float)0.01);
        // Vector3 temp = transform.rotation.eulerAngles;
        // temp.x = (float)-2.942;
        // temp.y= (float)179.785;
        // temp.z=(float) 0.14;
        // transform.rotation = Quaternion.Euler(temp);
        GameObject minerH = Instantiate(minerHat, myPosition, myQua);
        minerH.transform.parent = GameObject.FindWithTag("Astro").transform;
        minerH.gameObject.tag = ("MinerHat");
        minerBut.interactable=false;
    }

    public void TaskOnCowboyClick()
    {
        InteractableAll();
        destroyHat();
        hatSelected=2;
        Vector3 position = new Vector3((float)-5.317549e-14, (float)20.001009145, (float)-1.903477e-15);
        Debug.Log("You have clicked the cowboy hat button!");
        //Instantiate(cowboy,  position, Quaternion.identity);
        GameObject cowboyH = Instantiate(cowboy, myPosition, myQua);
        cowboyH.transform.parent = GameObject.FindWithTag("Astro").transform;
        cowboyH.gameObject.tag = ("Cowboy");
        cowBut.interactable=false;
    }   
    public void TaskOnCrownClick()
    {
        InteractableAll();
        destroyHat();
        hatSelected=3;
        Vector3 position = new Vector3((float)-5.317549e-14, (float)20.001009145, (float)-1.903477e-15);
        Debug.Log("You have clicked the crown hat button!");
        GameObject crownH = Instantiate(crown, myPosition, myQua);
        crownH.transform.parent = GameObject.FindWithTag("Astro").transform;
        crownH.gameObject.tag = ("Crown");
        crownBut.interactable=false;
    }
    public void TaskOnMagClick()
    {
        InteractableAll();
        destroyHat();
        hatSelected=4;
        Vector3 position = new Vector3((float)-5.317549e-14, (float)20.001009145, (float)-1.903477e-15);
        Debug.Log("You have clicked the magician hat button!");
        GameObject magH = Instantiate(mag,  myPosition, myQua);
        magH.transform.parent = GameObject.FindWithTag("Astro").transform;
        magH.gameObject.tag=("Mag");
        magBut.interactable=false;
    }
    public void destroyHat(){
        string[] tags = {"MinerHat","Cowboy", "Crown","Mag"};
        List<string> tagsList = new List<string>(tags);

        for (int i=0; i<4; i++){
            GameObject someHat;
            if (GameObject.FindWithTag(tagsList[i])!=null)
            {
                someHat = GameObject.FindWithTag(tagsList[i]);
                Destroy(someHat);
                Debug.Log("Old hat removed!");
            }
        }
    }
    public void InteractableAll(){
        minerBut.interactable=true;
        cowBut.interactable=true;
        crownBut.interactable=true;
        magBut.interactable=true;
    }
    public void setFromDB(){
        //colorDB = "pink";
        //hatDB = 3;

        if (hatDB==1){
            TaskOnMinerClick();
        } else if (hatDB==2){
            TaskOnCowboyClick();
        } else if (hatDB==3){
            TaskOnCrownClick();
        } else if (hatDB==4){
            TaskOnMagClick();  
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

    
    public void TaskSave()
    {
         UPcollection = database.GetCollection<userProfile>("userprofile");

        Login.UserNameS = "swati";
        var filter = new BsonDocument("username", Login.UserNameS);
		long findUsername = UPcollection.Find(filter).Count();

        if (findUsername == 1)
        {
            var fil = Builders<userProfile>.Filter.Eq("username", Login.UserNameS);
            var update = Builders<userProfile>.Update.Set("color", colorSelected).Set("hatID", hatSelected);
            UPcollection.UpdateOne(fil, update);
            Debug.LogWarning("Userprofile updated to DB!");
        }
        else 
        {
            UPcollection.InsertOne(new userProfile { username = Login.UserNameS, color = colorSelected, hatID = hatSelected });
            Debug.LogWarning("User userprofile added to db!");
        } 
        Debug.Log("Changes saved.");
    }
}
