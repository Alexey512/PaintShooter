namespace ECS
{
	public interface IComponent
	{
		int TypeId { get; }
		
		public IEntity Owner { get; set; }

		public IActor Actor { get; }
	}
}
