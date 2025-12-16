public interface IPersonState {
  #region Public Methods
  public void Enter(Person person);
  public void Update(Person person);
  public void Exit(Person person);
  #endregion
}
