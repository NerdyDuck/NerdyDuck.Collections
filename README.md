# NerdyDuck.Collections

This project provides a library with a set of specialized collections, e.g. for multi-threading purposes.

#### Platforms
- .NET Framework 4.6 or newer for desktop applications
- Universal Windows Platform (UWP) 10.0 (Windows 10) or newer for Windows Store Apps and [Windows 10 IoT](https://dev.windows.com/en-us/iot).

#### Dependencies
The project uses the following NuGet packages that are either found on NuGet.org or my own feed (see below):
- NerdyDuck.CodedExceptions

#### Languages
The neutral resource language for all texts is English (en-US). Currently, the only localization available is German (de-DE). If you like to add other languages, feel free to send a pull request with the translated resources!

#### How to get
- Use the NuGet package from my [MyGet](https://www.myget.org) feed: [https://www.myget.org/F/nerdyduck-release/api/v3/index.json](https://www.myget.org/F/nerdyduck-release/api/v3/index.json). If you need to debug the library, get the debug symbols from the same feed: [https://www.myget.org/F/nerdyduck-release/symbols/](https://www.myget.org/F/nerdyduck-release/symbols/).
- Download the binaries from the [Releases](../../releases/) page.
- You can clone the repository and compile the libraries yourself (see the [Wiki](../../wiki/) for requirements).

#### More information
For examples and a complete class reference, please see the [Wiki](../../wiki/). :exclamation: **Work in progress**.

#### Licence
The project is licensed under the [Apache License, Version 2.0](LICENSE).

#### History
#####2016-04-13 / v1.0.3 / DAK
- Updated reference for [NerdyDuck.CodedExceptions](../NerdyDuck.CodedExceptions) to v1.2.1.
- Switched exception error codes from literals to `ErrorCodes` enumeration, including comment text from ErrorCodes.csv.

#####2016-04-06 / v1.0.2 / DAK
- Updated reference for [NerdyDuck.CodedExceptions](../NerdyDuck.CodedExceptions) to v1.2.0.
- Added deployment project to compile all projects and create/push the NuGet package in one go. Removed separate NuGet project. Removes also dependency on NuGet Packager Template.
- Extracted file signing into its own reusable MSBuild target file.
- Extracted resource generation for desktop project into its own reusable MSBuild target file.
- Created a MSBuild target for automatic T4 transformations on build. Removes dependency on Visual Studio Modeling SDK.

##### 2016-02-26 / v1.0.1 / DAK
- Fixed: Wrong build action for Resources.tt breaks NuGet package for UWP, because the file is not part of package, but required as payload.

##### 2016-02-16 / v1.0.0 / DAK
- First release. Contains (Non)BlockingConcurrentList(T) and (Non)BlockingConcurrentDictionary(T).
