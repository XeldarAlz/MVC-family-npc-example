using System;

namespace MVP
{
	public class NPCPresenter : BasePresenter<NPCModel>
	{
		public event Action<float> OnMoveSpeedChanged;
		public event Action OnStopPatrol;
		public event Action OnStartPatrol;
		public event Action OnLookAtPlayer;
		public event Action<string> OnAnimatorTriggerChanged;

		NPCState _lastState;

		public NPCPresenter()
		{
			Model = new NPCModel()
			{
				Name = "Jack",
				MoveSpeed = 3f,
				GreetingsDistance = 7f,
				PatrolDistance = 15f
			};
			Model.OnStateChanged += UpdateBehaviour;
		}

		~NPCPresenter()
		{
			Model.OnStateChanged -= UpdateBehaviour;
		}

		void UpdateBehaviour(NPCState state)
		{
			switch (state)
			{
				case NPCState.Idle:
					if (_lastState != NPCState.Idle)
					{
						OnStopPatrol?.Invoke();
						OnLookAtPlayer?.Invoke();
						OnAnimatorTriggerChanged?.Invoke("Idle");
					}

					break;
				case NPCState.Patrol:
					OnStartPatrol?.Invoke();
					OnMoveSpeedChanged?.Invoke(Model.MoveSpeed);
					if (_lastState != NPCState.Patrol)
					{
						OnAnimatorTriggerChanged?.Invoke("Patrol");
					}

					break;
				case NPCState.Greetings:
					if (_lastState != NPCState.Greetings)
					{
						OnStopPatrol?.Invoke();
						OnLookAtPlayer?.Invoke();
						OnAnimatorTriggerChanged?.Invoke("Greetings");
					}

					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			_lastState = Model.State;
		}

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
