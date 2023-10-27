using MVC;

using UnityEngine;
using UnityEngine.AI;

public class PlayerView : BaseView<PlayerModel, PlayerController>
{
	[SerializeField] NavMeshAgent _agent;

	Camera _camera;

	protected override void Awake()
	{
		Model = new PlayerModel
		{
			MoveSpeed = 5f
		};
		base.Awake();
	}

	void Start()
	{
		_camera = Camera.main;
		_agent.speed = Model.MoveSpeed;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
			{
				_agent.SetDestination(hit.point);
			}
		}
	}
}
