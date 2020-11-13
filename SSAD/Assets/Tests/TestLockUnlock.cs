using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Core.Misc;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

public class TestLockUnlock : MonoBehaviour
{
    public ResultsDBAccess rda = new ResultsDBAccess();

    [UnityTest]
    public IEnumerator LockWhenLessThan75()
    {
        int here;
        string UserName = "swati";
        int [] ret = rda.checkDB(UserName);
        int pc = ret[0];
        int up = ret[1];
        if(pc<9){
            here = up;
        }
        else{
            here = up++;
        }
        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(here, up);

    }

    [UnityTest]
    public IEnumerator UnlockWhenGreaterThan75()
    {
        int here;
        string UserName = "swathi";
        int [] ret = rda.checkDB(UserName);
        int pc = ret[0];
        int up = ret[1];
        if(pc<9){
            here = up;
        }
        else{
            here = up+1;
        }
        yield return new WaitForSeconds(0.1f);
        Assert.Less(up,here);
        

    }


}
