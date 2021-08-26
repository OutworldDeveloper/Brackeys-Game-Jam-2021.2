using System;
using System.Collections.Generic;

public sealed class StateMachine
{

    private readonly Dictionary<State, List<Transition>> _transitions = new Dictionary<State, List<Transition>>();
    private readonly List<Transition> _globalTransitions = new List<Transition>();
    private readonly HashSet<string> _triggers = new HashSet<string>();

    private State _currentState;

    public void Start(State initialState)
    {
        _currentState = initialState;
        _currentState.Start();
    }

    public void Tick()
    {
        var nextState = FindNextState(_currentState);
        if (nextState != null && nextState != _currentState)
        {
            _currentState.End();
            _currentState = nextState;
            _currentState.Start();
            OnStateChanged();
        }
        _currentState.Tick();
    }

    public void CreateTransition(State from, State to, Func<bool> condition)
    {
        if (_transitions.ContainsKey(from) == false)
        {
            _transitions.Add(from, new List<Transition>());
        }
        var newTransition = new Transition(condition, to);
        _transitions[from].Add(newTransition);
    }

    public void CreateTransition(State to, Func<bool> condition)
    {
        var newTransition = new Transition(condition, to);
        _globalTransitions.Add(newTransition);
    }

    public void FireTrigger(string id)
    {
        _triggers.Add(id);
    }

    public bool CheckTrigger(string id)
    {
        return _triggers.Contains(id);
    }

    private void OnStateChanged()
    {
        _triggers.Clear();
    }

    private State FindNextState(State from)
    {
        foreach (var transition in _globalTransitions)
        {
            if (transition.Condition.Invoke() == true)
            {
                return transition.To;
            }
        }
        if (_transitions.TryGetValue(from, out List<Transition> transitions))
        {
            foreach (var transition in transitions)
            {
                if (transition.Condition.Invoke() == true)
                {
                    return transition.To;
                }
            }
        }
        return null;
    }

}

public abstract class State
{
    public virtual void Start() { }
    public virtual void Tick() { }
    public virtual void End() { }

}

public sealed class Transition
{
    public readonly Func<bool> Condition;
    public readonly State To;

    public Transition(Func<bool> condition, State to)
    {
        Condition = condition;
        To = to;
    }

}