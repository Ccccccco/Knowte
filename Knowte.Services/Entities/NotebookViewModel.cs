using Prism.Mvvm;

namespace Knowte.Services.Entities
{
    public class NotebookViewModel : BindableBase
    {
        public const string AllNotesNotebookId = "739582a3-ce1d-471d-b8cd-ed962c55781c";
        public const string UnfiledNotesNotebookId = "4e1afdd5-c288-4ad7-aa43-8397b70eb889";

        public string Id { get; private set; }

        public string Title { get; private set; }

        public bool IsDefault
        {
            get
            {
                return this.Id.Equals(AllNotesNotebookId) || this.Id.Equals(UnfiledNotesNotebookId);
            }
        }

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
