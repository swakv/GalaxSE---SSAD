using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Linq;

public class ToChallengeMonster : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TMP_Dropdown Galaxy;
    public TMPro.TMP_Dropdown Easy;
    public TMPro.TMP_Dropdown Medium;
    public TMPro.TMP_Dropdown Galaxies;
    public TMPro.TMP_Dropdown Difficult;
    public static BsonArray Questions;
    public static string linkText;
    
    void LoadCM() 
    {
        Questions = new BsonArray();
        
        Debug.Log("start");

        Debug.Log(Easy.value);

        if (SumIsEight(Easy.value, Medium.value, Difficult.value) == 1) {

            var Client = new MongoClient("mongodb+srv://Student:ssad@cluster.dibtk.mongodb.net/GalaxSEdb?retryWrites=true&w=majority");
            var Database = Client.GetDatabase("SpaceGo");
            var CollectionQ = Database.GetCollection<BsonDocument>("questionbanks");

            var easyQuestFilter = Builders<BsonDocument>.Filter.Eq("Galaxy", Galaxy.value + 1);
            easyQuestFilter = easyQuestFilter & Builders<BsonDocument>.Filter.Eq("Difficulty", "Easy");
            var easyDoc = CollectionQ.Find(easyQuestFilter).ToCursor();

            var mediumQuestFilter = Builders<BsonDocument>.Filter.Eq("Galaxy", Galaxy.value + 1);
            mediumQuestFilter = mediumQuestFilter & Builders<BsonDocument>.Filter.Eq("Difficulty", "Medium");
            var mediumDoc = CollectionQ.Find(mediumQuestFilter).ToCursor();

            var difficultQuestFilter = Builders<BsonDocument>.Filter.Eq("Galaxy", Galaxy.value + 1);
            difficultQuestFilter = difficultQuestFilter & Builders<BsonDocument>.Filter.Eq("Difficulty", "Hard");
            var hardDoc = CollectionQ.Find(difficultQuestFilter).ToCursor();

            int i = 0;
            
            //iterate through easy questions
            foreach (var easyd in easyDoc.ToEnumerable())
            {
                if (Easy.value == i)
                    break;
                Questions.Add(easyd["QuestionID"].ToString());
                i = i + 1;
            }

            //iterate through medium questions
            i = 0;
            foreach (var mediumd in mediumDoc.ToEnumerable())
            {
                if (Medium.value == i)
                    break;
                Questions.Add(mediumd["QuestionID"].ToString());
                i = i + 1;
            }

            i = 0;
            //iterate through hard questions
            foreach (var hardd in hardDoc.ToEnumerable())
            {
                if (Medium.value == i)
                    break;
                Questions.Add(hardd["QuestionID"].ToString());
                i = i + 1;
            }

            var CollectionC = Database.GetCollection<BsonDocument>("challenges");
            var DocumentC = new BsonDocument{
                 {"QuestionID",  Questions},
                 {"Username" , Login.UserNameS},
                 {"ChallengeCode","C"+Login.UserNameS+System.DateTime.Now.ToString("HHmmss")},
            };
            CollectionC.InsertOneAsync(DocumentC);
            linkText = "C"+ Login.UserNameS + System.DateTime.Now.ToString("HHmmss");

            Debug.Log("end");
            SceneManager.LoadScene("ChallengeMonster");

        }
    }
    public int SumIsEight(int e,int m,int h) {
        if (e+m+h==8)
            return 1;
        return 0;
    }
    void Start() {
        int val = Progress.unlockedPlanet;
        val = val/9 + 1;
        Galaxies.ClearOptions();

        for (int i=1; i<=val;i++) {
            Galaxies.options.Add(new TMPro.TMP_Dropdown.OptionData() { text = "Galaxy " + i.ToString()});
            Debug.Log(i);
        }
        Debug.Log("reaching here");
        
    }
}
