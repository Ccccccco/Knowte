namespace Knowte.PluginBase.Collection.Entities
{
    public interface INote
    {
        string Id { get; set; }

        string Title { get; set; }

        string NotebookId { get; set; }

        string Text { get; set; }

        long CreationDate { get; set; }

        long OpenDate { get; set; }

        long ModificationDate { get; set; }

        long Width { get; set; }

        long Height { get; set; }

        long Top { get; set; }

        long Left { get; set; }

        long Flagged { get; set; }

        long Maximized { get; set; }
    }
}
