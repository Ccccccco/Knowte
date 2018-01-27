using Digimezzo.Utilities.Settings;

namespace Knowte.Core.IO
{
    public class ApplicationPaths
    {
        public static string ColorSchemesSubDirectory = "ColorSchemes";
        public static string BuiltinLanguagesFolder = "Languages";
        public static string CustomLanguagesFolder = "Languages";

        public static string DefaultNoteStorageLocation
        {
            get
            {
                return System.IO.Path.Combine(SettingsClient.ApplicationFolder());
            }
        }

        public static string CurrentNoteStorageLocation
        {
            get
            {
                if (IsUsingDefaultStorageLocation)
                {
                    return DefaultNoteStorageLocation;
                }
                else
                {
                    return SettingsClient.Get<string>("General", "NoteStorageLocation");
                }
            }
        }

        public static bool IsUsingDefaultStorageLocation
        {
            get
            {
                return string.IsNullOrWhiteSpace(SettingsClient.Get<string>("General", "NoteStorageLocation"));
            }
        }
    }
}
