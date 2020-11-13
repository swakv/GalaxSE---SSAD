using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class TestMoving : MonoBehaviour
{
    private string keyip;
    private float speed = 1.0f;
    public GameObject block;
    public int width = 10;
    public int height = 4;
    public int result;
    public Move move = new Move();
  

    [UnityTest]
    public IEnumerator WMovesForward()
    {

        keyip = "W";

        GameObject g = new GameObject("move");
        g.transform.position = new Vector3(0,0,0);

        GameObject t = new GameObject("target");
        t.transform.position = new Vector3(0,0,1);

        // Debug.Log(g.transform.position); CREATED AT (0,1,0)

        g.transform.position = move.makeMove(g, keyip);
        
        
        if (Vector3.Distance (g.transform.position, t.transform.position) <= 0)
        {
            result = 1;
        }
        else{
            result = -1;
        }

        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, 1);

    }

    [UnityTest]
    public IEnumerator AMovesLeft()
    {

        keyip = "A";

        GameObject g = new GameObject("move");
        g.transform.position = new Vector3(0,0,0);

        GameObject t = new GameObject("target");
        t.transform.position = new Vector3(-1,0,0);

        // Debug.Log(g.transform.position); CREATED AT (0,1,0)

         g.transform.position = move.makeMove(g, keyip);
        
        
        if (Vector3.Distance (g.transform.position, t.transform.position) <= 0)
        {
            result = 1;
        }
        else{
            result = -1;
        }

        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, 1);

    }

    [UnityTest]
    public IEnumerator SMovesBack()
    {

        keyip = "S";

        GameObject g = new GameObject("move");
        g.transform.position = new Vector3(0,0,0);

        GameObject t = new GameObject("target");
        t.transform.position = new Vector3(0,0,-1);

        // Debug.Log(g.transform.position); CREATED AT (0,1,0)

         g.transform.position = move.makeMove(g, keyip);
        
        if (Vector3.Distance (g.transform.position, t.transform.position) <= 0)
        {
            result = 1;
        }
        else{
            result = -1;
        }

        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, 1);

    }
    
    [UnityTest]
    public IEnumerator DMovesRight()
    {

        keyip = "D";

        GameObject g = new GameObject("move");
        g.transform.position = new Vector3(0,0,0);

        GameObject t = new GameObject("target");
        t.transform.position = new Vector3(1,0,0);

        // Debug.Log(g.transform.position); CREATED AT (0,1,0)

         g.transform.position = move.makeMove(g, keyip);
        
        if (Vector3.Distance (g.transform.position, t.transform.position) <= 0)
        {
            result = 1;
        }
        else{
            result = -1;
        }

        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, 1);

    }
}
