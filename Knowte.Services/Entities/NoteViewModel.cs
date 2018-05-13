using Digimezzo.Foundation.Core.Settings;
using Knowte.Core.Utils;

namespace Knowte.Services.Entities
{
    public class NoteViewModel
    {
        public string Id { get; private set; }

        public string Title { get; private set; }

        public long ModificationDate { get; private set; }

        public bool IsMarked { get; set; }

        public string FormattedModificationDate
        {
            get
            {
                return DateUtils.FormatNoteModificationDate(
                    this.ModificationDate,
                    SettingsClient.Get<bool>("Notes", "ShowExactDates"));
            }
        }


        public NoteViewModel(string id, string title, long modificationDate, long flagged)
        {
            this.Id = id;
            this.Title = title;
            this.ModificationDate = modificationDate;
            this.IsMarked = flagged == 1 ? true : false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return this.Id.Equals(((NoteViewModel)obj).Id);
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
