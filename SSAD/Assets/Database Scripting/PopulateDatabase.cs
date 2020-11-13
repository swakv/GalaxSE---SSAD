using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Core.Misc;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
// using static UnityEngine.Random;
// using static System.Random;

public class PopulateDatabase : MonoBehaviour
{
  private MongoClient client;
	private IMongoDatabase database;
  private IMongoCollection<progDetails> progCollection;
  private IMongoCollection<loginDetails> loginCollection;
  private IMongoCollection<rankDetails> rankCollection;
  private IMongoCollection<profDetails> profCollection;


  public class loginDetails
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string id { get; set; }
		[BsonRepresentation(BsonType.String)]
		public string Username { get; set; }
		[BsonRepresentation(BsonType.String)]
		public string Password { get; set; }
	}

    public class rankDetails
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string id { get; set; }
		[BsonRepresentation(BsonType.String)]
		public string Name { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public int Score { get; set; }

	}

  public class progDetails
	{
  	[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string id { get; set; }
		[BsonRepresentation(BsonType.String)]
		public string UserName { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public int UnlockedPlanet { get; set; }
        
    [BsonRepresentation(BsonType.Int32)]
		public int PlanetCount { get; set; }
	}

  public class profDetails
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

  void Start(){
    client = new MongoClient("mongodb+srv://Student:ssad@cluster.dibtk.mongodb.net/GalaxSEdb?retryWrites=true&w=majority");
		database = client.GetDatabase("StudentTemp");
    Debug.Log("Connected to DB IN POPULATEDATABASE!");

    int numEntries = 50;
    for (int entry = 0; entry < numEntries; entry++){

      //generate random string for username
      var rString = "";
      for (var i = 0; i < 5; i++){
          rString += ((char)(Random.Range(1, 26) + 64)).ToString().ToLower();
      }
      int rNum = Random.Range(100,1000);
      string rUserName = rString + rNum;

      //generate random alphanumeric string for password
      const string glyphs= "abcdefghijklmnopqrstuvwxyz0123456789!@#$%&?";
      int charAmount = Random.Range(6, 8); 
      string rPassword = "";
      for(int i=0; i<charAmount; i++){
          rPassword += glyphs[Random.Range(0, glyphs.Length)];
      }

      //generate score
      int rScore = Random.Range(0,2000);
      int numSteps = (int)Mathf.Floor (rScore/100);
      int adjustedScore = numSteps * 100;

      //generate unlockedPlanet
      int rUnlocked = Random.Range(1,16);

      //generate planetCount
      int rPlanetCount = Random.Range(0,4);

      //generate hatID
      int rHatID = Random.Range(1,4);
      
      //generate colour
      string[] colours;
      colours = new string[5]{ "pink", "blue", "black", "red", "green"};
      int rIndex = Random.Range(0,4);
      string rColour = colours[rIndex];
      


    //insert into database
    progCollection = database.GetCollection<progDetails>("userProgressTemp");
    profCollection = database.GetCollection<profDetails>("userprofileTemp");
    rankCollection = database.GetCollection<rankDetails>("studentRankingTemp");
    loginCollection = database.GetCollection<loginDetails>("loginTemp");

    loginCollection.InsertOne(new loginDetails{ Username = rUserName, Password = rPassword });

    progCollection.InsertOne(new progDetails{ UserName = rUserName, UnlockedPlanet = rUnlocked, PlanetCount = rPlanetCount });

    rankCollection.InsertOne(new rankDetails{ Name = rUserName, Score = adjustedScore });

    profCollection.InsertOne(new profDetails{ username = rUserName, color = rColour, hatID = rHatID });
   





    }

  }

}
