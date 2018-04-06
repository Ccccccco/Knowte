using Knowte.Plugin.Contracts.Collection.Entities;
using SQLite;
using System;

namespace Knowte.Data.Entities
{
    public class Collection : ICollection
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Title { get; set; }

        public long IsActive { get; set; }

        public Collection()
        {
        }

        public Collection(string title, long isActive)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Title = title;
            this.IsActive = isActive;
        }

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
