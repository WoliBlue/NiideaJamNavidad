using UnityEngine;

// Se añade al objeto de Persona junto con Persona.cs para definir su personalidad.
// La personalidad hará que salgan ciertos diálogos y responda mejor ante ciertas respuestas
// Cuando respondes bien, se suma +1 al DecisionFinal. Cuando respondes mal, se resta -1.
// Si respondes neutro, no suma nada
// Si el balance es positivo después de varias interacciones, 
// la Persona te dirá que figura quiere y se la podrás dar.
// Si el balance es negativo, la Persona se irá y no te comprará nada.

public class PersonPersonality : MonoBehaviour {
  #region Variables
  [Header("Personality Info")]
  [SerializeField] private PersonPersonalityData _data;
  private int _humourBalance;
  #endregion

  #region Events
  private void Awake() {
    InitData();
  }
  #endregion

  #region Public Methods
  public void Add(int value) {
    _humourBalance += value;
  }
  public bool CheckHumourToBuy() {
    return _humourBalance > 0;
  }
  #region Getters
  public int HumourBalance => _humourBalance;
  public PersonPersonalityData Data => _data;
  #endregion
  #endregion

  #region Private Methods
  /// <summary>
  /// Method to load the associated data and not to change it to others 
  /// that have the same data
  /// </summary>
  private void InitData() {
    if (!_data) {
      Debug.LogWarning("PersonPersonalityData no asociada!");
      return;
    }

    _humourBalance = _data.InitialHumourBalance;
  }
  #endregion
}
