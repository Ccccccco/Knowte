using System;

namespace Knowte.Services.Search
{
    public interface ISearchService
    {
        string SearchText { get; set; }

        event EventHandler SearchTextChanged;
    }
}
