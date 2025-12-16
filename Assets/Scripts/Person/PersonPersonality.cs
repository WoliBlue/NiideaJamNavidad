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
  [SerializeField] private int _humourBalance;
  // [SerializeField] private Dialog _dialog;
  // public Dialog Dialog => _dialog;
  #endregion

  #region Public Methods
  public void Add(int value) {
    _humourBalance += value;
  }
  #region Getters
  public int HumourBalance => _humourBalance;
  #endregion
  #endregion
}
