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

public class Progress : MonoBehaviour
{
  private MongoClient client;
	private IMongoDatabase database;
  private IMongoCollection<progDetails> progCollection;

  public static int unlockedPlanet;
  public static int currentPlanet;
  public static int planetCount;

   public class progDetails
	{
  	[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string id { get; set; }
		[BsonRepresentation(BsonType.ObjectId)]
		public string dbUserName { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public int dbUnlockedP { get; set; }
        
    [BsonRepresentation(BsonType.Int32)]
		public int dbPlanetCount { get; set; }
	}

  void Start(){
    client = new MongoClient("mongodb+srv://Student:ssad@cluster.dibtk.mongodb.net/GalaxSEdb?retryWrites=true&w=majority");
		database = client.GetDatabase("Student");
    Debug.Log("Connected to DB IN PROGRESS!");

    progCollection = database.GetCollection<progDetails>("userProgress");

    var fil = Builders<progDetails>.Filter.Eq("UserName", Login.UserNameS);

    var projection = Builders<progDetails>.Projection.Include("UnlockedPlanet").Include("PlanetCount").Exclude("_id");


		var result = progCollection.Find<progDetails>(fil).Project(projection).ToList();
    var numResults = progCollection.Find<progDetails>(fil).Count();
    Debug.Log("In progress, numResults is ");
    Debug.Log(numResults);


    Debug.Log(result[0]["UnlockedPlanet"].ToInt32());
    unlockedPlanet = result[0]["UnlockedPlanet"].ToInt32();
    currentPlanet = unlockedPlanet;
    planetCount = result[0]["PlanetCount"].ToInt32();


  }

}
