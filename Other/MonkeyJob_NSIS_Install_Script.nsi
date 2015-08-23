; Script generated with the Venis Install Wizard

; Define your application name
!define APPNAME "MonkeyJob Tool"
!define APP_EXE "monkeyjobtool.exe"
!define PRODUCT_VERSION "6.0b"
!define APPNAMEANDVERSION "MonkeyJob Tool ${PRODUCT_VERSION}"
; Main Install settings
Name "${APPNAMEANDVERSION}"
InstallDir "$PROGRAMFILES\MonkeyJob Tool"
InstallDirRegKey HKLM "Software\${APPNAME}" ""
OutFile "MonkeyJobTool_${PRODUCT_VERSION}_Installer.exe"
InstType "un.Full remove ${APPNAME}"

; Use compression
SetCompressor Zlib

; Modern interface settings
!include "MUI.nsh"
!include "nsProcess.nsh"
!include "DotNetChecker.nsh"

!define MUI_ABORTWARNING

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

; Set languages (first is default language)
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_RESERVEFILE_LANGDLL



Section "MonkeyJob Tool" Section1

!insertmacro CheckNetFramework 35
${nsProcess::FindProcess} "${APP_EXE}" $R0

${If} $R0 == 0
    DetailPrint "${AppName} is running. Closing it down"
    ${nsProcess::CloseProcess} "${APP_EXE}" $R0
    DetailPrint "Waiting for ${AppName} to close"
    Sleep 2000
${Else}
    DetailPrint "${APP_EXE} was not found to be running"
${EndIf}

${nsProcess::Unload}
	; Set Section properties
	SetOverwrite on

	; Set Section Files and Shortcuts
	SetOutPath "$INSTDIR\"

	File "E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\HelloBotCommunication.dll"
	File "E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\HelloBotCore.dll"
	File "E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\HelloBotModuleHelper.dll"
	File "E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\HtmlAgilityPack.dll"
	File "E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\LogentriesCore.dll"
	File "E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\LogentriesNLog.dll"
	File "E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\MonkeyJobTool.exe"
	File "E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\MonkeyJobTool.exe.config"
	File "E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\NCalc.dll"
	File "E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\Newtonsoft.Json.dll"
	File "E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\Nigrimmist.Modules.dll"
	File "E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\NLog.config"
	File "E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\NLog.dll"
	File "E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\NLog.xml"
	File "E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\SharedHelper.dll"
	File "E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\SmartAssHandlerLib.dll"
	File "E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\Yushko.dll"
	SetOutPath "$INSTDIR\ModuleSettings\"

	CreateShortCut "$DESKTOP\MonkeyJob Tool.lnk" "$INSTDIR\MonkeyJobTool.exe"
	CreateDirectory "$SMPROGRAMS\MonkeyJob Tool"
	CreateShortCut "$SMPROGRAMS\MonkeyJob Tool\MonkeyJob Tool.lnk" "$INSTDIR\MonkeyJobTool.exe"
	CreateShortCut "$SMPROGRAMS\MonkeyJob Tool\Uninstall.lnk" "$INSTDIR\uninstall.exe"

SectionEnd

Section -FinishSection

	WriteRegStr HKLM "Software\${APPNAME}" "" "$INSTDIR"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "DisplayName" "${APPNAME}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "UninstallString" "$INSTDIR\uninstall.exe"
	WriteUninstaller "$INSTDIR\uninstall.exe"

SectionEnd

; Modern install component descriptions
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
	!insertmacro MUI_DESCRIPTION_TEXT ${Section1} ""
!insertmacro MUI_FUNCTION_DESCRIPTION_END

;Uninstall section
Section Uninstall secidx01 
SectionIn 1 RO 

	;Remove from registry...
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}"
	DeleteRegKey HKLM "SOFTWARE\${APPNAME}"

	; Delete self
	Delete "$INSTDIR\uninstall.exe"

	; Delete Shortcuts
	Delete "$DESKTOP\MonkeyJob Tool.lnk"
	Delete "$SMPROGRAMS\MonkeyJob Tool\MonkeyJob Tool.lnk"
	Delete "$SMPROGRAMS\MonkeyJob Tool\Uninstall.lnk"

${nsProcess::FindProcess} "${APP_EXE}" $R0

${If} $R0 == 0
    DetailPrint "${AppName} is running. Closing it down"
    ${nsProcess::CloseProcess} "${APP_EXE}" $R0
    DetailPrint "Waiting for ${AppName} to close"
    Sleep 2000
${Else}
    DetailPrint "${APP_EXE} was not found to be running"
${EndIf}

${nsProcess::Unload}

	; Clean up MonkeyJob Tool	
	Delete "$INSTDIR\HelloBotCommunication.dll"
	Delete "$INSTDIR\HelloBotCore.dll"
	Delete "$INSTDIR\HelloBotModuleHelper.dll"
	Delete "$INSTDIR\HtmlAgilityPack.dll"
	Delete "$INSTDIR\LogentriesCore.dll"
	Delete "$INSTDIR\LogentriesNLog.dll"
	Delete "$INSTDIR\MonkeyJobTool.exe"
	Delete "$INSTDIR\MonkeyJobTool.exe.config"
	Delete "$INSTDIR\NCalc.dll"
	Delete "$INSTDIR\Newtonsoft.Json.dll"
	Delete "$INSTDIR\Nigrimmist.Modules.dll"
	Delete "$INSTDIR\NLog.config"
	Delete "$INSTDIR\NLog.dll"
	Delete "$INSTDIR\NLog.xml"
	Delete "$INSTDIR\SharedHelper.dll"
	Delete "$INSTDIR\SmartAssHandlerLib.dll"
	Delete "$INSTDIR\Yushko.dll"


	; Remove remaining directories
	RMDir "$SMPROGRAMS\MonkeyJob Tool"


SectionEnd

Section "Un.Remove settings" 
SectionIn 1 
	SetShellVarContext current 
	RMDir /r "$INSTDIR\"
SectionEnd 

Function .oninstsuccess
Exec "$INSTDIR\${APP_EXE}"
FunctionEnd

Function un.onInit 
SectionSetText ${secidx01} "Remove application" 
FunctionEnd

BrandingText "test ss"

; eof
