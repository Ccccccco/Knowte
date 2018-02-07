using Digimezzo.Foundation.Core.Packaging;
using System;

namespace Knowte.Core.Base
{
    public static class ProductInformation
    {
        public static string ApplicationName = "Knowte";
        public static string ApplicationGuid = "e3be6998-dbcf-4f99-b2b5-bf046fe680f7";
        public static string Copyright = "Copyright Digimezzo © " + DateTime.Now.Year;

        public static ExternalComponent[] Components = {
           new ExternalComponent {
                Name = "Prism",
                Description = "Prism is a framework for building loosely coupled, maintainable, and testable XAML applications in WPF, Windows 10 UWP, and Xamarin Forms.",
                Url = "https://github.com/PrismLibrary/Prism",
                LicenseUrl = "https://github.com/PrismLibrary/Prism/blob/master/LICENSE"
            },
            new ExternalComponent {
                Name = "Sqlite-net",
                Description = "A minimal library to allow .NET and Mono applications to store data in SQLite 3 databases.",
                Url = "https://github.com/praeclarum/sqlite-net",
                LicenseUrl = "https://github.com/praeclarum/sqlite-net/blob/master/LICENSE.md"
            },
            new ExternalComponent {
                Name = "Unity",
                Description = "A lightweight extensible dependency injection container with support for constructor, property, and method call injection.",
                Url = "https://github.com/unitycontainer/unity",
                LicenseUrl = "https://github.com/unitycontainer/unity/blob/master/LICENSE.txt"
            },
            new ExternalComponent {
                Name = "WiX",
                Description = "Windows Installer XML Toolset.",
                Url = "https://github.com/wixtoolset/wix3",
                LicenseUrl = "https://github.com/wixtoolset/wix3/blob/develop/LICENSE.TXT"
            }
        };
    }
}
