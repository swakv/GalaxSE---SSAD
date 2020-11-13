
using System.Collections;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Core.Misc;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

// define custom variable for test test
public class PerformanceTest
{
    private MongoClient client;
	private IMongoDatabase database;
	private IMongoCollection<userDetail> loginCollection;
    private IMongoCollection<Quest> questionCollection;
    private string UserName;
	private string PassWord;
    private string passwd;

    public class userDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Username { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Password { get; set; }
    }

    public class Quest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string QuestionID { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Question { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Type { get; set; }

        // [BsonRepresentation(BsonType.Object)]
        public string Options { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string CorrectAns { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public int Galaxy { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public int Planet { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Difficulty { get; set; }
    }


    [UnityTest, Performance]
    public IEnumerator TestLogin() {
        var name = "swati";
        var password = "ssad";
        Measure.Method(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    CheckUser(name, password);
                }
            })
        .MeasurementCount(10)
        .IterationsPerMeasurement(5)
        .Run();
        
        yield return null;
    }
    [UnityTest, Performance]
    public IEnumerator TestQuestions() {
        client = new MongoClient("mongodb+srv://Student:ssad@cluster.dibtk.mongodb.net/GalaxSEdb?retryWrites=true&w=majority");
        Measure.Method(() => {
            for (int i=0; i < 5; i++) {

            var teacherdb = client.GetDatabase("SpaceGo");
            var questBank = teacherdb.GetCollection<Quest>("questionbanks");

            var filter = Builders<Quest>.Filter.Eq("Planet", 1);
            filter = filter & Builders<Quest>.Filter.Eq("Difficulty", "Easy") & Builders<Quest>.Filter.Eq("Galaxy", 1);
            var projection = Builders<Quest>.Projection.Include("Question").Include("QuestionID").Include("Options").Include("CorrectAns").Include("Galaxy").Include("Planet").Include("Difficulty").Exclude("_id");
            var result = questBank.Find<Quest>(filter).Project(projection).ToList();
        }
        })
        .MeasurementCount(10)
        .IterationsPerMeasurement(5)
        .Run();

        yield return null;
    }

    public int CheckUser(string GivenUsername, string GivenPassword)
	{
		// Database Access
		client = new MongoClient("mongodb+srv://Student:ssad@cluster.dibtk.mongodb.net/GalaxSEdb?retryWrites=true&w=majority");
		database = client.GetDatabase("Student");
        loginCollection = database.GetCollection<userDetail>("login");
		// Filter to get password only
		var fil = Builders<userDetail>.Filter.Eq("Username", GivenUsername);
		var projection = Builders<userDetail>.Projection.Include("Password").Exclude("_id");
		var result = loginCollection.Find<userDetail>(fil).Project(projection).ToList();

		foreach (var res in result)
		{
			var pwd = res["Password"];
			passwd = pwd.ToString();
		}

		//Filter to get username
		var filter = new BsonDocument("Username", GivenUsername);
		long findUsername = loginCollection.Find(filter).Count();

       
        if (findUsername == 1)
        {

            if (passwd != GivenPassword)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        
	    }
        return -1;
    }
    
    [UnityTest, Performance]
    public IEnumerator Galaxy_RenderPerformance(){

        SceneManager.LoadScene("Galaxies");
        
        yield return null;

        yield return Measure.Frames().Run();
    }

    [UnityTest, Performance]
    public IEnumerator OrangeGalaxy_RenderPerformance(){

        SceneManager.LoadScene("OrangeGalaxy");
        
        yield return null;

        yield return Measure.Frames().Run();
    }

    [UnityTest, Performance]
    public IEnumerator CreateLvl_RenderPerformance(){

        SceneManager.LoadScene("CreateLevel");
        
        yield return null;

        yield return Measure.Frames().Run();
    }

}

