using System;
using System.Timers;

namespace Knowte.Services.Search
{
    public class SearchService : ISearchService
    {
        private Timer searchTimer = new Timer();
        private double searchTimeout = 0.2;

        private string searchText;

        public string SearchText
        {
            get { return this.searchText; }
            set
            {
                this.searchText = value;
                this.searchTimer.Start();
            }
        }

        public SearchService()
        {
            this.searchTimer.Interval = TimeSpan.FromSeconds(this.searchTimeout).TotalMilliseconds;

            this.searchTimer.Elapsed += (_, __) =>
            {
                this.searchTimer.Stop();
                this.SearchTextChanged(this, new EventArgs());
            };
        }

        public event EventHandler SearchTextChanged = delegate { };
    }
}
