using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class TestDropDown : MonoBehaviour
{
    public ToChallengeMonster tcm = new ToChallengeMonster();

    [UnityTest]
    public IEnumerator DropDownEqual8()
    {
        int easyDrop = 2;
        int mediumDrop = 4;
        int hardDrop = 2;
        int result = tcm.SumIsEight(easyDrop, mediumDrop, hardDrop);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, 1);
    }

    [UnityTest]
    public IEnumerator DropDownNotEqual8()
    {
        int easyDrop = 2;
        int mediumDrop = 2;
        int hardDrop = 2;
        int result = tcm.SumIsEight(easyDrop, mediumDrop, hardDrop);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(result, 0);
    }

}
