using Prism.Mvvm;

namespace Knowte.Presentation.Contracts.Entities
{
    public class CollectionViewModel : BindableBase
    {
        public string Id { get; private set; }

        public string Title { get; private set; }

        public bool IsActive { get; private set; }

        public CollectionViewModel(string id, string title, long isActive)
        {
            this.Id = id;
            this.Title = title;
            this.IsActive = isActive.Equals(1) ? true : false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return this.Id.Equals(((CollectionViewModel)obj).Id);
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
