using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderPattern :MonoBehaviour, IBuilder
{

    public int Health {  get; private set; }
    public GameObject Skill { get; private set; }
    public int Speed { get; private set; }
    public GameObject Ui { get; private set; }

    public BuilderPattern AddHealth(int health)
    {
        Health = health;
        return this;
    }

    public BuilderPattern AddSkill(GameObject o)
    {
       Skill = o;
       return this;
    }

    public BuilderPattern AddSpeed(int speed)
    {
        Speed = speed;
        return this;
    }

    public BuilderPattern AddUI(GameObject o)
    {
        Ui = o;
        return this;
    }

    public Car Build()
    {
       return new Car(Health, Skill, Speed, Ui);
    }
}
