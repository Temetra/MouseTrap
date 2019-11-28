namespace MouseTrap.UserInterface.State
{
	public class InitialState : BaseInterfaceState, IInterfaceState
	{
		public override void SwitchMode(IInterfaceStateContext context, ViewType viewType)
		{
			if (viewType == ViewType.FindProgram) context.SetCurrentState(new FindProgramState());
			else context.SetCurrentState(new WindowListState());
		}
	}
}
