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
using TMPro;
using System;

public class ResultsDBAccess : MonoBehaviour
{

    private MongoClient client;
	private IMongoDatabase database;
    private IMongoDatabase Tdatabase;
	private IMongoCollection<ansDetails> ansCollection;
    private IMongoCollection<BsonDocument> studentRank;
    private IMongoCollection<leadDetails> leadCollection;
    private IMongoCollection<SDetails> SCollection;
    private IMongoCollection<progDetails> progCollection;
    public int studentScore;


    public TextMeshProUGUI Question1;
    public TextMeshProUGUI YourAns1;
    public TextMeshProUGUI CorrectAns1;

    public TextMeshProUGUI Question2;
    public TextMeshProUGUI YourAns2;
    public TextMeshProUGUI CorrectAns2;

    public TextMeshProUGUI Question3;
    public TextMeshProUGUI YourAns3;
    public TextMeshProUGUI CorrectAns3;

    public TextMeshProUGUI Question4;
    public TextMeshProUGUI YourAns4;
    public TextMeshProUGUI CorrectAns4;

    public class ansDetails
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string dbUserName { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string question { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string correctAnswer { get; set; }

        [BsonRepresentation(BsonType.String)]
		public string yourAnswer { get; set; }
        [BsonRepresentation(BsonType.String)]
		public string dbPlanet { get; set; }
        [BsonRepresentation(BsonType.String)]
		public string dbDifficulty { get; set; }
	}



    public class leadDetails
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string id { get; set; }
		[BsonRepresentation(BsonType.ObjectId)]
		public string dbUserName { get; set; }
		[BsonRepresentation(BsonType.Int32)]
		public int score { get; set; }
	}

    public class SDetails
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string id { get; set; }
		[BsonRepresentation(BsonType.ObjectId)]
		public string dbUserName { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public int score { get; set; }

	}

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
        //temporarily hardcoded
        // Progress.currentPlanet = 1;
        // Monster.currentMonster = "Easy";
        // Login.UserNameS = "swati";

		// Database Access 
		client = new MongoClient("mongodb+srv://Student:ssad@cluster.dibtk.mongodb.net/GalaxSEdb?retryWrites=true&w=majority");
		database = client.GetDatabase("Student");
        Tdatabase = client.GetDatabase("SpaceGo");
		Debug.Log("Connected to DB!");

        ansCollection = database.GetCollection<ansDetails>("ansChose");

        SCollection = Tdatabase.GetCollection<SDetails>("students");


		var fil = Builders<ansDetails>.Filter.Eq("StudentUsername", Login.UserNameS);
        fil = fil & Builders<ansDetails>.Filter.Eq("Planet", Progress.currentPlanet); 
        fil = fil & Builders<ansDetails>.Filter.Eq("Difficulty", Monster.currentMonster);
		var projection = Builders<ansDetails>.Projection.Include("Question").Include("YourAnswer").Include("RightAnswer").Exclude("_id");


		var result = ansCollection.Find<ansDetails>(fil).Project(projection).ToList();
        var numResults = ansCollection.Find<ansDetails>(fil).Count();

        // Debug.Log("1st here");

        var no = (int)numResults;

        for (int i = no-1; i > no - 5; i--)
		{
            if (i == no-1){
                Question1.text = result[i]["Question"].ToString();
                YourAns1.text = result[i]["YourAnswer"].ToString();
                CorrectAns1.text = result[i]["RightAnswer"].ToString();
            } else if (i == no-2) {
                Question2.text = result[i]["Question"].ToString();
                YourAns2.text = result[i]["YourAnswer"].ToString();
                CorrectAns2.text = result[i]["RightAnswer"].ToString();
            } else if (i == no-3) {
                Question3.text = result[i]["Question"].ToString();
                YourAns3.text = result[i]["YourAnswer"].ToString();
                CorrectAns3.text = result[i]["RightAnswer"].ToString();
            } else if (i == no-4) {
                Question4.text = result[i]["Question"].ToString();
                YourAns4.text = result[i]["YourAnswer"].ToString();
                CorrectAns4.text = result[i]["RightAnswer"].ToString();
            }
		}

        // Debug.Log("2nd here");

        studentScore = 0;

        progCollection = database.GetCollection<progDetails>("userProgress");
        var filProg = Builders<progDetails>.Filter.Eq("UserName", Login.UserNameS);
        var projectionProg = Builders<progDetails>.Projection.Include("UnlockedPlanet").Include("PlanetCount").Exclude("_id");

        var resultProg = progCollection.Find<progDetails>(filProg).Project(projectionProg).ToList();
        
        // Debug.Log("weeeeeeeee-----");
        // Debug.Log(Progress.unlockedPlanet.ToString());
        // Debug.Log(Progress.planetCount.ToString());

         if (Monster.currentMonster =="Easy"){
            for (int j= no-1; j > no-5; j--){
                if (String.Equals(result[j]["YourAnswer"].ToString(),result[j]["RightAnswer"].ToString())){
                    studentScore+=100;
                    if (Progress.currentPlanet == Progress.unlockedPlanet) {
                        Progress.planetCount += 1;
                    }
                    
                } 
            }

            //write back
            var updateProg = Builders<progDetails>.Update.Set("PlanetCount", Progress.planetCount);
            progCollection.UpdateOne(filProg, updateProg);

        } 
        else if (Monster.currentMonster == "Medium"){
            for (int j= no-1; j > no-5; j--){
                if (String.Equals(result[j]["YourAnswer"].ToString(),result[j]["RightAnswer"].ToString())){
                    studentScore+=200;
                    if (Progress.currentPlanet == Progress.unlockedPlanet) {
                        Progress.planetCount += 1;
                    }
                } 
            }

            //write back
            var updateProg = Builders<progDetails>.Update.Set("PlanetCount", Progress.planetCount);
            progCollection.UpdateOne(filProg, updateProg);

        } else if (Monster.currentMonster=="Hard"){
            for (int j= no-1; j > no-5; j--){
                if (String.Equals(result[j]["YourAnswer"].ToString(),result[j]["RightAnswer"].ToString())){
                    studentScore+=300;
                    if (Progress.currentPlanet == Progress.unlockedPlanet) {
                        Progress.planetCount += 1;
                    }
                } 
            }
            
            //write back
            var updateProg = Builders<progDetails>.Update.Set("PlanetCount", Progress.planetCount);

            progCollection.UpdateOne(filProg, updateProg);
        }

        

        if(Progress.planetCount >= 9){
           
            Progress.unlockedPlanet= Progress.unlockedPlanet+ 1;
            // Debug.Log("these are these whats happening here-----");
            // Debug.Log(Progress.unlockedPlanet.ToString());
            // Debug.Log(Progress.planetCount.ToString());

            Progress.planetCount = 0;
            //write back unlocked planet and planet count
            var updateProg = Builders<progDetails>.Update.Set("PlanetCount", Progress.planetCount);
            progCollection.UpdateOne(filProg, updateProg);

            var updateUnlocked = Builders<progDetails>.Update.Set("UnlockedPlanet", Progress.unlockedPlanet);
            progCollection.UpdateOne(filProg, updateUnlocked);

        }
        else{
            Debug.Log("Planet Count less than 9");
            
        }
    //get score to add and update CHECK

        // studentRank = database.GetCollection<BsonDocument>("studentRanking");

        leadCollection = database.GetCollection<leadDetails>("studentRanking");

        var filter = Builders<leadDetails>.Filter.Eq("Name", Login.UserNameS);
        var Tfilter = Builders<SDetails>.Filter.Eq("Username", Login.UserNameS);

		var projectionRank = Builders<leadDetails>.Projection.Include("Name").Include("Score").Exclude("_id");

		var rankResult = leadCollection.Find<leadDetails>(filter).Project(projectionRank).ToList();

        var num_cnt = leadCollection.Find<leadDetails>(filter).Project(projectionRank).Count();

        var temp = rankResult[0][1].ToInt32();
        studentScore =  temp + studentScore;

        var update = Builders<leadDetails>.Update.Set("Score", studentScore);
        var Tupdate = Builders<SDetails>.Update.Set("Score", studentScore);
        leadCollection.UpdateOne(filter, update);
        SCollection.UpdateOne(Tfilter, Tupdate);


    }
    void Update()
    {
        


    }
    public int[] checkDB(string UserName){
        client = new MongoClient("mongodb+srv://Student:ssad@cluster.dibtk.mongodb.net/GalaxSEdb?retryWrites=true&w=majority");
		database = client.GetDatabase("Student");

        progCollection = database.GetCollection<progDetails>("userProgress");

        var fil = Builders<progDetails>.Filter.Eq("UserName", UserName);

        var projection = Builders<progDetails>.Projection.Include("UnlockedPlanet").Include("PlanetCount").Exclude("_id");
        var result = progCollection.Find<progDetails>(fil).Project(projection).ToList();
 
        var planetCount = result[0]["PlanetCount"].ToInt32();
        var unlockedPlanet = result[0]["UnlockedPlanet"].ToInt32();
        int [] arr = new int[]{planetCount, unlockedPlanet};
        return arr;
    }
}












