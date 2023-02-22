namespace GraphEditor.Models.CRUD
{
    public class EntityBase
    {
        public readonly string Id;

        protected EntityBase(string id)
        {
            Id = id;
        }

        public EntityBase()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
