using UnityEngine;

[CreateAssetMenu(fileName = "PersonPersonalityData", menuName =
  "Game/Person/PerdonalityData")]
public class PersonPersonalityData : ScriptableObject {
  #region Variables
  [Header("General")]
  [SerializeField] private string _name;

  [Header("Dialogue")]
  [SerializeField] private int _dialogID;

  [Header("Stats")]
  [SerializeField] private int _initialHumourBalance;
  #endregion

  #region Getters
  public int InitialHumourBalance => _initialHumourBalance;
  public int DialogID => _dialogID;
  public string Name => _name;
  #endregion
}
