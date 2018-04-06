# How to write plugins

Writing plugins for Knowte is very easy. These are the steps:

* In Visual Studio, create a new Class Library project (.NET version >= 4.6.1)
* Add a reference to this dll: Knowte.WPF.PluginBase
* Implement one of the following interfaces:
..* Collection\ICollectionProvider
..* (Currently, there is only 1 interface available: ICollectionProvider)
* Add an attribute above your class definition, indicating which interface was implemented. E.g.: [Export(typeof(ICollectionProvider))]
* Compile your Class Library project
* Copy the compiled dll to the directory %appdata%\Knowte\Plugins
* Knowte will automatically pick up the plugin!
