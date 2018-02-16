namespace Knowte.Presentation.Contracts.Entities
{
    public class NotebookViewModel
    {
        public string Id { get; private set; }

        public string Title { get; private set; }

        public NotebookViewModel(string id, string title)
        {
            this.Id = id;
            this.Title = title;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return this.Id.Equals(((NotebookViewModel)obj).Id);
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
