using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Login : MonoBehaviour
{
	public static string UserNameS;
	public GameObject username;
	public InputField Username_f;
	public Text console;
	public GameObject password;
	private string UserName;
	private string PassWord;
	private string DecryptedPass;
	private MongoClient client;
	private IMongoDatabase database;
	private IMongoCollection<userDetail> loginCollection;
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

	public void LoginButton()
	{
		bool UN = false;
		bool PW = false;

		loginCollection = database.GetCollection<userDetail>("login");

		// Filter to get password only
		var fil = Builders<userDetail>.Filter.Eq("Username", UserName);
		var projection = Builders<userDetail>.Projection.Include("Password").Exclude("_id");
		var result = loginCollection.Find<userDetail>(fil).Project(projection).ToList();

		foreach (var res in result)
		{
			var pwd = res["Password"];
			passwd = pwd.ToString();
		}

		Debug.LogWarning("Got Password");

		//Filter to get username
		var filter = new BsonDocument("Username", UserName);
		long findUsername = loginCollection.Find(filter).Count();

		if ((UserName != "") && (PassWord != "")) //if username & password exist in db
		{
			if (findUsername == 1)
			{
				UN = true;

				if (passwd != PassWord)
				{
					Username_f.ActivateInputField();
					Debug.LogWarning("Password Is invalid");
					console.text = "Password in Invalid";
				}
				else
				{
					PW = true;
				}
			}
			else
			{
				Username_f.ActivateInputField();
				Debug.LogWarning("Username or Password Invaild");
				console.text = "Username or Password Invalid";
			}
		}
		else
		{
			if (UserName == "")
			{
				Username_f.ActivateInputField();
				Debug.LogWarning("Username Field Empty");
				console.text = "Username Field Empty";
			}
			else
			{
				Username_f.ActivateInputField();
				Debug.LogWarning("Password Field Empty");
				console.text = "Password Field Empty";
			}
		}

		if (UN == true && PW == true)
		{
			username.GetComponent<InputField>().text = "";
			password.GetComponent<InputField>().text = "";
			console.text = "Login Successful";
			Debug.Log("Login Done");
			UserNameS = UserName;
			 if (System.IO.File.Exists("/Users/Swa/Documents/NTU/Y3S1/CZ3003/PROJECT/GalaxSE/newFile.txt")){
					//if user is logged in
						string linkText = System.IO.File.ReadAllText("/Users/Swa/Documents/NTU/Y3S1/CZ3003/PROJECT/GalaxSE/newFile.txt");
						Debug.Log("entering file exists"); 
						System.IO.File.Delete("/Users/Swa/Documents/NTU/Y3S1/CZ3003/PROJECT/GalaxSE/newFile.txt");
						SceneManager.LoadScene("ChallengeMonster");
						Debug.Log("exiting");
				}
				else{
					SceneManager.LoadScene("Home");
				}
			
		}

	}

	void Start()
	{
		Username_f.ActivateInputField();

		// Database Access 
		client = new MongoClient("mongodb+srv://Student:ssad@cluster.dibtk.mongodb.net/GalaxSEdb?retryWrites=true&w=majority");
		database = client.GetDatabase("Student");
		Debug.Log("Connected to DB!");
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (username.GetComponent<InputField>().isFocused)
			{
				password.GetComponent<InputField>().Select();
			}
		}
		if (Input.GetKeyDown(KeyCode.Return))
		{
			if (PassWord != "")
			{
				LoginButton();
			}
		}

		UserName = username.GetComponent<InputField>().text;
		PassWord = password.GetComponent<InputField>().text;
	}

	public int CheckDB(string GivenUsername, string GivenPassword)
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
}
