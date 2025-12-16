using UnityEngine;

public class PersonStateLeave : IPersonState {
  #region Public Methods
  public void Enter(Person person) {
    Debug.Log(person.Name + " se va\n");
  }
  public void Update(Person person) {
    Debug.Log(person.Name + " se está yendo...\n");
  }
  public void Exit(Person person) {
    Debug.Log(person.Name + " deja de irse\n");
    person.FinishBuying();
  }
  #endregion
}
