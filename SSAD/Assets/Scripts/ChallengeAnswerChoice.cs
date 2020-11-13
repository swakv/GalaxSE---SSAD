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
using System.Linq;
using UnityEngine.SceneManagement;

public class ChallengeAnswerChoice : MonoBehaviour
{
    public string correctAnswer = "A";
    public Button A;
    public Button B;
    public Button C;
    public Button D;
    private MongoClient client;
    
    private IMongoDatabase database;
    private IMongoDatabase teacherdb;
    private IMongoCollection<ansColl> ansCollection;
    private IMongoCollection<Quest> questBank;
    private IMongoCollection<assignColl> assignmentRes;
    public string rightAns;
    public string question;
    public int galaxy;
    public int planet;
    public string questionid;
    public string diff;
    public string OptionA;
    public string OptionB;
    public string OptionC;
    public string OptionD;
    public TextMeshProUGUI Question_UI;
    public TextMeshProUGUI TextA;
    public TextMeshProUGUI TextB;
    public TextMeshProUGUI TextC;
    public TextMeshProUGUI TextD;
    public int number = 0;
    List<Quest> questionsByTopic = new List<Quest>();
    public static int score = 0;
    public static string Galaxy;
    public TextMeshProUGUI console;
    public char work = 'p'; 

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

    public class ansColl
    {
        [BsonRepresentation(BsonType.String)]
        public string StudentUsername { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Question { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string QuestionID { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string YourAnswer { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string RightAnswer { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public int Galaxy { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public int Planet { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Difficulty { get; set; }

        [BsonRepresentation(BsonType.Boolean)]
        public bool correctCount { get; set; }
    }

    public class assignColl
    {
        [BsonRepresentation(BsonType.String)]
        public string Username { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string AssignmentCode { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public int Score { get; set; }
    }

    public void questions(int i)
    {
        int length = 0;
       
        foreach(char c in ClickToStart.linkText) ++length;
        foreach (char item in ClickToStart.linkText) { work = item; break;}
        
        Debug.Log(length);
        if (length < 2) {
            Debug.Log("going insiide Challenger");
            var questBank = teacherdb.GetCollection<BsonDocument>("questionbanks");
            var filter = Builders<BsonDocument>.Filter.In("QuestionID", ToChallengeMonster.Questions);
            var result = questBank.Find(filter).ToList();

            //var result = formQuestions();
            rightAns = result[i][5].ToString();
            var Question = result[i][2];
            question = Question.ToString();

            Question_UI.SetText(question);

            var QuestId = result[i][1];
            questionid = QuestId.ToString();

            var gal = result[i][6];
            galaxy = gal.ToInt32();
            Galaxy = galaxy.ToString();

            var plan = result[i][7];
            planet = plan.ToInt32();


            var difficult = result[i][8];
            diff = difficult.ToString();
            console.SetText(diff);
            var aChoice = result[i][4][0];
            OptionA = aChoice.ToString();

            TextA.text = OptionA;

            var bChoice = result[i][4][1];
            OptionB = bChoice.ToString();

            TextB.text = OptionB;

            var cChoice = result[i][4][2];
            OptionC = cChoice.ToString();

            Debug.Log(result[i][4].ToString());

            TextC.text = OptionC;

            var dChoice = result[i][4][3];
            OptionD = dChoice.ToString();

            // Debug.Log(OptionD);

            TextD.text = OptionD;
        }
        else {
            if (work == 'C') {
                // Debug.Log(ClickToStart.linkText);
                var Challenge = teacherdb.GetCollection<BsonDocument>("challenges");
                var filter_1 = Builders<BsonDocument>.Filter.Eq("ChallengeCode", ClickToStart.linkText); 
                var res = Challenge.Find(filter_1).ToList();
                Debug.Log(res[0][1].ToString());
                List<string> array = BsonSerializer.Deserialize<List<string>>(res[0][1].ToJson());

                var questBank = teacherdb.GetCollection<BsonDocument>("questionbanks");
                var filter_2 = Builders<BsonDocument>.Filter.In("QuestionID", array);
                var result = questBank.Find(filter_2).ToList();

                // Debug.Log(result.ToString());

                rightAns = result[i][5].ToString();
                var Question = result[i][2];
                question = Question.ToString();

                Debug.Log(question);

                Question_UI.SetText(question);

                var QuestId = result[i][1];
                questionid = QuestId.ToString();

                var gal = result[i][6];
                galaxy = gal.ToInt32();
                Galaxy = galaxy.ToString();

                var plan = result[i][7];
                planet = plan.ToInt32();

                var difficult = result[i][8];
                diff = difficult.ToString();

                var aChoice = result[i][4][0];
                OptionA = aChoice.ToString();

                TextA.text = OptionA;

                var bChoice = result[i][4][1];
                OptionB = bChoice.ToString();

                TextB.text = OptionB;

                var cChoice = result[i][4][2];
                OptionC = cChoice.ToString();

                TextC.text = OptionC;

                var dChoice = result[i][4][3];
                OptionD = dChoice.ToString();

                TextD.text = OptionD;

            }
            else {
                var Challenge = teacherdb.GetCollection<BsonDocument>("assignments");
                var filter_1 = Builders<BsonDocument>.Filter.Eq("AssignmentCode", ClickToStart.linkText); // "AReena1602584077000"
                var res = Challenge.Find(filter_1).ToList();
                List<string> array = BsonSerializer.Deserialize<List<string>>(res[0][1].ToJson());

                var questBank = teacherdb.GetCollection<BsonDocument>("questionbanks");
                var filter_2 = Builders<BsonDocument>.Filter.In("QuestionID", array);
                var result = questBank.Find(filter_2).ToList();

                //var result = formQuestions();
                rightAns = result[i][5].ToString();
                var Question = result[i][2];
                question = Question.ToString();

                Question_UI.SetText(question);

                var QuestId = result[i][1];
                questionid = QuestId.ToString();

                var gal = result[i][6];
                galaxy = gal.ToInt32();
                Galaxy = galaxy.ToString();

                var plan = result[i][7];
                planet = plan.ToInt32();


                var difficult = result[i][8];
                diff = difficult.ToString();

                var aChoice = result[i][4][0];
                OptionA = aChoice.ToString();

                TextA.text = OptionA;

                var bChoice = result[i][4][1];
                OptionB = bChoice.ToString();

                TextB.text = OptionB;

                var cChoice = result[i][4][2];
                OptionC = cChoice.ToString();

                TextC.text = OptionC;

                var dChoice = result[i][4][3];
                OptionD = dChoice.ToString();

                TextD.text = OptionD;
            }
        }

    }

    //Start is called before the first frame update
    void Start()
    {
        // Database Access
        client = new MongoClient("mongodb+srv://Student:ssad@cluster.dibtk.mongodb.net/GalaxSEdb?retryWrites=true&w=majority");
        database = client.GetDatabase("Student");
        teacherdb = client.GetDatabase("SpaceGo");
        score=0;
        questions(number);
    }

    //Update is called once per frame
    void Update()
    {
        ColorBlock colorA = A.colors;
        ColorBlock colorB = B.colors;
        ColorBlock colorC = C.colors;
        ColorBlock colorD = D.colors;
        bool correctNumCount = true;
        string chosen = EventSystem.current.currentSelectedGameObject.name;

        correctAnswer = rightAns.ToString();

        if (chosen == correctAnswer)
        {
            if (chosen == "A")
            {
                colorA.pressedColor = Color.blue;
                A.colors = colorA;
            }
            else if (chosen == "B")
            {
                colorB.pressedColor = Color.blue;
                B.colors = colorB;
            }
            else if (chosen == "C")
            {
                colorC.pressedColor = Color.blue;
                C.colors = colorC;
            }
            else
            {
                colorD.pressedColor = Color.blue;
                D.colors = colorD;
            }

            if (number < 8)
            {
                if (Input.GetMouseButtonDown(0)){
                    number++;
                    score += 1;
                    questions(number);
                }
            }
            else{
                string sceneName = PlayerPrefs.GetString("lastLoadedScene");
                SceneManager.LoadScene("ChallengeFriends");
            }
        }
        else if (true)
        {
            correctNumCount = false;

            if (chosen == "A")
            {
                colorA.pressedColor = Color.red;
                A.colors = colorA;
            }
            else if (chosen == "B")
            {
                colorB.pressedColor = Color.red;
                B.colors = colorB;
            }
            else if (chosen == "C")
            {
                colorC.pressedColor = Color.red;
                C.colors = colorC;
            }
            else
            {
                colorD.pressedColor = Color.red;
                D.colors = colorD;
            }

            if (number < 8)
            {
                if (Input.GetMouseButtonDown(0)){
                    number++;
                    questions(number);
                }
            }
            else{
                if (work == 'A'){
                    Debug.Log(Login.UserNameS);
                    Debug.Log(ClickToStart.linkText);
                    Debug.Log(score);
                    assignmentRes = database.GetCollection<assignColl>("assignmentResult");
                    assignmentRes.InsertOne(new assignColl { Username = Login.UserNameS, AssignmentCode = ClickToStart.linkText, Score = score});
                    SceneManager.LoadScene("AssignmentResult");
                }
                else
                    SceneManager.LoadScene("ChallengeFriends");
            }

            
        }

    }
}

