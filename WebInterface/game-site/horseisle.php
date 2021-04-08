<?php
  include("config.php");
?>
<html lang="en">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>HORSEISLE - Secret Land of Horses</title>
<link rel="shortcut icon" href="/favicon.ico" type="image/x-icon">
<link rel="icon" href="/favicon.ico" type="image/x-icon">
<!-- Google Analytics -->
<script src="http://www.google-analytics.com/urchin.js" type="text/javascript">
</script>
<script type="text/javascript">
_uacct = "UA-1805076-1";
urchinTracker();
</script>

<script language="VBScript" type="text/vbscript">
<!-- // Visual basic helper required to detect Flash Player ActiveX control version information
Function VBGetSwfVer(i)
  on error resume next
  Dim swControl, swVersion
  swVersion = 0
  
  set swControl = CreateObject("ShockwaveFlash.ShockwaveFlash." + CStr(i))
  if (IsObject(swControl)) then
    swVersion = swControl.GetVariable("$version")
  end if
  VBGetSwfVer = swVersion
End Function
// -->
</script>
</head>
<body bgcolor="#A797A7" MARGINWIDTH=0 MARGINHEIGHT=0 LEFTMARGIN=0 TOPMARGIN=0 onLoad="">
<!--url's used in the movie-->
<!--text used in the movie-->
<CENTER>
<!--
<p align="center"></p>
<p align="left"></p>
<p align="left"><font face="Arial" size="9" color="#000000" letterSpacing="0.000000" kerning="1"><b>FPS</b></font></p>
<p align="center"><font face="Times New Roman" size="18" color="#000000" letterSpacing="0.000000" kerning="1"><b>CONNECTION TO SERVER LOST:</b></font></p><p align="center"></p><p align="center"><font face="Times New Roman" size="18" color="#000000" letterSpacing="0.000000" kerning="1"><b> Either your Internet connection is down, or the <sbr />server is restarting or possibly down. &nbsp;</b></font></p><p align="center"></p><p align="center"><font face="Times New Roman" size="18" color="#000000" letterSpacing="0.000000" kerning="1"><b>Please try again shortly.</b></font></p><p align="center"></p><p align="center"><font face="Times New Roman" size="18" color="#000066" letterSpacing="0.000000" kerning="1"><a href="http://hi1.horseisle.com/" target = "_self"><b>HI1.HORSEISLE.COM</b></a></font></p>
-->
<script type="text/javascript">
/* 
	I dont really use php but I this might be xss-able by sending something like this as your server ip.
		';alert(1);//
	But it was like that before so im gonna assume it probably works
*/
<?php
echo("const server_ip = '".$server_ip."', server_port = ".$server_port.";");
?>


let ConfirmClose = true;

window.addEventListener('beforeunload', event => {
	if(ConfirmClose)
		return event.preventDefault(), '[ Please use QUIT GAME button to exit Horse Isle ]'
})

function allowExit() {
  ConfirmClose = false;
}



// -----------------------------------------------------------------------------
// Globals
// Major version of Flash required
const requiredMajorVersion = 8;
// Minor version of Flash required
const requiredMinorVersion = 0;
// Revision of Flash required
const requiredRevision = 0;
// -----------------------------------------------------------------------------

const isIE  = navigator.appVersion.includes('MSIE'),
	isWin = navigator.appVersion.toLowerCase().includes('win'),
	isOpera = navigator.userAgent.includes('Opera');

// JavaScript helper required to detect Flash Player PlugIn version information
function JSGetSwfVer() {
	// NS/Opera version >= 3 check for Flash plugin in plugin array
	const plugins = navigator.plugins;
	const ua = navigator.userAgent.toLowerCase();

	if (plugins && plugins.length > 0 && (plugins['Shockwave Flash 2.0'] || plugins['Shockwave Flash'])) {
		/* I would fix this stuff up better but i dont have flash so i cant know how the stuff would be... */
		let swVer2 = plugins['Shockwave Flash 2.0'] ? ' 2.0' : '';
		let flashDescription = plugins['Shockwave Flash' + swVer2].description;
		let descArray = flashDescription.split(' ');
		let tempArrayMajor = descArray[2].split('.');
		let versionMajor = tempArrayMajor[0];
		let versionMinor = tempArrayMajor[1];

		if (descArray[3])
			tempArrayMinor = descArray[3].split('r');
		else
			tempArrayMinor = descArray[4].split('r');

		return `${versionMajor}.${versionMinor}.${Math.max(tempArrayMinor[1], 0)}`;
	}
	// MSN/WebTV 2.6 supports Flash 4
	if (ua.includes('webtv/2.6'))
		return 4;
	// WebTV 2.5 supports Flash 3
	if (ua.includes('webtv/2.5'))
		return 3;
	// older WebTV supports Flash 2
	if (ua.includes('webtv'))
		return 2;
	return -1;
}

// If called with reqMajorVer, reqMinorVer, reqRevision returns true if that version or greater is available
function DetectFlashVer(reqMajorVer, reqMinorVer, reqRevision) {
 	let reqVer = parseFloat(reqMajorVer + "." + reqRevision);
	// loop backwards through the versions until we find the newest version	
	for (let i = 25, versionStr, versionArray; i > 0; i--) {	
		if (isIE && isWin && !isOpera) {
			versionStr = VBGetSwfVer(i);
		} else {
			versionStr = JSGetSwfVer(i);		
		}
		if (versionStr === -1) { 
			return false;
		} else if (versionStr) {
			if(isIE && isWin && !isOpera)
				versionArray      = tempString.split(' ')[1].split(',');				
			else
				versionArray      = versionStr.split('.');

			let [versionMajor, versionMinor, versionRevision] = versionArray
			
			let versionNum = parseFloat(versionMajor + "." + versionRevision);
			// is the major.revision >= requested major.revision AND the minor version >= requested minor
			return (versionMajor > reqMajorVer && versionNum >= reqVer) || 
					(versionNum >= reqVer && versionMinor >= reqMinorVer);
		}
	}
	return false
}

if(DetectFlashVer(requiredMajorVersion, requiredMinorVersion, requiredRevision)) {
	/* this should eventually be removed and it should be done in a safer way */
	var oeTags = `
	<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="790" height="500" id="horseisle" name="horseisle" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab">
		<param name="movie" value="horseisle.swf?SERVER=${server_ip}&PORT=${server_port}&USER=&2158322" />
		<param name="loop" value="false" />
		<param name="menu" value="false" />
		<param name="quality" value="high" />
		<param name="scale" value="noscale" />
		<param name="salign" value="t" />
		<param name="bgcolor" value="#ffffff" />
		<embed src="horseisle.swf?SERVER=${server_ip}&PORT=${server_port}&USER=&2158322" loop="false" menu="false" quality="high" scale="noscale" salign="t" bgcolor="#ffffff" width="790" height="500" name="horseisle" align="top" play="true" loop="false" quality="high" allowScriptAccess="sameDomain" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer">
		</embed>
	</object>
	`
	document.write(oeTags);
} else {
	let alternateContent = `This content requires the Macromedia Flash Player.<a href="http://www.macromedia.com/go/getflash/">Get Flash</a>`;
	document.write(alternateContent);  // insert non-flash content
}
// -->
</script>
<noscript><CENTER>
It appears you do not have the required Flash Player Software.<BR>
<B>Horse Isle requires the Adobe Flash Player 9+.</B><BR>
It is a free and easy download - <a href="http://www.macromedia.com/go/getflash/">Get Flash</a><BR>
</noscript>




</body>
</html>
