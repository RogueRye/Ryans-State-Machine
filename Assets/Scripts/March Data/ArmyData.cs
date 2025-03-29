using UnityEngine;

[CreateAssetMenu(menuName = "March/Army Data")]
public class ArmyData : ScriptableObject
{
  [SerializeField]
  private string attackTrigger;

  [SerializeField]
  private float attackDuration;

  public string AttackTrigger => attackTrigger;

  public float AttackDuration => attackDuration;
}
