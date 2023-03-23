namespace ECS
{
	public interface IComponent
	{
		public IEntity Owner { get; set; }
	}
}
