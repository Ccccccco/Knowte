using Knowte.Data.Contracts.Entities;
using Prism.Mvvm;

namespace Knowte.Presentation.Contracts.Entities
{
    public class CollectionViewModel : BindableBase
    {
        public Collection Collection { get; private set; }

        public bool IsActive => this.Collection.IsActive.Equals(1) ? true : false;

        public CollectionViewModel(Collection Collection)
        {
            this.Collection = Collection;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return this.Collection.Id.Equals(((CollectionViewModel)obj).Collection.Id);
        }

        public override int GetHashCode()
        {
            return this.Collection.GetHashCode();
        }

        public override string ToString()
        {
            return this.Collection.Title;
        }
    }
}
