using SQLite;

namespace Knowte.Data.Contracts.Entities
{
    public class Collection
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Title { get; set; }

        public long IsActive { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return this.Id.Equals(((Collection)obj).Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
