using System.Diagnostics;
using UnityEngine;

public class MarchArmyView : MonoBehaviour
{
    [SerializeField]
    private Transform origin;
    [SerializeField]
    private Transform destination;

    private StateMachine stateMachine;
    private Animator animator;
    private ArmyData armyData;

    private Stopwatch timer;

    private bool ReachedTarget(Vector3 target) => Vector3.Distance(gameObject.transform.position, target) < Mathf.Epsilon;
    private bool DoneAttacking() => timer.Elapsed.Seconds >= armyData.AttackDuration;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        armyData = GetComponentInChildren<ArmyDataHolder>().ArmyData;
        timer = new Stopwatch();
        
        var o = gameObject;
        var marching = new MarchingState(o.transform, destination.position, 10);
        var returning = new MarchingState(o.transform, origin.position, 10);
        var attacking = new AttackState(timer, animator, armyData.AttackTrigger);

        stateMachine = new StateMachine();
        
        stateMachine.AddTransition(marching, attacking, () => ReachedTarget(destination.position));
        stateMachine.AddTransition(attacking, returning, DoneAttacking);
        stateMachine.AddTransition(returning, marching, () => ReachedTarget(origin.position));
        
        stateMachine.SetState(marching);

    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.UpdateCurrentState();
    }
}


public class MarchingState : IState
{
    private readonly Transform march;
    private readonly Vector3 destination;
    private readonly float speed;

    public MarchingState(Transform march, Vector3 destination, float speed)
    {
        this.march = march;
        this.destination = destination;
        this.speed = speed;
    }
    public void Enter()
    {
    }
    public void Update()
    {
        march.position = Vector3.MoveTowards(march.position, destination, speed * Time.deltaTime);
    }
    public void Exit()
    {
        //Do nothing
    }
}

public class AttackState : IState
{
    private readonly Stopwatch timer;
    private readonly Animator animator;
    private readonly string attackTrigger;

    public AttackState(Stopwatch timer, Animator animator, string attackTrigger)
    {
        this.timer = timer;
        this.animator = animator;
        this.attackTrigger = attackTrigger;
    }
    public void Enter()
    {
        timer.Start();
        animator.SetTrigger(attackTrigger);
    }
    public void Update()
    {
        //
    }
    public void Exit()
    {
        timer.Stop();
        timer.Reset();
    }
}

