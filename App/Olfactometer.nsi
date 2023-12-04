!define MUI_VERBOSE 1
!include "FileFunc.nsh"

!define CompanyName "Champalimaud Foundation"
!define AppName "Olfactometer"
!define AppNameNoSpaces "Olfactometer"
!define AppVersion "${VERSION_MAJOR}.${VERSION_MINOR}.${VERSION_BUILD}"
!define /date YEAR "%Y"
!define OUT_FILE_NAME "${AppNameNoSpaces}.v${VERSION_MAJOR}.${VERSION_MINOR}.${VERSION_BUILD}-win-x64"

Unicode true
Name "${AppName} v${AppVersion}"
Icon "Assets\cf-logo.n1.ico"

;--------------------------------
;Include Modern UI
  !include "MUI2.nsh"
;--------------------------------

OutFile ".\bin\Release\net6.0\win-x64\${OUT_FILE_NAME}.exe"
InstallDir "$LOCALAPPDATA\${CompanyName}\${AppName}"
RequestExecutionLevel user

;--------------------------------
;Version information
    VIProductVersion "${AppVersion}.0"
    VIAddVersionKey "ProductName" "${AppName}"
    VIAddVersionKey "CompanyName" "${CompanyName}"
    VIAddVersionKey "FileDescription" "${AppName}"
    VIAddVersionKey "FileVersion" "${AppVersion}"
    VIAddVersionKey "ProductVersion" "${AppVersion}"
    VIAddVersionKey "LegalCopyright" "© ${YEAR} ${CompanyName}"
    VIAddVersionKey "LegalTrademarks" "${CompanyName}"
    VIAddVersionKey "OriginalFilename" "${OUT_FILE_NAME}"

;--------------------------------
;Variables
  Var StartMenuFolder
;--------------------------------
;Interface Settings
  !define MUI_ABORTWARNING
  !define MUI_HEADERIMAGE
  !define MUI_HEADERIMAGE_BITMAP "Assets\cf-logo-small.bmp"
  !define MUI_HEADERIMAGE_RIGHT
;--------------------------------
;Pages

  !insertmacro MUI_PAGE_WELCOME
  ;!insertmacro MUI_PAGE_LICENSE "${NSISDIR}/Contrib/Modern UI 2/Pages/License.nsh"
  !insertmacro MUI_PAGE_COMPONENTS
  ;!insertmacro MUI_PAGE_DIRECTORY
  
  ;Start Menu Folder Page Configuration
  !define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKCU" 
  !define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\${CompanyName}\${AppName}" 
  !define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"

  !define MUI_STARTMENUPAGE_DEFAULTFOLDER "${CompanyName}\${AppName}"
  
  !insertmacro MUI_PAGE_STARTMENU Application $StartMenuFolder
  
  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------
;Languages
  !insertmacro MUI_LANGUAGE "English"
;--------------------------------

;Installer Sections
Section "${AppName} v${AppVersion}" FirstSection
  !define ARP "Software\Microsoft\Windows\CurrentVersion\Uninstall\${AppName}"
  SetOutPath "$INSTDIR"
  
  ; Include everything from the output folder
  File /r `.\bin\Release\net6.0\win-x64\publish\*.*`

  ; Get estimated size installed
  ${GetSize} "$INSTDIR" "/S=0K" $0 $1 $2
  IntFmt $0 "0x%08X" $0
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "EstimatedSize" "$0"

  # Registry information for add/remove programs
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "DisplayName" "${CompanyName} - ${AppName}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "UninstallString" "$\"$INSTDIR\uninstall.exe$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "QuietUninstallString" "$\"$INSTDIR\uninstall.exe$\" /S"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "InstallLocation" "$\"$INSTDIR$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "DisplayIcon" "$\"$INSTDIR\cf-logo.ico$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "Publisher" "${CompanyName}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "DisplayVersion" "${AppVersion}"
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "VersionMajor" ${VERSION_MAJOR}
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "VersionMinor" ${VERSION_MINOR}
	# There is no option for modifying or repairing the install
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "NoModify" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "NoRepair" 1
  
  ;Store installation folder
  WriteRegStr HKCU "Software\${CompanyName}\${AppName}" "" $INSTDIR
  
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"
  
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    
    ;Create shortcuts
    CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
    CreateShortcut "$SMPROGRAMS\$StartMenuFolder\${AppName}.lnk" "$INSTDIR\${AppName}.exe" "" "$INSTDIR\${AppName}.exe" 0
    CreateShortcut "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk" "$INSTDIR\Uninstall.exe" "" "$INSTDIR\Uninstall.exe" 0
  
  !insertmacro MUI_STARTMENU_WRITE_END

SectionEnd

;--------------------------------
;Uninstaller Section

Section "Uninstall"

  Delete "$INSTDIR\Uninstall.exe"

  Delete "$INSTDIR\*.*"

  RMDir /r "$INSTDIR"
  
  !insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuFolder

  Delete "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk"
  Delete "$SMPROGRAMS\$StartMenuFolder\${AppName}.lnk"
  RMDir "$SMPROGRAMS\$StartMenuFolder"
  
  DeleteRegKey /ifempty HKCU "Software\${CompanyName}\${AppName}"
  DeleteRegKey /ifempty HKCU "Software\${CompanyName}"
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}"
  

SectionEnd

; !insertmacro SECTION_BEGIN
; File /r `./bin/Release/net6.0/win-x64/publish/*.*`
; !insertmacro SECTION_END
