using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public interface IAnimal
{
    public string name();
    public void Sound();
    public void Eat();
    public void Sleep();
}


public interface IAnimalFactory
{
    IAnimal createAnimal();
}


public class FactoryPattern : MonoBehaviour
{

    public IAnimalFactory factory1, factory2; 

    void Start()
    {
        factory1 = new BasicAnimalFactory();
        factory2 = new RandomAnimalFactory();

        IAnimal animal = factory1.createAnimal();
        IAnimal animal1 = factory2.createAnimal();
    }

    void Update()
    {
        
    }

}
