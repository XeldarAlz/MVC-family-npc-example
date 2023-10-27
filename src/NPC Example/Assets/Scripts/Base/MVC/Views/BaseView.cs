using UnityEngine;

namespace MVC
{
	public class BaseView<M, C> : MonoBehaviour
		where M : BaseModel
		where C : BaseController<M>, new()
	{
		public M Model;
		protected C Controller;

		protected virtual void Awake()
		{
			Controller = new C();
			Controller.Setup(Model);
		}
	}
}
