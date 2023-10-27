using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

namespace MVC
{
	public class NPCView_MVC : BaseView<NPCModel, NPCController>
	{
		[SerializeField] NavMeshAgent _agent;
		[SerializeField] Animator _animator;
		[SerializeField] List<Vector3> _movePoints = new List<Vector3>();

		Transform _player;
		int _lastMovePointIndex = 0;
		NPCState _lastState;

		protected override void Awake()
		{
			Model = new NPCModel()
			{
				Name = "Jack",
				MovePoints = _movePoints,
				MoveSpeed = 3f,
				GreetingsDistance = 7f,
				PatrolDistance = 15f,
			};
			base.Awake();
			Model.OnStateChanged += UpdateBehaviour;
		}

		void OnDestroy()
		{
			Model.OnStateChanged -= UpdateBehaviour;
		}

		void Start()
		{
			_player = FindObjectOfType<PlayerView>().transform;
			_agent.speed = Model.MoveSpeed;
		}

		void Update()
		{
			UpdateState();
			if (Model.State == NPCState.Patrol)
			{
				Patrol();
			}
		}

		void UpdateState()
		{
			float distance = Vector3.Distance(_player.position, transform.position);
			Controller.UpdateState(distance);
		}

		void UpdateBehaviour(NPCState state)
		{
			switch (state)
			{
				case NPCState.Idle:
					if (_lastState != NPCState.Idle)
					{
						StopMoving();
						LookAtPlayer();
						_animator.SetTrigger("Idle");
					}

					break;
				case NPCState.Patrol:
					if (_lastState != NPCState.Patrol)
					{
						_animator.SetTrigger("Patrol");
					}

					break;
				case NPCState.Greetings:
					if (_lastState != NPCState.Greetings)
					{
						StopMoving();
						LookAtPlayer();
						_animator.SetTrigger("Greetings");
					}

					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			_lastState = Model.State;
		}

		void StopMoving()
		{
			_agent.SetDestination(transform.position);
			_agent.isStopped = true;
		}

		void LookAtPlayer()
		{
			transform.LookAt(_player);
		}

		void Patrol()
		{
			_agent.isStopped = false;
			_agent.SetDestination(_movePoints[_lastMovePointIndex]);
			if (Vector3.Distance(transform.position, _movePoints[_lastMovePointIndex]) <= 2f)
			{
				_lastMovePointIndex = _lastMovePointIndex >= _movePoints.Count - 1 ? 0 : _lastMovePointIndex + 1;
			}
		}
	}
}
