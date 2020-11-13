using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

public class UserProfile : MonoBehaviour
{
    // Database access functions
    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<userProfile> UPcollection;

    public string colorSelected;
    public int hatSelected;

    // Color buttons
    public Button PinkButton;
    public Button BlueButton;
    public Button GreenButton;
    public Button RedButton;
    public Button BlackButton;
    public Button SaveButton;
   

    /*
    public Button HatOneButton;
    public Button HatTwoButton;
    public Button HatThreeHButton;
    public Button HatFourButton;
    */

    public class userProfile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string username { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string color { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public int hatID { get; set; }
    }

    public void saveButtonClick(){

        UPcollection = database.GetCollection<userProfile>("userprofile");

        Login.UserNameS = "swati";
        var filter = new BsonDocument("Username", Login.UserNameS);
		long findUsername = UPcollection.Find(filter).Count();

        if((findUsername == 1))
        {
            var fil = Builders<userProfile>.Filter.Eq("username", Login.UserNameS);
            var update = Builders<userProfile>.Update.Set("color", colorSelected).Set("hatID", hatSelected);
            UPcollection.UpdateOne(fil, update);
            Debug.LogWarning("Userprofile updated to DB!");
        }
        else
        {
            UPcollection.InsertOne(new userProfile { username = Login.UserNameS, color = colorSelected, hatID = hatSelected });
            Debug.LogWarning("Userprofile added to DB!");
        } 
    }

    public void PinkButtonClick()
    {
        colorSelected = "Pink";
        Debug.LogWarning("Pink button has been clicked");
    }
    public void BlueButtonClick()
    {
        colorSelected = "Blue";
        Debug.LogWarning("Blue button has been clicked");
    }
    public void GreenButtonClick()
    {
        colorSelected = "Green";
        Debug.LogWarning("Green button has been clicked");
    }
    public void RedButtonClick()
    {
        colorSelected = "Red";
        Debug.LogWarning("Red button has been clicked");
    }
    public void BlackButtonClick()
    {
        colorSelected = "Black";
        Debug.LogWarning("Black button has been clicked");
    }


    //public void honebuttonclick()
    //{
    //    hatselected = 1;
    //}
    //public void htwobuttonclick()
    //{
    //    hatselected = 2;
    //}
    //public void hthreebuttonclick()
    //{
    //    hatselected = 3;
    //}
    //public void hfourbuttonclick()
    //{
    //    hatselected = 4;
    //}

    // Start is called before the first frame update
    void Start()
    {
        // Database Access
        client = new MongoClient("mongodb+srv://Student:ssad@cluster.dibtk.mongodb.net/GalaxSEdb?retryWrites=true&w=majority");
        database = client.GetDatabase("Student");
        Debug.LogWarning("Connected to db!");

        /*
        Button PinkButton = PButton.GetComponent<Button>();
        Button BlueButton = BButton.GetComponent<Button>();
        Button GreenButton = GButton.GetComponent<Button>();
        Button RedButton = RButton.GetComponent<Button>();
        Button BlackButton = BkButton.GetComponent<Button>();
        Button SaveButton = SButton.GetComponent<Button>();
        */

        /*
        Button HoneButton = HatOneButton.GetComponent<Button>();
        Button HtwoButton = HatTwoButton.GetComponent<Button>();
        Button HthreeButton = HatThreeHButton.GetComponent<Button>();
        Button HfourButton = HatFourButton.GetComponent<Button>();
        */

    }

    // Update is called once per frame
    void Update()
    {
        //color button selected 
        PinkButton.onClick.AddListener(PinkButtonClick);
        BlueButton.onClick.AddListener(BlueButtonClick);
        GreenButton.onClick.AddListener(GreenButtonClick);
        RedButton.onClick.AddListener(RedButtonClick);
        BlackButton.onClick.AddListener(BlackButtonClick);

        //hat button selected 
        /*    
        HoneButton.onClick.AddListener(HoneButtonClick());
        HtwoButton.onClick.AddListener(HtwoButtonClick());
        HthreeButton.onClick.AddListener(HthreeButtonClick());
        HfourButton.onClick.AddListener(HfourButtonClick());
        */

        // SaveButton.GetComponent<Button>().onClick.AddListener(saveButtonClick());
    }
}
