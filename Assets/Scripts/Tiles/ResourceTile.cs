using Untitled.Resource;

namespace Untitled
{
    namespace Tiles
    {
        public class ResourceTile : IResourceStorage
        {
            private float val;
			
            public void AddResources(ResourceType type, float count) {
              val += count;
            }

            public float GetResourceCount(ResourceType type) {
              return val;
            }

            public ResourceTile(float startingVal)
            {
                val = startingVal;
            }
        }
    }
}

