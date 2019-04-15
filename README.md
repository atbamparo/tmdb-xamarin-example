# tmdb-xamarin-example

## Build instructions
 * Open the TMDbExample\TMDbExample.sln with VS 2017+ with Xamarin features turned on
 * **Replace TMDbExample\src\TMDbExample.Forms\ViewModels\ViewModelLocator.cs, line 16 "API-KEY" string with a valid TMDb API-KEY**
 ``` csharp
 const string TMDbApiKey = "API-KEY";
 ```
 should looks like 
 ``` csharp
 const string TMDbApiKey = "f263b70fd8b07829fda6ab82c1c7a600";
 /* this is an invalid key used only for demonstration */
 ```
* Build and deploy project TMDbExample.Android to a Android smartphone or emulator (Tested with Android 9 API 28)

## Notes
* Deleted the iOS project because i'm unable to build and test it.

## Thirdy party libraries
* **Xamarin** - toolkit used to create and build the whole app
* **AutoFac** - Dependency injection container that i already work with, used to resolve the core project services and repositories. Used in TMDbExample\src\TMDbExample.Forms\ViewModels\ViewModelLocator.cs
