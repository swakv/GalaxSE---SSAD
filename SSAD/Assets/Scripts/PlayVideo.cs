using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Core.Misc;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;

public class PlayVideo : MonoBehaviour
{

    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<video> videoCollection;
    public string videourl;

    public class video
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public int galaxy { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string url { get; set; }
    }

    void Start()
    {
        client = new MongoClient("mongodb+srv://Student:ssad@cluster.dibtk.mongodb.net/GalaxSEdb?retryWrites=true&w=majority");
        database = client.GetDatabase("SpaceGo");
        Debug.Log("Connected to DB!");
    }

    void Update()
    {
      
    }

     public void PlayRedVid(){

        videoCollection = database.GetCollection<video>("playvideo");
        var fil = Builders<video>.Filter.Eq("galaxy", 1);
        var projection = Builders<video>.Projection.Include("galaxy").Include("url").Exclude("_id");
        var result = videoCollection.Find<video>(fil).Project(projection).ToList();
        videourl = result[0][1].ToString(); // contains the url of the specified galaxy
        Debug.Log("aNJSCn" + result[0][0].ToString());
        Application.OpenURL(videourl);

    }

    public void PlayBlueVid(){

        var fil = Builders<video>.Filter.Eq("galaxy", 2);
        var projection = Builders<video>.Projection.Include("galaxy").Include("url").Exclude("_id");
        var result = videoCollection.Find<video>(fil).Project(projection).ToList();
        videourl = result[0][1].ToString(); // contains the url of the specified galaxy
        Application.OpenURL(videourl);

    }
   
    public void PlayGreenVid(){

        var fil = Builders<video>.Filter.Eq("galaxy", 3);
        var projection = Builders<video>.Projection.Include("galaxy").Include("url").Exclude("_id");
        var result = videoCollection.Find<video>(fil).Project(projection).ToList();
        videourl = result[0][1].ToString(); // contains the url of the specified galaxy
        Application.OpenURL(videourl);
    }
    public void PlayOrangeVid(){

        var fil = Builders<video>.Filter.Eq("galaxy", 4);
        var projection = Builders<video>.Projection.Include("galaxy").Include("url").Exclude("_id");
        var result = videoCollection.Find<video>(fil).Project(projection).ToList();
        videourl = result[0][1].ToString(); // contains the url of the specified galaxy
        Application.OpenURL(videourl);

    }

}

