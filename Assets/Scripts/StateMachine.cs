using System;
using System.Collections.Generic;

/// <summary>
/// Class representing a finite state machine
/// </summary>
public class StateMachine
{
  private IState currentState;

  //"Type" is a concrete implementation of IState being transitioned From
  private Dictionary<Type, List<Transition>> transitions = new();
  
  private List<Transition> currentStateTransitions = new();
  
  // Transitions that can occur from any state
  private List<Transition> anyTransitions = new();

  // Pre allocated list of to assign to states with no transitions to avoid excessive allocations when setting states
  private static List<Transition> emptyTransitions = new(0);


  /// <summary>
  /// Checks Transitions conditions for current state and transitions if needed
  /// Otherwise runs the current state's update method
  /// </summary>
  public void UpdateCurrentState()
  {
    if (currentState == null)
    {
      throw new Exception("No Initial State Set");
    }
    
    var (shouldTransition, transition) = CheckTransition();
    if (shouldTransition)
    {
      SetState(transition.To);
    }
  
    currentState.Update();
  }
  

  public void SetState(IState state)
  {
    if (state == currentState)
    {
      return;
    }
    
    currentState?.Exit();
    currentState = state;

    if (!transitions.TryGetValue(currentState.GetType(), out currentStateTransitions))
    {
      currentStateTransitions = emptyTransitions;
    }
    
    currentState.Enter();
  }

  /// <summary>
  /// Adds a state transition
  /// </summary>
  /// <param name="from">State to add the transition to</param>
  /// <param name="to">State this transition will connect to</param>
  /// <param name="condition">Condition to trigger this transition</param>
  public void AddTransition(IState from, IState to, Func<bool> condition)
  {
    if (!transitions.TryGetValue(from.GetType(), out var stateTransitions))
    {
      stateTransitions = new();
      transitions[from.GetType()] = stateTransitions;
    }
    
    stateTransitions.Add(new Transition(to, condition));
  }


  public void AddAnyTransition(IState to, Func<bool> condition)
  {
    anyTransitions.Add(new Transition(to, condition));
  }

  /// <summary>
  /// Checks transitions conditions
  /// Prioritizes any transitions before current state transitions
  /// Will return the first transition found with no concept of explicit priority if multiple conditions are met.
  /// This will be based on the call order of AddTransition
  /// </summary>
  /// <returns>If a transitions is happening as well as the transition itself</returns>
  private (bool, Transition) CheckTransition()
  {
    //check any transitions first
    foreach (var transition in anyTransitions)
    {
      if (transition.Condition())
      {
        return (true, transition);
      }
    }
    
    //check state transitions
    foreach (var transition in currentStateTransitions)
    {
      if (transition.Condition())
      {
        return (true, transition);
      }
    }
    
    return (false, null);
  }
  
  private record Transition(IState To, Func<bool> Condition)
  {
    public IState To { get; } = To;

    public Func<bool> Condition { get; } = Condition;
  }

}


