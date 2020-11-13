using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class TestAnswers : MonoBehaviour
{
    public string rightAns;
    public string yourAns;
    public string currentMonster;
    public int score;
    public string questionString;
    public AnswerChoice ac = new AnswerChoice();
    
    [UnityTest]
    // Start is called before the first frame update
    public IEnumerator CorrectAnsEasy()
    {
        score=0;
        questionString="If a Direct approach to software project sizing is taken, size can be measured in";
        yourAns="A";
        int[] arr = ac.CheckDB(questionString, "Easy", yourAns);
        int result= arr[0];
        score= arr[1];
        int updatedScore = 100;
        // Debug.Log(result);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, 1);
        Assert.AreEqual(score, updatedScore);
    }

    [UnityTest]
    public IEnumerator WrongAnsEasy()
    {
        score=0;
        questionString="If a Direct approach to software project sizing is taken, size can be measured in";
        yourAns="B";
        int[] arr = ac.CheckDB(questionString, "Easy", yourAns);
        int result= arr[0];
        score= arr[1];
        int updatedScore = 0;

        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, -1);
        Assert.AreEqual(score, updatedScore);
    }

    [UnityTest]
    public IEnumerator CorrectAnsMed()
    {
        score=0;
        questionString="Which software project sizing approach develop estimates of the information domain characteristics?";
        yourAns="B";
        int[] arr = ac.CheckDB(questionString, "Medium", yourAns);
        int result= arr[0];
        score= arr[1];
        int updatedScore = 200;

        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, 1);
        Assert.AreEqual(score, updatedScore);
    }
    
    [UnityTest]
    public IEnumerator WrongAnsMed()
    {
        score=0;
        questionString="Which software project sizing approach develop estimates of the information domain characteristics?";
        yourAns="D";
        int[] arr = ac.CheckDB(questionString, "Medium", yourAns);
        int result= arr[0];
        score= arr[1];
        int updatedScore = 0;

        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, -1);
        Assert.AreEqual(score, updatedScore);
    }

    [UnityTest]
    public IEnumerator CorrectAnsDifficult()
    {
        score=0;
        questionString="Programming language experience is a part of which factor of COCOMO cost drivers?";
        yourAns="A";
        int[] arr = ac.CheckDB(questionString, "Hard", yourAns);
        int result= arr[0];
        score= arr[1];
        int updatedScore = 300;

        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, 1);
        Assert.AreEqual(score, updatedScore);
    }

    [UnityTest]
    public IEnumerator WrongAnsDifficult()
    {
        score=0;
        questionString="Programming language experience is a part of which factor of COCOMO cost drivers?";
        yourAns="C";
        int[] arr = ac.CheckDB(questionString, "Hard", yourAns);
        int result= arr[0];
        score= arr[1];
        int updatedScore = 0;

        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, -1);
        Assert.AreEqual(score, updatedScore);
    }
}
