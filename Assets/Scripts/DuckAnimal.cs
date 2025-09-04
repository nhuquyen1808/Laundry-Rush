using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckAnimal : IAnimal
{
    public void Eat()
    {
        throw new System.NotImplementedException();
    }

    public string name()
    {
        return "i am a duck";
    }

    public void Sleep()
    {
        throw new System.NotImplementedException();
    }

    public void Sound()
    {
        throw new System.NotImplementedException();
    }
}
