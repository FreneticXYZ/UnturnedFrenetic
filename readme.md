Unturned Frenetic
-----------------

A mod for Unturned; adds a full high-power script engine to allow dynamic modding!

# First time building

- Open UnturnedFreneticInjector.sln in Visual Studio 2015
- VS2015 -> Build -> Build Solution
- Open your favorite command line, EG Bash, Powershell, Cmd, or whatever - open in the Injector's release folder
- Run the injector via command line
- Open UnturnedFrenetic.sln in Visual Studio 2015
- VS2015 -> Build -> Build Solution
- Copy Injector/Release/Assembly-CSharp.Patched.dll to Unturned's Managed/Assembly-CSharp.dll
- Copy UnturnedFrenetic/Release/Frenetic.dll and UnturnedFrenetic.dll to Unturned's Managed folder as well.
- Launch the Unturned server!
- Be warned, your local client may no longer be able to play games until you revert Assembly-CSharp.dll to its original form. Make sure you are running off a server box!