using Digimezzo.Foundation.Core.Settings;

namespace Knowte.Core.IO
{
    public class ApplicationPaths
    {
        public static string ColorSchemesDirectory = "ColorSchemes";
        public static string BuiltinLanguagesDirectory = "Languages";
        public static string CustomLanguagesDirectory = "Languages";
        public static string PluginsDirectory = "Plugins";

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
