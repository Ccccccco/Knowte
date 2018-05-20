using System;

namespace Knowte.Services.Search
{
    public class SearchService : ISearchService
    {
        public string SearchText { get; set; }

        public event EventHandler SearchTextChanged = delegate { };
    }
}
