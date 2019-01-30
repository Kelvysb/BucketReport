; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "BucketReport"
#define MyAppVersion "1.0.0.0"
#define MyAppPublisher "Kelvys B."
#define MyAppURL "https://github.com/Kelvysb/BucketReport"
#define MyAppExeName "BucketReport.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{5292AB21-3F6F-471A-A20D-2F564E57EDFD}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=C:\Users\k.boniek.pantaleao\OneDrive - Avanade\Documents\GitHub\BucketReport\LICENSE.txt
OutputDir=C:\Users\k.boniek.pantaleao\Desktop
OutputBaseFilename=setupBucketReport
SetupIconFile=C:\Users\k.boniek.pantaleao\OneDrive - Avanade\Documents\GitHub\BucketReport\BucketReport\icon.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\k.boniek.pantaleao\OneDrive - Avanade\Documents\GitHub\BucketReport\BucketReport\bin\Release\BucketReport.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\k.boniek.pantaleao\OneDrive - Avanade\Documents\GitHub\BucketReport\BucketReport\bin\Release\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{commonprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
