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

public class AnswerChoice : MonoBehaviour
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
    public TextMeshProUGUI console;
    public string difficulty_choice;
    public TextMeshProUGUI TextA;
    public TextMeshProUGUI TextB;
    public TextMeshProUGUI TextC;
    public TextMeshProUGUI TextD;
    public int number = 0;
    List<Quest> questionsByTopic = new List<Quest>();

    public string currDifficulty;
    public static List<string> difficulties = new List<string>();
    public static List<string> questionsList = new List<string>();

    public Animator anim;
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

    public void questions(int i, int plt, string currDiff, int galax)
    {
        
        ansCollection = database.GetCollection<ansColl>("ansChose");
        questBank = teacherdb.GetCollection<Quest>("questionbanks");

        var filter = Builders<Quest>.Filter.Eq("Planet", plt);
        filter = filter & Builders<Quest>.Filter.Eq("Difficulty", currDiff) & Builders<Quest>.Filter.Eq("Galaxy", galax);
        var projection = Builders<Quest>.Projection.Include("Question").Include("QuestionID").Include("Options").Include("CorrectAns").Include("Galaxy").Include("Planet").Include("Difficulty").Exclude("_id");
        var result = questBank.Find<Quest>(filter).Project(projection).ToList();
        var num_count = questBank.Find<Quest>(filter).Project(projection).Count();
        result = result.OrderBy(a => Guid.NewGuid()).ToList();

        var Question = result[i][1];
        question = Question.ToString();
        

        Question_UI.SetText(question);

        var QuestId = result[i][0];
        questionid = QuestId.ToString();

        var gal = result[i][4];
        galaxy = gal.ToInt32();

        var plan = result[i][5];
        planet = plan.ToInt32();

        var difficult = result[i][6];
        diff = difficult.ToString();

        console.SetText(diff);

        var aChoice = result[i][2][0];
        OptionA = aChoice.ToString();

        TextA.text = OptionA;

        var bChoice = result[i][2][1];
        OptionB = bChoice.ToString();

        TextB.text = OptionB;

        var cChoice = result[i][2][2];
        OptionC = cChoice.ToString();

        TextC.text = OptionC;

        var dChoice = result[i][2][3];
        OptionD = dChoice.ToString();

        TextD.text = OptionD;

        var rightans = result[i][3];
        rightAns = rightans.ToString();



    }

    //Start is called before the first frame update
    void Start()
    {
        client = new MongoClient("mongodb+srv://Student:ssad@cluster.dibtk.mongodb.net/GalaxSEdb?retryWrites=true&w=majority");
        database = client.GetDatabase("Student");
        teacherdb = client.GetDatabase("SpaceGo");
        Debug.LogWarning("Connected to db!");

        currDifficulty = Monster.currentMonster;
        // Debug.Log(Monster.currentMonster);
        questions(number, Progress.currentPlanet, Monster.currentMonster, Galaxy1.currentGalxy);

        

    }

    //Update is called once per frame
    void Update()
    {
        anim.SetFloat("motion", -1);
        ColorBlock colorA = A.colors;
        ColorBlock colorB = B.colors;
        ColorBlock colorC = C.colors;
        ColorBlock colorD = D.colors;
        bool correctNumCount = true;
        string chosen = "";
        correctAnswer = rightAns;
        chosen = EventSystem.current.currentSelectedGameObject.name;
        if(number == 0){
            if (Input.GetMouseButtonDown(0)) {
                
                if(rightAns != ""){
                    difficulties.Add(currDifficulty);
                    questionsList.Add(question);
                    ansCollection.InsertOne(new ansColl { StudentUsername = Login.UserNameS, Question = question, QuestionID = questionid, YourAnswer = chosen, RightAnswer = correctAnswer, Galaxy = galaxy, Planet = planet, Difficulty = diff, correctCount = correctNumCount });
                    number++;
                    if (chosen == correctAnswer)
                    {
                        anim.SetFloat("motion", 1);
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

                        if (currDifficulty=="Easy") {
                            currDifficulty="Medium";
                        }
                        else if (currDifficulty=="Medium") {
                            currDifficulty="Hard";
                        }
                        questions(number, Progress.currentPlanet, currDifficulty, Galaxy1.currentGalxy);
                        
                    }
                    else
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
                        questions(number, Progress.currentPlanet, currDifficulty, Galaxy1.currentGalxy);
                        
                        
                    }
                }
                
            }
        }

        
        else if (number < 4)
        {
            if (Input.GetMouseButtonDown(0)) {

                if(rightAns != ""){
                    difficulties.Add(currDifficulty);
                    questionsList.Add(question);
                    ansCollection.InsertOne(new ansColl { StudentUsername = Login.UserNameS, Question = question, QuestionID = questionid, YourAnswer = chosen, RightAnswer = correctAnswer, Galaxy = galaxy, Planet = planet, Difficulty = Monster.currentMonster, correctCount = correctNumCount });
                    number++;
                    if (chosen == correctAnswer)
                    {
                        anim.SetFloat("motion", 1);
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

                        if (currDifficulty=="Easy") {
                            currDifficulty="Medium";
                        }
                        else if (currDifficulty=="Medium") {
                            currDifficulty="Hard";
                        }
                        questions(number, Progress.currentPlanet, currDifficulty, Galaxy1.currentGalxy);
                        
                    }
                    else
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
                        questions(number, Progress.currentPlanet, currDifficulty, Galaxy1.currentGalxy);
                        
                        
                    }
                }
            }
            
        }
        else
        {
            SceneManager.LoadScene("Results");
        }
    }
    public int[] CheckDB(string questionString, string currentMonster, string yourAns)
	{
        int num=0;
        var score = 0;
		// Database Access 
		client = new MongoClient("mongodb+srv://Student:ssad@cluster.dibtk.mongodb.net/GalaxSEdb?retryWrites=true&w=majority");
		database = client.GetDatabase("Student");
        teacherdb = client.GetDatabase("SpaceGo");

        questBank = teacherdb.GetCollection<Quest>("questionbanks");

		var filter = new BsonDocument("Question", questionString);
        var projection = Builders<Quest>.Projection.Include("CorrectAns");
        var result = questBank.Find<Quest>(filter).Project(projection).ToList();
		long findQname = questBank.Find<Quest>(filter).Count();

        foreach (var res in result){
            var rS = res[1];
            rightAns = rS.ToString();
        } 

        // Debug.Log("rightAns");
        // Debug.Log(rightAns);
        // Debug.Log("YourAns");
        // Debug.Log(yourAns);

        if (findQname == 1)
        {
            if (rightAns != yourAns)
            {
                // Debug.Log("not go");
                num=-1;
            }
            else
            {
                if (currentMonster =="Easy"){
                    score+=100;
                } else if (currentMonster=="Medium"){
                    score+=200;
                } else if (currentMonster == "Hard"){
                    score+=300;
                }
                num= 1;
            }
	    }
        else{
            num=-1;
        }
        
        int[] myArr = new int[]{num,score};
        return myArr;
    }
}

