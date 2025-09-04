using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Car car;
    void Start()
    {
    Car c = new BuilderPattern()
            .AddHealth(4)
            .AddHealth(100)
            .AddSkill(null)
            .AddSpeed(1)
            .Build();

        Car a = Instantiate(c, this.transform);
        car = a;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
