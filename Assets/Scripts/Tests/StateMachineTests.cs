using System;
using NUnit.Framework;

public class StateMachineTests
{
  [Test]
  public void StateMachine_CanTransition()
  {
    var stateMachine = new StateMachine();
    var stateA = new MockStateA();
    var stateB = new MockStateB();

    stateMachine.AddTransition(stateA, stateB, () => true);
    
    stateMachine.SetState(stateA);
    Assert.True(stateA.EnterCalled);
    
    stateMachine.UpdateCurrentState();
    Assert.True(stateA.ExitCalled);
    Assert.True(stateB.EnterCalled);
    Assert.True(stateB.UpdateCalled);
  }
  
  [Test]
  public void StateMachine_Wont_Transition_On_False_Condition()
  {
    var stateMachine = new StateMachine();
    var stateA = new MockStateA();
    var stateB = new MockStateB();

    stateMachine.AddTransition(stateA, stateB, () => false);
    
    stateMachine.SetState(stateA);
    Assert.True(stateA.EnterCalled);
    
    stateMachine.UpdateCurrentState();
    Assert.False(stateA.ExitCalled);
    Assert.False(stateB.EnterCalled);
    Assert.False(stateB.UpdateCalled);
  }

  [Test]
  public void StateMachine_CanTransition_Changing_Condition()
  {
    var stateMachine = new StateMachine();
    var stateA = new MockStateA();
    var stateB = new MockStateB();

    var val = 0;

    stateMachine.AddTransition(stateA, stateB, () => val >= 6);
    
    stateMachine.SetState(stateA);
    Assert.True(stateA.EnterCalled);
    
    stateMachine.UpdateCurrentState();

    Assert.False(stateA.ExitCalled);
    Assert.False(stateB.EnterCalled);
    Assert.False(stateB.UpdateCalled);

    val = 10;
    
    stateMachine.UpdateCurrentState();

    Assert.True(stateA.EnterCalled);
    Assert.True(stateA.ExitCalled);
    Assert.True(stateB.EnterCalled);
    Assert.True(stateB.UpdateCalled);
  }

  [Test]
  public void StateMachine_Errors_NoCurrentStateSet()
  {
    var stateMachine = new StateMachine();
    var stateA = new MockStateA();
    var stateB = new MockStateB();
    stateMachine.AddTransition(stateA, stateB, () => true);

    Assert.Throws<Exception>(() => stateMachine.UpdateCurrentState());
  }
  
  /*[Test]
  public void StateMachine_AddTransition_NullArguments_Exception()
  {
    var stateMachine = new StateMachine();
    var stateA = new MockStateA();
    Assert.Throws<NullReferenceException>(() => stateMachine.AddTransition(null, stateA, () => true));

    stateMachine = new StateMachine();
    stateA = new MockStateA();
    Assert.Throws<NullReferenceException>(() => stateMachine.AddTransition( stateA, null, () => true));
    
    stateMachine = new StateMachine();
    stateA = new MockStateA();
    var stateB = new MockStateB();
    Assert.Throws<NullReferenceException>(() => stateMachine.AddTransition( stateA, stateB, null));
  }*/
}


public class MockStateA : IState
{
  public bool EnterCalled;
  public bool UpdateCalled;
  public bool ExitCalled;
  public void Enter()
  {
    EnterCalled = true;
  }
  public void Update()
  {
    UpdateCalled = true;
  }
  public void Exit()
  {
    ExitCalled = true;
  }
}

public class MockStateB : IState
{
  public bool EnterCalled;
  public bool UpdateCalled;
  public bool ExitCalled;
  public void Enter()
  {
    EnterCalled = true;
  }
  public void Update()
  {
    UpdateCalled = true;
  }
  public void Exit()
  {
    ExitCalled = true;
  }
}
