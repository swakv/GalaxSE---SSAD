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


public class LBoardDBAccess : MonoBehaviour
{
    private MongoClient client;
	private IMongoDatabase database;
	private IMongoCollection<leadDetails> leadCollection;

    

    public TextMeshProUGUI Name1;
    public TextMeshProUGUI Score1;
    public TextMeshProUGUI Name2;
    public TextMeshProUGUI Score2;
    public TextMeshProUGUI Name3;
    public TextMeshProUGUI Score3;
    public TextMeshProUGUI Name4;
    public TextMeshProUGUI Score4;
    public TextMeshProUGUI Name5;
    public TextMeshProUGUI Score5;
    public TextMeshProUGUI Name6;
    public TextMeshProUGUI Score6;
    public TextMeshProUGUI Name7;
    public TextMeshProUGUI Score7;
    public TextMeshProUGUI Name8;
    public TextMeshProUGUI Score8;
    public TextMeshProUGUI Name9;
    public TextMeshProUGUI Score9;
    public TextMeshProUGUI Name10;
    public TextMeshProUGUI Score10;


    public class leadDetails
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string id { get; set; }
		[BsonRepresentation(BsonType.ObjectId)]
		public string dbUserName { get; set; }

		[BsonRepresentation(BsonType.String)]
		public int score { get; set; }

	}


    void Start(){
		// Database Access 
		client = new MongoClient("mongodb+srv://Student:ssad@cluster.dibtk.mongodb.net/GalaxSEdb?retryWrites=true&w=majority");
		database = client.GetDatabase("Student");
		Debug.Log("Connected to DB!");

        leadCollection = database.GetCollection<leadDetails>("studentRanking");
		var projection = Builders<leadDetails>.Projection.Include("Name").Include("Score").Exclude("_id");

        var sort = Builders<leadDetails>.Sort.Descending("Score");

		var result = leadCollection.Find(new BsonDocument()).Sort(sort).Project(projection).ToList();
        // var numResults = leadCollection.Find(new BsonDocument()).Count();
        
        Debug.Log(result);
        for (int i = 0; i < 10; i++)
		{     
            if (i == 0){
                Name1.text = result[i]["Name"].ToString();
                Score1.text = result[i]["Score"].ToString();
            } else if (i == 1){
                Name2.text = result[i]["Name"].ToString();
                Score2.text = result[i]["Score"].ToString();
            } else if (i == 2){
                Name3.text = result[i]["Name"].ToString();
                Score3.text = result[i]["Score"].ToString();
            } else if (i == 3){
                Name4.text = result[i]["Name"].ToString();
                Score4.text = result[i]["Score"].ToString();
            } else if (i == 4){
                Name5.text = result[i]["Name"].ToString();
                Score5.text = result[i]["Score"].ToString();
            } else if (i == 5){
                Name6.text = result[i]["Name"].ToString();
                Score6.text = result[i]["Score"].ToString();
            } else if (i == 6){
                Name7.text = result[i]["Name"].ToString();
                Score7.text = result[i]["Score"].ToString();
            } else if (i == 7){
                Name8.text = result[i]["Name"].ToString();
                Score8.text = result[i]["Score"].ToString();
            } else if (i == 8){
                Name9.text = result[i]["Name"].ToString();
                Score9.text = result[i]["Score"].ToString();
            } else if (i == 9){
                Name10.text = result[i]["Name"].ToString();
                Score10.text = result[i]["Score"].ToString();
            }
		}



    }
    void Update()
    {
        


    }
}












