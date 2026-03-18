; NCUT Internet Auto Login - Inno Setup Script
; Build: ISCC /dMyAppVersion=1.0.0 installer\setup.iss

#ifndef MyAppVersion
  #define MyAppVersion "1.0.0"
#endif

#define MyAppName    "NCUT Internet Auto Login"
#define MyAppExeName "NCUT-Internet-Auto-Login.exe"
#define MyServiceName "NCUT Auto Login Service"

[Setup]
AppId={{F9CF5BF2-F88C-4A93-95D4-462B419838E0}}
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
; All published files (GUI exe + Worker self-contained exe + dependencies)
Source: "..\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: nowait postinstall skipifsilent

[Code]
const
  ServiceName = 'NCUT Auto Login Service';

procedure RegisterService();
var
  ExePath: String;
  ResultCode: Integer;
begin
  ExePath := ExpandConstant('{app}\NCUT-Internet-Auto-Login.Worker.exe');

  if not FileExists(ExePath) then
  begin
    MsgBox('Worker EXE not found: ' + ExePath, mbError, MB_OK);
    Exit;
  end;

  Exec(ExpandConstant('{sys}\sc.exe'),
    'create "' + ServiceName + '" binPath= "' + ExePath + '" start= auto DisplayName= "' + ServiceName + '"',
    '', SW_HIDE, ewWaitUntilTerminated, ResultCode);

  Exec(ExpandConstant('{sys}\sc.exe'),
    'description "' + ServiceName + '" "NCUT campus network auto-login service"',
    '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
end;

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
    StopAndRemoveService();

  if CurStep = ssPostInstall then
    RegisterService();
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usUninstall then
    StopAndRemoveService();
end;

