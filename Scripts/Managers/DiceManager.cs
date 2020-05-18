using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : Singleton<DiceManager>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public int ThrowDice()
    {
        return Random.Range(1, 7);
    }


    public int GetSumOfXThrownDice(int countOfThrownDice)
    {
        int sumOfThrowmDie = 0;

        for(int i = 0; i < countOfThrownDice; i ++)
        {
            sumOfThrowmDie += ThrowDice();
        }

        return sumOfThrowmDie;
    }

}
