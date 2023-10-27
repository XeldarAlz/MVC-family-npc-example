namespace MVC
{
	public class NPCController : BaseController<NPCModel>
	{
		public void UpdateState(float playerDistance)
		{
			NPCState state = playerDistance switch
			{
				var distance when distance >= Model.PatrolDistance => NPCState.Patrol,
				var distance when distance <= Model.GreetingsDistance => NPCState.Greetings,
				_ => NPCState.Idle
			};
			Model.State = state;
		}
	}
}
