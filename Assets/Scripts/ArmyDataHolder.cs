using UnityEngine;
public class ArmyDataHolder : MonoBehaviour
{
  [SerializeField]
  private ArmyData armyData;
  
  public ArmyData ArmyData => armyData;
}
