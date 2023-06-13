using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate void UpdateStackEvent(); //creates a delegated event

public class ObservableStack<T> : Stack<T> //TYPE generic
{
    public event UpdateStackEvent OnPush;
    public event UpdateStackEvent OnPop;
    public event UpdateStackEvent OnClear;

    public ObservableStack(ObservableStack<T> items) : base(items) //passing in the items and moving it to the base, instantiating a stack with amount of itemems
    {

    }

    public ObservableStack() //internal structure, removes errors when instantiating an observablestack with 0 items from the beginning
    {

    }

    public new void Push(T item)
    {
        base.Push(item); //add an event listener to push to stack size
        if (OnPush != null)
        {
            OnPush();
        }
    }
    //Calls functions from the stack and raises an event to update something
    public new T Pop()
    {
        T item = base.Pop();
        if (OnPop != null)
        {
            OnPop();
        }
        return item;
    }
    public new void Clear()
    {
        base.Clear();
        if (OnClear != null)
        {
            OnClear();
        }
    }

    
    
}




