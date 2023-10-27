using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

namespace MVP
{
	public class NPCView_MVP : BaseView<NPCPresenter, NPCModel>
	{
		[SerializeField] NavMeshAgent _agent;
		[SerializeField] Animator _animator;
		[SerializeField] List<Vector3> _movePoints = new List<Vector3>();

		Transform _player;
		int _lastMovePointIndex = 0;
		NPCState _lastState;
		NPCPresenter _npcPresenter;
		bool _isPatrolling;

		void Awake()
		{
			_player = FindObjectOfType<PlayerView>().transform;
			_npcPresenter = new NPCPresenter();
		}

		void Start()
		{
			_npcPresenter.OnMoveSpeedChanged += ChangeSpeed;
			_npcPresenter.OnStopPatrol += StopMoving;
			_npcPresenter.OnLookAtPlayer += LookAtPlayer;
			_npcPresenter.OnAnimatorTriggerChanged += ChangeAnimation;
			_npcPresenter.OnStartPatrol += StartPatrol;
		}

		void StartPatrol()
		{
			_isPatrolling = true;
		}

		void ChangeAnimation(string trigger)
		{
			_animator.SetTrigger(trigger);
		}

		void LookAtPlayer()
		{
			transform.LookAt(_player);
		}

		void StopMoving()
		{
			_isPatrolling = false;
			_agent.SetDestination(transform.position);
			_agent.isStopped = true;
		}

		void ChangeSpeed(float speed)
		{
			_agent.speed = speed;
		}

		void OnDestroy()
		{
			_npcPresenter.OnMoveSpeedChanged -= ChangeSpeed;
			_npcPresenter.OnStopPatrol -= StopMoving;
			_npcPresenter.OnLookAtPlayer -= LookAtPlayer;
			_npcPresenter.OnAnimatorTriggerChanged -= ChangeAnimation;
		}

		void Update()
		{
			UpdateState();
			if (_isPatrolling)
			{
				Patrol();
			}
		}

		void UpdateState()
		{
			float distance = Vector3.Distance(_player.position, transform.position);
			_npcPresenter.UpdateState(distance);
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
