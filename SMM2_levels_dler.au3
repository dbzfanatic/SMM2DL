#Region ;**** Directives created by AutoIt3Wrapper_GUI ****
#AutoIt3Wrapper_Icon=chaos.ico
#AutoIt3Wrapper_Outfile=MM2_Level_downloader (x86).exe
#AutoIt3Wrapper_Outfile_x64=MM2_Level_downloader.exe
#AutoIt3Wrapper_Compile_Both=y
#AutoIt3Wrapper_Res_Comment=Auto-populate and replace/install levels for MM2 in yuzu!
#AutoIt3Wrapper_Res_Description=Auto-populate and replace/install levels for MM2 in yuzu!
#AutoIt3Wrapper_Res_Fileversion=1.2.1.0
#AutoIt3Wrapper_Res_ProductName=Super Mario Maker 2 Level Downloader
#AutoIt3Wrapper_Res_ProductVersion=1.2.1
#AutoIt3Wrapper_Res_Language=1033
#AutoIt3Wrapper_Run_Before=attrib "%LocalAppData%\AutoIt v3\Aut2exe\*" -R /S /D
#AutoIt3Wrapper_Run_Tidy=y
#AutoIt3Wrapper_Run_Au3Stripper=y
#Au3Stripper_Parameters=/so
#EndRegion ;**** Directives created by AutoIt3Wrapper_GUI ****
#include <GUIConstantsEx.au3>
#include <GUIListView.au3>
#include <GUIImageList.au3>
#include <GUIStatusBar.au3>
#include <WinAPI.au3>
#include <GDIPlus.au3>
#include <IE.au3>
#include <Misc.au3>
#include <array.au3>
#include <EditConstants.au3>
#include <ListViewConstants.au3>
#include <WindowsConstants.au3>
#include <GuiMenu.au3>
#Region ### START Koda GUI section ### Form=
$frmMain = GUICreate("Mario Maker 2 Level Downloader - dbzfanatic", 770, 605, 190, 125)
$lstMarioLevels = _GUICtrlListView_Create($frmMain, "Loading", 0, 0, 770, 425, -1, BitOR($LVS_EX_FULLROWSELECT, $LVS_EX_SUBITEMIMAGES, $LVS_EX_GRIDLINES, $LVS_EX_CHECKBOXES, $LVS_EX_DOUBLEBUFFER))
;~ _GUICtrlListView_SetView( $lstMarioLevels, 3 ) ; 1 = Large icon <<<<<<<<
_GUICtrlListView_AddItem($lstMarioLevels, "Loading Data...")
_GUICtrlListView_AddSubItem($lstMarioLevels, 1, "", 1)
_GUICtrlListView_SetColumnWidth($lstMarioLevels, 0, $LVSCW_AUTOSIZE)
$stsStatus = _GUICtrlStatusBar_Create($frmMain)
_GUICtrlStatusBar_SetText($stsStatus, "Ready")
$cDummy = GUICtrlCreateDummy()
$txtLLevel = GUICtrlCreateInput("", 120, 450, 75, 20, BitOR(-1, $ES_NUMBER))
$Label1 = GUICtrlCreateLabel("Last level to load:", 10, 450, 110, 20)
GUICtrlSetFont(-1, 10, 450, 0, "MS Sans Serif")
$txtFolder = GUICtrlCreateInput(@AppDataDir & "\yuzu\nand\user\save\0000000000000000\D89D632B98FD70C3A30916A6BF4138F6\01009B90006DC000", 105, 480, 655, 20)
$Label2 = GUICtrlCreateLabel("Save Location:", 10, 480, 95, 20)
GUICtrlSetFont(-1, 10, 450, 0, "MS Sans Serif")

$mnuMarioLevels = GUICtrlCreateContextMenu($lstMarioLevels)
$mnuExtract = GUICtrlCreateMenuItem("Extract", $mnuMarioLevels)
GUICtrlCreateMenuItem("", $mnuMarioLevels)

;~ GUISetState(@SW_SHOW)
#EndRegion ### END Koda GUI section ###

Dim $iLasttoLoad = 706, $bFastLoad = False, $sText, $sTOD[$iLasttoLoad + 1], $sCreator[$iLasttoLoad + 1], $sRD[$iLasttoLoad + 1], $sWorld[$iLasttoLoad + 1], $sGT[$iLasttoLoad + 1], $sName[$iLasttoLoad + 1], $idExtract = 1000
FileInstall("C:\Users\dbzfanatic\AppData\Roaming\MM2\Thumbs\02.bmp", @AppDataDir & "\MM2\Thumbs\02.bmp")


If FileExists(@AppDataDir & "\MM2\settings.ini") Then
	$sSaveFolder = IniRead(@AppDataDir & "\MM2\settings.ini", "UserPrefs", "SaveFolder", @AppDataDir & "\yuzu\nand\user\save\0000000000000000\D89D632B98FD70C3A30916A6BF4138F6\01009B90006DC000")
	$iLasttoLoad = IniRead(@AppDataDir & "\MM2\settings.ini", "UserPrefs", "LvlLoad", $iLasttoLoad)
	GUICtrlSetData($txtFolder, $sSaveFolder)
	GUICtrlSetData($txtLLevel, $iLasttoLoad)
EndIf
GUISetState(@SW_SHOW)


If Not FileExists(@AppDataDir & "\MM2\Thumbs") Then
	DirCreate(@AppDataDir & "\MM2\Thumbs")
EndIf
If Not FileExists(@AppDataDir & "\MM2\Levels") Then
	DirCreate(@AppDataDir & "\MM2\Levels")
EndIf

;~ ShellExecute("explorer.exe",@AppDataDir & "\MM2\Thumbs")

$hImage = _GUIImageList_Create(64, 36)

;~ if not FileExists(@AppDataDir & "\MM2\Thumbs\" & $iLasttoLoad & ".bmp") and $bFastLoad = False Then
For $i = 0 To $iLasttoLoad
	_GUICtrlStatusBar_SetText($stsStatus, "Loading image: " & $i)
	If $i < 10 Then
		If Not FileExists(@AppDataDir & "\MM2\Thumbs\0" & $i & ".bmp") Then
			$iGet = InetRead("https://tinfoil.io/MarioMaker/Thumb/0" & $i)
			If $iGet <> "" Then
;~ 					Do
;~ 						Sleep(10)
;~ 					Until FileExists(@AppDataDir & "\MM2\Thumbs\0" & $i & ".bmp")
;~ 					ConsoleWrite("Binary pic data: " & $iGet & @CRLF)
				_GDIPlus_Startup()
				$bmp = _GDIPlus_BitmapCreateFromMemory($iGet)
;~ 					$bmp = _GDIPlus_BitmapCreateFromHBITMAP($hHBitmapL)
				$bmpres = _GDIPlus_ImageResize($bmp, 64, 36)
;~ 					ConsoleWrite("Create error: " & @error & " Image: " & $i & @CRLF)
				_GDIPlus_ImageSaveToFile($bmpres, @AppDataDir & "\MM2\Thumbs\0" & $i & ".bmp")
;~ 					ConsoleWrite("Save error: " & @error & " Image: " & $i & @CRLF)
				_GDIPlus_BitmapDispose($bmpres)
;~ 					_WinAPI_DeleteObject($hHBitmapL)
				_GDIPlus_Shutdown()
				_GUIImageList_AddBitmap($hImage, @AppDataDir & "\MM2\Thumbs\0" & $i & ".bmp")
			Else
				_GUIImageList_Add($hImage, _GUICtrlListView_CreateSolidBitMap($lstMarioLevels, 0xFF0000, 64, 36))
			EndIf
		Else
			_GUIImageList_AddBitmap($hImage, @AppDataDir & "\MM2\Thumbs\0" & $i & ".bmp")
		EndIf
	Else
		If Not FileExists(@AppDataDir & "\MM2\Thumbs\" & $i & ".bmp") Then
			$iGet = InetRead("https://tinfoil.io/MarioMaker/Thumb/" & $i)
			If $iGet <> "" Then
;~ 					ConsoleWrite("Binary pic data: " & $iGet & @CRLF)
				_GDIPlus_Startup()
				$bmp = _GDIPlus_BitmapCreateFromMemory($iGet)
;~ 					$bmp = _GDIPlus_BitmapCreateFromHBITMAP($hHBitmapL)
				$bmpres = _GDIPlus_ImageResize($bmp, 64, 36)
;~ 					ConsoleWrite("Create error: " & @error & " Image: " & $i & @CRLF)
				_GDIPlus_ImageSaveToFile($bmpres, @AppDataDir & "\MM2\Thumbs\" & $i & ".bmp")
;~ 					ConsoleWrite("Save error: " & @error & " Image: " & $i & @CRLF)
				_GDIPlus_BitmapDispose($bmpres)
;~ 					_WinAPI_DeleteObject($hHBitmapL)
				_GDIPlus_Shutdown()
				_GUIImageList_AddBitmap($hImage, @AppDataDir & "\MM2\Thumbs\" & $i & ".bmp")
			Else
				_GUIImageList_Add($hImage, _GUICtrlListView_CreateSolidBitMap($lstMarioLevels, 0xFF0000, 64, 36))
			EndIf
		Else
			_GUIImageList_AddBitmap($hImage, @AppDataDir & "\MM2\Thumbs\" & $i & ".bmp")
		EndIf
	EndIf

Next
;~ EndIf

;~ _GUIImageList_Swap($hImage,0,1)
;~ _GUIImageList_Swap($hImage,1,2)
_GUICtrlListView_SetImageList($lstMarioLevels, $hImage, 1)
_GUICtrlListView_SetColumnWidth($lstMarioLevels, 0, $LVSCW_AUTOSIZE)

$oIE = _IECreate("about:blank", 1, 0, 1, 0)
;~ For $i = 1 To $iLasttoLoad

;~ Next

_GUICtrlListView_DeleteItem($lstMarioLevels, 0)
_GUICtrlListView_DeleteColumn($lstMarioLevels, 0)

_GUICtrlListView_InsertColumn($lstMarioLevels, 0, "Name")
_GUICtrlListView_SetColumnWidth($lstMarioLevels, 0, $LVSCW_AUTOSIZE)
_GUICtrlListView_InsertColumn($lstMarioLevels, 1, "ToD")
_GUICtrlListView_SetColumnWidth($lstMarioLevels, 1, $LVSCW_AUTOSIZE)
_GUICtrlListView_InsertColumn($lstMarioLevels, 2, "World")
_GUICtrlListView_SetColumnWidth($lstMarioLevels, 2, $LVSCW_AUTOSIZE)
_GUICtrlListView_InsertColumn($lstMarioLevels, 3, "Release Date")
_GUICtrlListView_SetColumnWidth($lstMarioLevels, 3, $LVSCW_AUTOSIZE)
_GUICtrlListView_InsertColumn($lstMarioLevels, 4, "Creator")
_GUICtrlListView_SetColumnWidth($lstMarioLevels, 4, $LVSCW_AUTOSIZE)
_GUICtrlListView_InsertColumn($lstMarioLevels, 5, "Level Number")
_GUICtrlListView_SetColumnWidth($lstMarioLevels, 5, $LVSCW_AUTOSIZE)
_GUICtrlListView_InsertColumn($lstMarioLevels, 6, "Game Type")
_GUICtrlListView_SetColumnWidth($lstMarioLevels, 6, $LVSCW_AUTOSIZE_USEHEADER)



For $i = 1 To $iLasttoLoad

	_GUICtrlStatusBar_SetText($stsStatus, "Loading info for level: " & $i)
	If $i < 10 Then
		_IENavigate($oIE, "http://tinfoil.io/MarioMaker/View/0" & $i, 1)
	Else
		_IENavigate($oIE, "http://tinfoil.io/MarioMaker/View/" & $i, 1)
	EndIf
;~ 	ConsoleWrite(@CRLF & "Page: " & $i)
	$sText = _IEBodyReadText($oIE)
	If StringInStr($sText, "Time of Day") Then
;~ 		ClipPut($sText)
		$sTOD[$i] = StringRegExp($sText, "(Time of Day)\s*(Day|Night)", 1)[1]
		$sWorld[$i] = StringRegExp($sText, "(World)\s*(\w+)", 1)[1]
		$sRD[$i] = StringRegExp($sText, "(Release Date)\s*(\d{2}-\d{2}-\d{4})", 1)[1]
		$sCreator[$i] = StringRegExp($sText, "(Creator)\s*(\w*\s?\w+)", 1)[1]
		$sNameReg = StringRegExp($sText, "(Log in)\s*(\b((?!=|\,).)+(.)\b)", 1)
		$sGT[$i] = StringRegExp($sText, "(Game\s*)(\w*(\s?\w+)*)", 1)[1]
		If UBound($sNameReg) >= 2 Then
			$sName[$i] = StringRegExp($sText, "(Log in)\s*(\b((?!=|\,).)+(.)\b)", 1)[1]
		Else
			$sName[$i] = "Unsupported Name"
		EndIf
		ConsoleWrite("Reading page: " & $i & @CRLF)
	Else
		$sTOD[$i] = "N/A"
		$sWorld[$i] = "N/A"
		$sRD[$i] = "N/A"
		$sCreator[$i] = "N/A"
		$sName[$i] = "N/A"
		$sGT[$i] = "N/A"
	EndIf
;~ 	MsgBox("","",$sName)

	If Not StringInStr($sName[$i], "N/A") Then
;~ 	ConsoleWrite(@CRLF & "Adding Item: " & $i & $sName[$i] & "|"& $sTOD[$i] & "|"& $sWorld[$i] & "|"& $sRD[$i] & "|"& $sCreator[$i] & "|")
;~ 	GUICtrlCreateListViewItem($sName[$i] & "|"& $sTOD[$i] & "|"& $sWorld[$i] & "|"& $sRD[$i] & "|"& $sCreator[$i],$lstMarioLevels)
		_GUICtrlListView_AddItem($lstMarioLevels, $sName[$i], $i)
		_GUICtrlListView_AddSubItem($lstMarioLevels, _GUICtrlListView_GetItemCount($lstMarioLevels) - 1, $sTOD[$i], 1)
		_GUICtrlListView_AddSubItem($lstMarioLevels, _GUICtrlListView_GetItemCount($lstMarioLevels) - 1, $sWorld[$i], 2)
		_GUICtrlListView_AddSubItem($lstMarioLevels, _GUICtrlListView_GetItemCount($lstMarioLevels) - 1, $sRD[$i], 3)
		_GUICtrlListView_AddSubItem($lstMarioLevels, _GUICtrlListView_GetItemCount($lstMarioLevels) - 1, $sCreator[$i], 4)
		If $i < 10 Then
			_GUICtrlListView_AddSubItem($lstMarioLevels, _GUICtrlListView_GetItemCount($lstMarioLevels) - 1, "0" & $i, 5)
		Else
			_GUICtrlListView_AddSubItem($lstMarioLevels, _GUICtrlListView_GetItemCount($lstMarioLevels) - 1, $i, 5)
		EndIf
		_GUICtrlListView_AddSubItem($lstMarioLevels, _GUICtrlListView_GetItemCount($lstMarioLevels) - 1, $sGT[$i], 6)
		For $y = 0 To 4
			_GUICtrlListView_SetColumnWidth($lstMarioLevels, $y, $LVSCW_AUTOSIZE)
			_GUICtrlListView_SetColumnWidth($lstMarioLevels, $y, $LVSCW_AUTOSIZE_USEHEADER)
;~ 			GUICtrlSendMsg($lstMarioLevels, $LVM_SETCOLUMNWIDTH, $y, -1) ; $LVSCW_AUTOSIZE
;~ 			GUICtrlSendMsg($lstMarioLevels, $LVM_SETCOLUMNWIDTH, $y, -2) ; $LVSCW_AUTOSIZE_USEHEADER
		Next
	EndIf
Next

_GUICtrlStatusBar_SetText($stsStatus, "Loading complete!")

GUIRegisterMsg($WM_NOTIFY, "WM_NOTIFY")
_GUICtrlListView_RegisterSortCallBack($lstMarioLevels, 0)

While 1
	$nMsg = GUIGetMsg()
	If _IsPressed("0D") = True Then
		$CurrCtrl = ControlGetHandle($frmMain, "", ControlGetFocus($frmMain))
		If $CurrCtrl = ControlGetHandle($frmMain, "", $txtLLevel) And WinActive($frmMain) Then
			IniWrite(@AppDataDir & "\MM2\settings.ini", "UserPrefs", "LvlLoad", GUICtrlRead($txtLLevel))
		ElseIf $CurrCtrl = ControlGetHandle($frmMain, "", $txtFolder) Then
			IniWrite(@AppDataDir & "\MM2\settings.ini", "UserPrefs", "SaveFolder", GUICtrlRead($txtFolder))
		EndIf
	EndIf
	Switch $nMsg
		Case $GUI_EVENT_CLOSE
			_IEQuit($oIE)
			_GUICtrlListView_UnRegisterSortCallBack($lstMarioLevels)
			Exit
		Case $LVN_ITEMACTIVATE
;~             MsgBox(0, "TEST", "Doubleclicked")
			; Add the dummy to the button case <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		Case $cDummy
			$sLvlNum = StringSplit(_GUICtrlListView_GetItemTextString($lstMarioLevels), "|")[6]
;~             MsgBox(0, "dummy fire", $sLvlNum)
			If Not FileExists(@AppDataDir & "\MM2\Levels\" & $sLvlNum & ".zip") Then
				ConsoleWrite("http://tinfoil.io/MarioMaker/Download/" & $sLvlNum)
				InetGet("http://tinfoil.io/MarioMaker/Download/" & $sLvlNum, @AppDataDir & "\MM2\Levels\" & $sLvlNum & ".zip")
				_GUICtrlStatusBar_SetText($stsStatus, "Download complete!")
			Else
				_ExtractZip(@AppDataDir & "\MM2\Levels\" & $sLvlNum & ".zip", GUICtrlRead($txtFolder))
				_GUICtrlStatusBar_SetText($stsStatus, "Extraction complete!")
			EndIf
;~ 		Case $mnuExtract
;~ 			extractEvent()
		Case $lstMarioLevels
			_GUICtrlListView_SortItems($lstMarioLevels, GUICtrlGetState($lstMarioLevels))
	EndSwitch
WEnd

Func WM_NOTIFY($hWnd, $iMsg, $iwParam, $ilParam)
	Local $hWndFrom, $iIDFrom, $iCode, $tNMHDR, $hWndListView, $tInfo
	$hWndListView = $lstMarioLevels
	If Not IsHWnd($lstMarioLevels) Then $hWndListView = GUICtrlGetHandle($lstMarioLevels)

	$tNMHDR = DllStructCreate($tagNMHDR, $ilParam)
	$hWndFrom = HWnd(DllStructGetData($tNMHDR, "hWndFrom"))
	;$iIDFrom = DllStructGetData($tNMHDR, "IDFrom")
	$iCode = DllStructGetData($tNMHDR, "Code")
	Switch $hWndFrom
		Case $hWndListView
			Switch $iCode
				Case $NM_RCLICK ; Sent by a list-view control when the user clicks an item with the left mouse button
					ListView_RClick()
					Return 0
				Case $NM_DBLCLK
					GUICtrlSendToDummy($cDummy)
				Case $LVN_COLUMNCLICK ; A column was clicked
					Local $tInfo = DllStructCreate($tagNMLISTVIEW, $ilParam)

					; Kick off the sort callback
					_GUICtrlListView_SortItems($hWndFrom, DllStructGetData($tInfo, "SubItem"))
					; No return value
			EndSwitch
	EndSwitch
	Return $GUI_RUNDEFMSG
EndFunc   ;==>WM_NOTIFY

Func _ExtractZip($sZipFile, $sDestinationFolder, $sFolderStructure = "")
	If Not FileExists($sDestinationFolder) Then
		DirCreate($sDestinationFolder)
	EndIf
	Local $i
	Do
		$i += 1
		$sTempZipFolder = @TempDir & "\Temporary Directory " & $i & " for " & StringRegExpReplace($sZipFile, ".*\\", "")
	Until Not FileExists($sTempZipFolder) ; this folder will be created during extraction

	Local $oShell = ObjCreate("Shell.Application")

	If Not IsObj($oShell) Then
		Return SetError(1, 0, 0) ; highly unlikely but could happen
	EndIf

	Local $oDestinationFolder = $oShell.NameSpace($sDestinationFolder)
	If Not IsObj($oDestinationFolder) Then
		DirCreate($sDestinationFolder)
;~         Return SetError(2, 0, 0) ; unavailable destionation location
	EndIf

	Local $oOriginFolder = $oShell.NameSpace($sZipFile & "\" & $sFolderStructure) ; FolderStructure is overstatement because of the available depth
	If Not IsObj($oOriginFolder) Then
		Return SetError(3, 0, 0) ; unavailable location
	EndIf

	Local $oOriginFile = $oOriginFolder.Items() ;get all items
	If Not IsObj($oOriginFile) Then
		Return SetError(4, 0, 0) ; no such file in ZIP file
	EndIf

	; copy content of origin to destination
	$oDestinationFolder.CopyHere($oOriginFile, 20) ; 20 means 4 and 16, replaces files if asked

	DirRemove($sTempZipFolder, 1) ; clean temp dir

	Return 1 ; All OK!

EndFunc   ;==>_ExtractZip

Func ListView_RClick()
	Local $aHit

	$aHit = _GUICtrlListView_SubItemHitTest($lstMarioLevels)
	If ($aHit[0] <> -1) Then
		; Create a standard popup menu
		; -------------------- To Do --------------------
		$hMenu = _GUICtrlMenu_CreatePopup()
		_GUICtrlMenu_AddMenuItem($hMenu, "Extract", $idExtract)
		; ========================================================================
		; Shows how to capture the context menu selections
		; ========================================================================
		Switch _GUICtrlMenu_TrackPopupMenu($hMenu, $lstMarioLevels, -1, -1, 1, 1, 2)
			Case $idExtract
				extractEvent()
		EndSwitch
		_GUICtrlMenu_DestroyMenu($hMenu)
	EndIf
EndFunc   ;==>ListView_RClick

Func extractEvent()
	$sLvlNum = StringSplit(_GUICtrlListView_GetItemTextString($lstMarioLevels), "|")[6]
	$iLvlRep = InputBox("Enter Level Number", "Please enter the level number you are replacing", "000")
	If Not FileExists(@AppDataDir & "\MM2\Levels\" & $sLvlNum & ".zip") Then
		ConsoleWrite("http://tinfoil.io/MarioMaker/Download/" & $sLvlNum)
		InetGet("http://tinfoil.io/MarioMaker/Download/" & $sLvlNum, @AppDataDir & "\MM2\Levels\" & $sLvlNum & ".zip")
		_GUICtrlStatusBar_SetText($stsStatus, "Download complete!")
	EndIf
	FileCopy(GUICtrlRead($txtFolder) & "\course_data_000.bcd", GUICtrlRead($txtFolder) & "\temp\", 8)
	FileCopy(GUICtrlRead($txtFolder) & "\course_replay_000.dat", GUICtrlRead($txtFolder) & "\temp\", 8)
	FileCopy(GUICtrlRead($txtFolder) & "\course_thumb_000.btl", GUICtrlRead($txtFolder) & "\temp\", 8)
	_ExtractZip(@AppDataDir & "\MM2\Levels\" & $sLvlNum & ".zip", GUICtrlRead($txtFolder))
	FileMove(GUICtrlRead($txtFolder) & "\course_data_000.bcd", GUICtrlRead($txtFolder) & "\course_data_" & $iLvlRep & ".bcd")
	FileMove(GUICtrlRead($txtFolder) & "\course_replay_000.dat", GUICtrlRead($txtFolder) & "\course_replay_" & $iLvlRep & ".dat")
	FileMove(GUICtrlRead($txtFolder) & "\course_thumb_000.btl", GUICtrlRead($txtFolder) & "\course_thumb_" & $iLvlRep & ".btl")
	FileMove(GUICtrlRead($txtFolder) & "\temp\course_data_000.bcd", GUICtrlRead($txtFolder), 8)
	FileMove(GUICtrlRead($txtFolder) & "\temp\course_replay_000.dat", GUICtrlRead($txtFolder), 8)
	FileMove(GUICtrlRead($txtFolder) & "\temp\course_thumb_000.btl", GUICtrlRead($txtFolder), 8)
	_GUICtrlStatusBar_SetText($stsStatus, "Extraction complete!")
	DirRemove(GUICtrlRead($txtFolder) & "\temp\")
EndFunc   ;==>extractEvent
