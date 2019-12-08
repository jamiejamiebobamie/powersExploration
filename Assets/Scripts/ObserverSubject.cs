using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObserverSubject : MonoBehaviour, IObservable
{
    private List<Observer> _observers = new List<Observer>();

    // Attach an observer to the subject.
    public void Attach(Observer observer)
    {
        _observers.Add(observer);
    }

    // Detach an observer from the subject.
    public void Detach(Observer observer)
    {
        _observers.Remove(observer);

    }

    // Notify all observers about an event.
    public void Notify()
    {
        foreach (Observer observer in _observers)
        {
            observer.UpdateObserver(this);
        }
    }

}
