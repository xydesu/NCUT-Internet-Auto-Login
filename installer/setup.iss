; NCUT Internet Auto Login - Inno Setup Script
; Build: ISCC /dMyAppVersion=1.0.0 installer\setup.iss

#ifndef MyAppVersion
  #define MyAppVersion "1.0.0"
#endif

#define MyAppName    "NCUT Internet Auto Login"
#define MyAppExeName "NCUT-Internet-Auto-Login.exe"
#define MyServiceName "NCUT Auto Login Service"

[Setup]
AppId={{F9CF5BF2-F88C-4A93-95D4-462B419838E0}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher=sangege & AI LIFE
AppPublisherURL=https://github.com/xydesu/NCUT-Internet-Auto-Login
AppSupportURL=https://github.com/xydesu/NCUT-Internet-Auto-Login
AppUpdatesURL=https://github.com/xydesu/NCUT-Internet-Auto-Login/releases
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputDir=dist
OutputBaseFilename=NCUT-Internet-Auto-Login-Setup
SetupIconFile=ncut_setup.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
MinVersion=10.0

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
; All published files (GUI exe + Worker dll + dependencies)
Source: "..\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: nowait postinstall skipifsilent

[Code]
const
  ServiceName = '{#MyServiceName}';

// ──────────────────────────────────────────────────
// Find the full path to dotnet.exe
// ──────────────────────────────────────────────────
function GetDotnetExePath(): String;
var
  Path64, Path32: String;
begin
  Path64 := ExpandConstant('{pf64}\dotnet\dotnet.exe');
  Path32 := ExpandConstant('{pf}\dotnet\dotnet.exe');
  if FileExists(Path64) then
    Result := Path64
  else if FileExists(Path32) then
    Result := Path32
  else
    Result := '';
end;

// ──────────────────────────────────────────────────
// Register the Worker as a Windows service
// ──────────────────────────────────────────────────
procedure RegisterService();
var
  DotnetPath, DllPath, Args: String;
  ResultCode: Integer;
begin
  DotnetPath := GetDotnetExePath();
  if DotnetPath = '' then
  begin
    MsgBox(
      '.NET 9 Runtime not found. The background service will not be registered.' + #13#10 +
      'Please install .NET 9 Runtime and re-run the installer.',
      mbError, MB_OK);
    Exit;
  end;

  DllPath := ExpandConstant('{app}\NCUT-Internet-Auto-Login.Worker.dll');

  // sc.exe binPath= value is the full service command line:
  //   "<dotnet.exe full path>" "<worker.dll full path>"
  // The surrounding quotes inside binPath= are escaped as \" for sc.exe
  Args := 'create "' + ServiceName + '" binPath= "\"' + DotnetPath +
          '\" \"' + DllPath + '\"" start= auto DisplayName= "' + ServiceName + '"';
  Exec(ExpandConstant('{sys}\sc.exe'), Args, '',
    SW_HIDE, ewWaitUntilTerminated, ResultCode);

  Exec(ExpandConstant('{sys}\sc.exe'),
    'description "' + ServiceName + '" ' +
    '"NCUT campus network auto-login service"',
    '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
end;

// ──────────────────────────────────────────────────
// Stop and remove the service (used on upgrade/uninstall)
// ──────────────────────────────────────────────────
procedure StopAndRemoveService();
var
  ResultCode: Integer;
begin
  Exec(ExpandConstant('{sys}\sc.exe'), 'stop "' + ServiceName + '"',
    '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
  Sleep(3000);
  Exec(ExpandConstant('{sys}\sc.exe'), 'delete "' + ServiceName + '"',
    '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssInstall then
    StopAndRemoveService();   // stop old service before overwriting files
  if CurStep = ssPostInstall then
    RegisterService();        // register the fresh service
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usUninstall then
    StopAndRemoveService();
end;

// ──────────────────────────────────────────────────
// Warn the user if .NET 9 is not detected
// ──────────────────────────────────────────────────
function InitializeSetup(): Boolean;
begin
  Result := True;
  if GetDotnetExePath() = '' then
  begin
    if MsgBox(
      '.NET 9 Runtime was not found on this machine.' + #13#10 +
      'The background Worker service requires .NET 9 to run.' + #13#10 + #13#10 +
      'Click Yes to open the .NET 9 download page, then re-run the installer.' + #13#10 +
      'Click No to continue installing without it.',
      mbConfirmation, MB_YESNO) = IDYES then
    begin
      ShellExec('open',
        'https://dotnet.microsoft.com/download/dotnet/9.0',
        '', '', SW_SHOW, ewNoWait, 0);
      Result := False;   // abort setup; user should install .NET first
    end;
  end;
end;
