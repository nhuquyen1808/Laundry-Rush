using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilder 
{
    BuilderPattern AddUI(GameObject o);
    BuilderPattern AddHealth(int Health);
    BuilderPattern AddSkill(GameObject o);
    BuilderPattern AddSpeed(int speed);

    Car Build();
}
