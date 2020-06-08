namespace Untitled
{
    namespace Resource
    {
		public interface  IResourceStorage 
		{
			void AddResources(ResourceType type, float count);
			float GetResourceCount(ResourceType type);
		}
	}
}
