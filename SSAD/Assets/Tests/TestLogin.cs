using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestLogin
{

    private string UserName;
	private string PassWord;
    public Login login = new Login();


    [UnityTest]
    public IEnumerator LoginUserNameAndPwdinDB()
    {
        UserName="swati";
        PassWord="ssad";
        int result = login.CheckDB(UserName, PassWord);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, 1);
    }

    [UnityTest]
    public IEnumerator LoginUserNameinDBAndPwdWrong()
    {
        UserName="swati";
        PassWord="iamswati";
        int result = login.CheckDB(UserName, PassWord);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, -1);
    }

    [UnityTest]
    public IEnumerator LoginUserNameNotinDBAndPwdCorrect()
    {
        UserName="swathhii";
        PassWord="ssad";
        int result = login.CheckDB(UserName, PassWord);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, -1);
    }

    [UnityTest]
    public IEnumerator LoginUserNameNotinDBAndPwdWrong()
    {
        UserName="swathhii";
        PassWord="sssaaaddd";
        int result = login.CheckDB(UserName, PassWord);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, -1);
    }

    [UnityTest]
    public IEnumerator LoginUserNameNullDBAndPwdWrong()
    {
        UserName="";
        PassWord="sssaaaddd";
        int result = login.CheckDB(UserName, PassWord);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, -1);
    }

    [UnityTest]
    public IEnumerator LoginUserNameAndPwdNull()
    {
        UserName="swati";
        PassWord="";
        int result = login.CheckDB(UserName, PassWord);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, -1);
    }

    [UnityTest]
    public IEnumerator LoginUserNameNullAndPwdNull()
    {
        UserName="";
        PassWord="";
        int result = login.CheckDB(UserName, PassWord);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, -1);
    }



}