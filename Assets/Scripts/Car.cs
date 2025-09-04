using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public int Health;
    public GameObject Skill;
    public int Speed;
    public GameObject Ui;

    public Car(int health, GameObject skill, int speed, GameObject ui)
    {
        Health = health;
        Skill = skill;
        Speed = speed;
        Ui = ui;
    }

    private void Start()
    {
        this.gameObject.SetActive(true);
    }
}
