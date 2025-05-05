function SetEncodedProperty ( strPropName, strValue, strSource )
{
	var strSeperator = "|";
	var strPre	= "";
	var strTail 	= "";

	var i = 0;
	var j = 0;

	if(null == strValue)
	{
		strValue = "";
	}

	i = strSource.indexOf( strPropName + " = ");
	
	if (i < 0)
	{
		strSource += strPropName + " = " + strValue + "|";
	}
	else
	{
		strPre = strSource.substr ( 0, i + strPropName.length + 3 );

		j = strSource.indexOf ( strSeperator, i + strPropName.length + 3 );
		
		strTail = strSource.substr ( j, strSource.length - j );
		
		strSource = strPre + strValue + strTail;
	}		
	
	return strSource;
}	

function SetEncodedProperties ( strPropName, sarValue, strSource )
{
	var i = 0;
	
	for(i = 0;i< sarValue.length; i++)
	{
		strSource = SetEncodedProperty (strPropName + i, sarValue[i], strSource);
	}
	return strSource;
}

function GetEncodedData (strValueName,strSource)
{
	return ( GetEncodedDataCore(strValueName,strSource,'|') );
}

function GetEncodedDataS (strValueName,strSource)
{
	return ( GetEncodedDataCore(strValueName,strSource,'#$') );
}

function GetEncodedDataCore (strValueName,strSource,strSeperator)
{
	var strValue;
	if ( null == strSource )
	{
		strValue = "";
	}

	// the property will remain in the region by Property = value| format
	var i = strSource.indexOf ( strValueName + " = " );
	if ( i < 0 )
	{
		strValue = "";
	}
	else
	{
		j = strSource.indexOf ( strSeperator, i + strValueName.length + 3 );
		strValue = strSource.substr ( i + strValueName.length + 3, j - i - strValueName.length - 3 );
	}
	
	return (strValue);
}

function GetEncodedProperties(strPropName, strSource) {

    var sarValue = new Array();
    var strValue = "";
    var strSource1 = strSource;

    var bChk = true;
    var iIndex = 0;
    while (bChk)
    {
        strValue = "";
        strValue = GetEncodedData(strPropName + iIndex, strSource1);
        if (strValue == "" || strValue == null) {
            bChk = false
        }

        sarValue[iIndex] = strValue.trim();
        iIndex++
    }
    
    return (sarValue);
}

// Comma Remove
function CR(strSrc)
{
	return ( strSrc.replace(/,/g,"") );
}

function Power (dSource,lExp)
{
	var dResult = 1;
    for ( i = 0; i < lExp; i++ )
		dResult *= dSource;
	return ( Number ( dResult ) );
}

/// 0:Rounding, 1:Flooring, 2:Ceiling
function C ( strSource, iDigit, iRoundingType )
{		
	if ( null == strSource || "" == strSource.toString() )
		return "";
	
	strSource = CR ( strSource.toString() );	
	
	bPositive = true;
	if(Number(strSource) < 0)
		bPositive = false;

	//	split and calculate
	var arSource = strSource.split( "." );

	if ( null != arSource[1] && "" != arSource[1] )
	{	
		if(!bPositive)
			strSource = strSource * -1;
			
		var n = 1;
		
		for(var i = 0;i< iDigit; i++)
		{
			n = n * 10;
		}
			
		strSource = CFL_Multiply ( Number(strSource), n ); 				

		if(iRoundingType == 0)
		{
			strSource = Math.round(strSource);											
		}
		else if(iRoundingType == 1)
		{
			strSource = Math.floor(strSource);				
		}
		else if(iRoundingType == 2)
		{
			strSource = Math.ceil(strSource);				
		}				
			strSource = CFL_Divide ( Number(strSource), n ); 					
			
		if(!bPositive)
			strSource = strSource * -1;
	}
	
	arSource = String(strSource).split( "." );
	
	var strInteger = arSource[0];
	
	
	var strFraction = "";
	if( arSource[1]!= null &&  arSource[1] != "")
		strFraction =  arSource[1];				
					
	if ( Number ( strInteger ) == 0 && strInteger.indexOf("-") != 0)
		strInteger = "0";
	
	var bPositive = true;
	if ( strInteger.indexOf ( "-" ) == 0 )
	{
		bPositive = false;
		strInteger = strInteger.substr ( 1 );
	}

	while ( strInteger.indexOf ( "0" ) == 0 && strInteger.length > 1 && strInteger[1] != '.' )
		strInteger = strInteger.substr ( 1 );
	
	if ( !bPositive )
		strInteger = "-" + strInteger;	
			
	if ( strInteger.length == 0 )
		strInteger = "0";
	else if(strInteger == "-")
		strInteger = "-0";

	if ( iDigit > 0 )
	{
		while ( strFraction.length < iDigit )
			strFraction += "0";

		strFraction = "." + strFraction;
	}

	// eliminate - / + notation
	var strHeader = "";
	if ( strInteger.length > 0 )
	{
		if ( "-" == strInteger.charAt ( 0 ) || "+" == strInteger.charAt ( 0 ) )
		{
			strHeader = strInteger.charAt ( 0 );
			strInteger = strInteger.substr ( 1 );
		}
	}	

	// insert Comma
	if ( strInteger.length > 2 )
	{
		var strResult = strInteger.substr ( strInteger.length - 3, 3 );
		strInteger = strInteger.substr ( 0, strInteger.length - 3 );

		while ( strInteger.length > 2 )
		{
			strResult = strInteger.substr ( strInteger.length - 3, 3 ) + "," + strResult;
			strInteger = strInteger.substr ( 0, strInteger.length - 3 );
		}

		if ( strInteger.length > 0 )
			strResult = strInteger + "," + strResult;

		return ( strHeader + strResult + strFraction );
	}
	else
		return ( strHeader + strInteger + strFraction );
}

// Multiply Two Numbers
function Mult( dSource1, dSource2 )
{
	if ( null == dSource1 || "" == dSource1.toString() )
		return 0;
	
	if ( null == dSource2 || "" == dSource2.toString() )
		return 0;
		
	dSource1 = CR(dSource1.toString());
	dSource2 = CR(dSource2.toString());
	
	var arSource1 = dSource1.split(".");
	var arSource2 = dSource2.split(".");
	
	var dInteger1 = arSource1[0];
	var dFraction1 = "0." + arSource1[1];
	
	if( null == arSource1[1] || "" == arSource1[1].toString() || arSource1[1].length < 1)
		dFraction1 = "0";	
		
	if ( dInteger1.indexOf ( "-" ) == 0 )
	{
		dFraction1 = "-"+dFraction1.toString();
	}

	dInteger1 = Number(dInteger1);
	dFraction1 = Number(dFraction1);

	var dInteger2 = arSource2[0];		
	var dFraction2 = "0." + arSource2[1];
	
	if( null == arSource2[1] || "" == arSource2[1].toString() || arSource2[1].length < 1)
		dFraction2 = "0";
			
	if ( dInteger2.indexOf ( "-" ) == 0 )
	{
		dFraction2 = "-"+dFraction2.toString();
	}

	dInteger2 = Number(dInteger2);
	dFraction2 = Number(dFraction2);

	var dResult = dInteger1 * dInteger2 + dInteger1 * dFraction2 + dFraction1* dInteger2 + dFraction1 * dFraction2;
	
	return dResult;
}

// dSource1 * dSource2
function CFL_Multiply( dSource1, dSource2 )
{
	if ( null == dSource1 || "" == dSource1.toString() )
		return 0;
	
	if ( null == dSource2 || "" == dSource2.toString() )
		return 0;
		
	dSource1 = CR(dSource1.toString());
	dSource2 = CR(dSource2.toString());
	
	var arSource1 = dSource1.split(".");
	var arSource2 = dSource2.split(".");
	
	var iDigit1 = 0;
	var iDigit2 = 0;

	if( null == arSource1[1] || "" == arSource1[1].toString() || arSource1[1].length < 1)
	{
		iDigit1 = 0;
		arSource1[1] = "";
	}
	else
		iDigit1 = arSource1[1].length;

	if( null == arSource2[1] || "" == arSource2[1].toString() || arSource2[1].length < 1)
	{
		iDigit2 = 0;
		arSource2[1] = "";
	}
	else
		iDigit2 = arSource2[1].length;

	var dResult = ( Number ( arSource1[0] + arSource1[1] )) * ( Number ( arSource2[0] + arSource2[1] )) / Power ( 10, ( iDigit1 + iDigit2 ));
	
	return dResult;
}

// Subtract dSource2 from dSource1 ( dSource1 - dSource2 )
function CFL_Subtract( dSource1, dSource2 )
{
	if ( null == dSource1 || "" == dSource1.toString() )
		return 0;
	
	if ( null == dSource2 || "" == dSource2.toString() )
		return 0;
		
	dSource1 = CR(dSource1.toString());
	dSource2 = CR(dSource2.toString());
	
	var arSource1 = dSource1.split(".");
	var arSource2 = dSource2.split(".");
	
	var iDigit1 = 0;
	var iDigit2 = 0;

	if( null == arSource1[1] || "" == arSource1[1].toString() || arSource1[1].length < 1)
		iDigit1 = 0;
	else
		iDigit1 = arSource1[1].length;

	if( null == arSource2[1] || "" == arSource2[1].toString() || arSource2[1].length < 1)
		iDigit2 = 0;
	else
		iDigit2 = arSource2[1].length;

	var iDigit = iDigit1;
	if ( iDigit2 > iDigit1 )
		iDigit = iDigit2;

	var dResult = ( CFL_Multiply ( dSource1, Power ( 10, iDigit ) ) - CFL_Multiply ( dSource2, Power ( 10, iDigit ) ) ) / Power ( 10, iDigit );

	return dResult;

}

// dSource1/ dSource2
function CFL_Divide( dSource1, dSource2 )
{
	if ( null == dSource1 || "" == dSource1.toString() )
		return 0;
	
	if ( null == dSource2 || "" == dSource2.toString() )
		return 0;
		
	dSource1 = CR(dSource1.toString());
	dSource2 = CR(dSource2.toString());
	
	var arSource1 = dSource1.split(".");
	var arSource2 = dSource2.split(".");
	
	var iDigit1 = 0;
	var iDigit2 = 0;

	if( null == arSource1[1] || "" == arSource1[1].toString() || arSource1[1].length < 1)
		iDigit1 = 0;
	else
		iDigit1 = arSource1[1].length;

	if( null == arSource2[1] || "" == arSource2[1].toString() || arSource2[1].length < 1)
		iDigit2 = 0;
	else
		iDigit2 = arSource2[1].length;

	var dSource3 = ( CFL_Multiply ( dSource1, Power ( 10, iDigit1 )) / CFL_Multiply ( dSource2, Power ( 10, iDigit2 )));

	if ( null == dSource3 || "" == dSource3.toString() )
		return 0;
		
	dSource3 = CR(dSource3.toString());

	var arSource3 = dSource3.split(".");
	var iDigit3 = 0;

	if( null == arSource3[1] || "" == arSource3[1].toString() || arSource3[1].length < 1)
		iDigit3 = 0;
	else
		iDigit3 = arSource3[1].length;
	
	var dResult = CFL_Multiply ( dSource3, Power ( 10, iDigit3 ) ) / Power ( 10, iDigit1 - iDigit2 + iDigit3 );
	
	return dResult;
}

// Check If Numeric
function NCheck(strSrc)
{
	strSource = CR ( strSrc );
	if ( 'NaN' == Number ( strSource ).toString() )
	{
		alert("Only Numeric!");
		return ( '' );
	}
	else
		return ( strSource );
}

// Check MaxLength
function LCheck(strSrc, iMaxLength)
{
	var strSource = CR(strSrc);
	var iIndex = strSource.indexOf('.')
	if (iIndex > -1)
		strSource = strSource.substr(0, iIndex);
	strSource = strSource.replace(/-/g, '');
	if (strSource.length > iMaxLength)
	{
		alert('Value out of range!');
		return '';
	}
	else
		return strSrc;
}

// Remove / or : from DateTime String
function RemSlash (strSrc)
{
	while ( strSrc.indexOf ( "/" ) > -1 )
		strSrc = strSrc.replace("/","");
	return strSrc.replace(/:/g,"").replace(/ /ig,"");
}

// Add / or : from DateTime String
function AddSlash ( strDateTime, bShortYear, bYear )
{
	if ( !bYear )
	{
		if ( strDateTime.length == 4 )
			return strDateTime.substr ( 0, 2 ) + ':' + strDateTime.substr ( 2, 2 );
		else if ( strDateTime.length == 6 )
			return strDateTime.substr ( 0, 2 ) + ':' + strDateTime.substr ( 2, 2 ) + ':' + strDateTime.substr ( 4, 2 );
		else
			return strDateTime;
	}

	if ( bShortYear )
		strDateTime = '20' + strDateTime;
	
	var strValue = '';
	var iLength = strDateTime.length;

	if ( iLength == 4 )
		strValue = strDateTime;
	else if ( iLength == 6 )
		strValue = strDateTime.substr ( 0, 4 ) + '/' + strDateTime.substr ( 4, 2 );
	else if ( iLength == 8 )
		strValue = strDateTime.substr ( 0, 4 ) + '/' + strDateTime.substr ( 4, 2 ) + '/' + strDateTime.substr ( 6, 2 );
	else if ( iLength == 12 )
		strValue = strDateTime.substr ( 0, 4 ) + '/' + strDateTime.substr ( 4, 2 ) + '/' + strDateTime.substr ( 6, 2 ) + ' ' + strDateTime.substr ( 8, 2 ) + ':' + strDateTime.substr ( 10, 2 );
	else if ( iLength == 14 )
		strValue = strDateTime.substr ( 0, 4 ) + '/' + strDateTime.substr ( 4, 2 ) + '/' + strDateTime.substr ( 6, 2 ) + ' ' + strDateTime.substr ( 8, 2 ) + ':' + strDateTime.substr ( 10, 2 ) + ':' + strDateTime.substr ( 12, 2 );
	else
		strValue = strDateTime;

	if ( bShortYear )
		strValue = strValue.substr ( 2 );

	return strValue;
}

function PopupGateWay ( strAppName, strModule, strClassName, strMethodName, strWhereClause, strConditions )
{
	// Create an instance of the XML HTTP Request object
	var oXMLHTTP = new ActiveXObject('Microsoft.XMLHTTP');
	
	// Prepare the XMLHTTP object for a HTTP POST to our validation ASP page
	var sURL = strAppName + '/Common/GateWay/ClientGW.asmx';
	
	var sRequestInfo = '<?xml version="1.0" encoding="utf-8"?>\n<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">\n	<soap:Body>\n		<PopupGateWay xmlns="http://tempuri.org/">\n			<strModule>' + strModule + '</strModule>\n			<strClassName>' + strClassName + '</strClassName>\n			<strMethodName>' + strMethodName + '</strMethodName>\n			<strWhereClause>' + strWhereClause.replace(/</g, '&lt;').replace(/>/g, '&gt;') + '</strWhereClause>\n			<strConditions>' + strConditions.replace(/</g, '&lt;').replace(/>/g, '&gt;') + '</strConditions>\n		</PopupGateWay>\n	</soap:Body>\n</soap:Envelope>\n';
	
	oXMLHTTP.open( 'POST', sURL, false );

	oXMLHTTP.setRequestHeader('Content-Type','text/xml; charset=utf-8');
	oXMLHTTP.setRequestHeader('SOAPAction','"http://tempuri.org/PopupGateWay"');
	
	// Execute the request
	oXMLHTTP.send(sRequestInfo);

	var XMLDoc = oXMLHTTP.responseXML;
	
	return XMLDoc.getElementsByTagName("anyType");
}

function TreePopupGateWay ( strAppName, strModule, strClassName, strMethodName, strWhereClause, strConditions, bNonLeafSelection )
{
	// Create an instance of the XML HTTP Request object
	var oXMLHTTP = new ActiveXObject('Microsoft.XMLHTTP');
	
	// Prepare the XMLHTTP object for a HTTP POST to our validation ASP page
	var sURL = strAppName + '/Common/GateWay/ClientGW.asmx';
	
	var sRequestInfo = '<?xml version="1.0" encoding="utf-8"?>\n<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">\n	<soap:Body>\n		<TreeGateWay xmlns="http://tempuri.org/">\n			<strModule>' + strModule + '</strModule>\n			<strClassName>' + strClassName + '</strClassName>\n			<strMethodName>' + strMethodName + '</strMethodName>\n			<strWhereClause>' + strWhereClause.replace(/</g, '&lt;').replace(/>/g, '&gt;') + '</strWhereClause>\n			<strConditions>' + strConditions.replace(/</g, '&lt;').replace(/>/g, '&gt;') + '</strConditions>\n			<bNonLeafSelection>' + ( bNonLeafSelection ? 'true' : 'false' ) + '</bNonLeafSelection>\n		</TreeGateWay>\n	</soap:Body>\n</soap:Envelope>\n';
	
	oXMLHTTP.open( 'POST', sURL, false );

	oXMLHTTP.setRequestHeader('Content-Type','text/xml; charset=utf-8');
	oXMLHTTP.setRequestHeader('SOAPAction','"http://tempuri.org/TreeGateWay"');
	
	// Execute the request
	oXMLHTTP.send(sRequestInfo);

	var XMLDoc = oXMLHTTP.responseXML;
	
	return XMLDoc.getElementsByTagName("anyType");
}

function WebService ( strAddress, strFunctionName, sarArgumentNames, sarArgumentValues )
{
	// Create an instance of the XML HTTP Request object
	var oXMLHTTP = new ActiveXObject( "Microsoft.XMLHTTP" );

	// Prepare the XMLHTTP object for a HTTP POST to our validation ASP page
	oXMLHTTP.open("POST", strAddress + "/" + strFunctionName, false);
	oXMLHTTP.setRequestHeader("Content-Type","application/x-www-form-urlencoded");

	// Make FormData
	var strFormData = "";
	if ( sarArgumentNames.length > 0 )
		strFormData += encodeURIComponent(sarArgumentNames[0]) + "=" + encodeURIComponent(sarArgumentValues[0]);
	for ( var i = 1; i < sarArgumentNames.length; i++ )
		strFormData += "&" + encodeURIComponent(sarArgumentNames[i]) + "=" + encodeURIComponent(sarArgumentValues[i]);

	// Execute the request
	oXMLHTTP.send( strFormData );

	var XMLDoc = oXMLHTTP.responseXML;
	var items = XMLDoc.getElementsByTagName("anyType")
	if (items.length < 1)
	{
		var strMsg = oXMLHTTP.responseText;
		if (strMsg.indexOf('\n') > -1)
		{
			strMsg = strMsg.substr(0, strMsg.indexOf('\n'));
			if (strMsg.indexOf(':') > -1)
				strMsg = strMsg.substr (strMsg.indexOf(':')+1, strMsg.length-strMsg.indexOf(':')-1);
		}
		alert ( strMsg );
		return false;
	}
	else if(items[0].nodeTypedValue != "00")
	{
		alert ( items[1].nodeTypedValue );
		return false;
	}

	return items;
}

function GetDataRowsCount ( items )
{
	if ( items.length < 4 )
		return 0;
	else 
		return Number ( items[3].nodeTypedValue );
}

function GetDataColumnsCount ( items )
{
	if ( items.length < 3 )
		return 0;
	else 
		return Number ( items[2].nodeTypedValue );
}

function GetDataFieldValue ( items, Row, Col )
{
	if ( items.length < 2 )
		return "";
	var iColumnCount = GetDataColumnsCount ( items );
	var iRowCount = GetDataRowsCount ( items );
	if ( Col >= iColumnCount || Row >= iRowCount )
		return "";

	return items[4+Row*iColumnCount+Col].nodeTypedValue;
}

function GetComboValues ( items )
{
	if ( items )
	{
		var itemValues = new Array();
		var iRowCnt = GetDataRowsCount( items );
		var iColCnt = GetDataColumnsCount( items );
		for ( var i = 0; i < iRowCnt; i++ )
		{
			var strValue = "";
			for ( var j = 0; j < iColCnt - 1; j++ )
			{
				strValue += GetDataFieldValue ( items, i, j );
				if ( j != iColCnt - 2 )
					strValue += ',';
			}
			itemValues[i] = strValue;
		}
		return itemValues;
	}
	else
		return new Array();
}

function GetComboTexts ( items )
{
	if ( items )
	{
		var itemTexts = new Array();
		var iRowCnt = GetDataRowsCount( items );
		var iColCnt = GetDataColumnsCount( items );
		for ( var i = 0; i < iRowCnt; i++ )
			itemTexts[i] = GetDataFieldValue ( items, i, iColCnt - 1 );
		return itemTexts;
	}
	else
		return new Array();
}

//유비리포트 팝업  함수 추가. 2013.08.06 
function ReportPopup(strjrf, strScale) {
    var url = '';
    url = '/SINT/Report/Report.aspx';
    url = url + '?jrfFileName=' + strjrf;
    url = url + '&scale=' + strScale;

    var specs = '';
    specs = 'Width=' + screen.width;
    specs = specs + ', height=' + screen.height;
    specs = specs + ', fullscreen=yes';

    window.open(url, '', specs);
}


//유비리포트 팝업  함수 추가. 2013.08.14
function ReportPopupByMultiReport(strjrf, strScale, strIsMultiReport, strMultiCount) {
    var url = '';
    url = '/SINT/Report/Report.aspx';
    url = url + '?jrfFileName=' + strjrf;
    url = url + '&scale=' + strScale;
    url = url + '&ismultireport=' + strIsMultiReport;
    url = url + '&multicount=' + strMultiCount;

    var specs = '';
    specs = 'Width=' + screen.width;
    specs = specs + ', height=' + screen.height;
    specs = specs + ', fullscreen=yes';

    window.open(url, '', specs);
}