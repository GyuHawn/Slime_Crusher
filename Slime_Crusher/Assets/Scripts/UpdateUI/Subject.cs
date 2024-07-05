using System;

public interface Subject
{
    void RegisterObserver(Observer observer);
    void UnregisterObserver(Observer observer);
    void NotifyObservers();
}
