using Prism.Mvvm;

namespace Knowte.ViewModels
{
    public class NoteViewModel : BindableBase
    {
        private string title;

        public string Title
        {
            get { return this.title; }
            set { SetProperty<string>(ref this.title, value); }
        }
    }
}
