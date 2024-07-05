using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeUI : MonoBehaviour, Subject
{
    private List<Observer> observers = new List<Observer>();
    private float gameTime;

    void Update()
    {
        gameTime += Time.deltaTime;
        NotifyObservers();
    }
    public void NotifyObservers()
    {
        foreach(Observer observer in observers)
        {
            observer.UpdateTime(gameTime);
        }
    }

    public void RegisterObserver(Observer observer)
    {
        observers.Add(observer);
    }

    public void UnregisterObserver(Observer observer)
    {
        observers.Remove(observer);
    }

}
