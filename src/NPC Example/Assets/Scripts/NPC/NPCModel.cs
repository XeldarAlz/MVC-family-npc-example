using System;
using System.Collections.Generic;

using UnityEngine;

public class NPCModel : BaseModel
{
	public event Action<NPCState> OnStateChanged;
	
	public string Name = "";
	public float GreetingsDistance = 5f;
	public float PatrolDistance = 10f;
	public List<Vector3> MovePoints = new List<Vector3>();
	public float MoveSpeed = 1f;

	private NPCState _state = NPCState.Idle;
	public NPCState State
	{
		get => _state;
		set
		{
			_state = value;
			OnStateChanged?.Invoke(_state);
		}
	}
}
