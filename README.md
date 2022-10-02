# FingerprintCombos
[Latest Release](https://github.com/bencharpit/FingerprintCombos/releases/tag/Release)

#### Protect your combo cloud by adding a fingerprint to your combos!

## Features:
- Find your combos lines in fully modified/randomized combos, protect them from being leaked
- Optimized to handle 1000's of combos/millions of lines at a time
- Can be used for lines: EMAIL:PASS | USER:PASS

## Instructions:
1. Download .NET core runtime in case you don't have it already downloaded
    - Direct links: [Windows with 64 Bits](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-3.1.29-windows-x64-installer) | [Windows with 32 Bits](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-3.1.29-windows-x86-installer) - [macOS](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-3.1.29-macos-x64-installer)
2. Download [Latest release](https://github.com/bencharpit/FingerprintCombos/releases/tag/Release)
3. Open the .EXE in case you are on Windows, in case you are on macOS/Linux, go to the directory and type in the console: `dotnet run FingerprintCombos.dll`
4. Go to option 1 to start protecting your combos, they will be saved in /ProtectedCombos/ComboName.txt-date/ComboName-Protected.txt
    - You are free to post this combo somewhere safe
5. To search for the fingerprints of your combos in 3rd party combos, upload the 3rd party text files in /CheckCombos/ and simply open option 2
    - This will save a report in **Reports/report-date.txt**, inside will appear the names of the combos that were investigated with their fingerprints
    - Having only 1 fingerprint in a third party combo means that it has lines belonging to your combo
    
## Credits:

Developed by https://t.me/Charpit, you are free to modify it if you like, please read the [license agreement](https://github.com/bencharpit/FingerprintCombos/blob/master/LICENSE) before making changes to the source code.

## Extra:

I am not responsible for any malicious use of this tool, it is created for the sole purpose of 
showing how a fingerprint for text files could work based on the sale of YOUR own accounts,
I DO NOT SUPPORT the distribution of stolen credentials.
