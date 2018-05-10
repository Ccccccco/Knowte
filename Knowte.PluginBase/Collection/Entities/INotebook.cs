namespace Knowte.PluginBase.Collection.Entities
{
    public interface INotebook
    {
        string Id { get; set; }

        string Title { get; set; }

        string CollectionId { get; set; }

        long CreationDate { get; set; }
    }
}
