using Digimezzo.Foundation.Core.Settings;

namespace Knowte.Core.IO
{
    public class ApplicationPaths
    {
        public static string ColorSchemesFolder = "ColorSchemes";
        public static string BuiltinLanguagesFolder = "Languages";
        public static string CustomLanguagesFolder = "Languages";
        public static string PluginsFolder = "Plugins";

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
