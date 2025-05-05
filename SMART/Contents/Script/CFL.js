/********************************************************************************/
/* 생성자 : 유태한
 * 생성일자 : 2021.03-05
 * 유틸정보가 존재하는 스크립트 필요할 경우 추가하여 사용                           */
/********************************************************************************/

function CR(strSrc) {
	if (strSrc == null) {
		strSrc = "";
    }
	return (strSrc.replace(/,/g, ""));
}


/// 0:Rounding, 1:Flooring, 2:Ceiling
function C(strSource, iDigit, iRoundingType) {
	if (null == strSource || "" == strSource.toString())
		return "";

	strSource = CR(strSource.toString());

	bPositive = true;
	if (Number(strSource) < 0)
		bPositive = false;

	//	split and calculate
	var arSource = strSource.split(".");

	if (null != arSource[1] && "" != arSource[1]) {
		if (!bPositive)
			strSource = strSource * -1;

		var n = 1;

		for (var i = 0; i < iDigit; i++) {
			n = n * 10;
		}

		strSource = CFL_Multiply(Number(strSource), n);

		if (iRoundingType == 0) {
			strSource = Math.round(strSource);
		}
		else if (iRoundingType == 1) {
			strSource = Math.floor(strSource);
		}
		else if (iRoundingType == 2) {
			strSource = Math.ceil(strSource);
		}
		strSource = CFL_Divide(Number(strSource), n);

		if (!bPositive)
			strSource = strSource * -1;
	}

	arSource = String(strSource).split(".");

	var strInteger = arSource[0];


	var strFraction = "";
	if (arSource[1] != null && arSource[1] != "")
		strFraction = arSource[1];

	if (Number(strInteger) == 0 && strInteger.indexOf("-") != 0)
		strInteger = "0";

	var bPositive = true;
	if (strInteger.indexOf("-") == 0) {
		bPositive = false;
		strInteger = strInteger.substr(1);
	}

	while (strInteger.indexOf("0") == 0 && strInteger.length > 1 && strInteger[1] != '.')
		strInteger = strInteger.substr(1);

	if (!bPositive)
		strInteger = "-" + strInteger;

	if (strInteger.length == 0)
		strInteger = "0";
	else if (strInteger == "-")
		strInteger = "-0";

	if (iDigit > 0) {
		while (strFraction.length < iDigit)
			strFraction += "0";

		strFraction = "." + strFraction;
	}

	// eliminate - / + notation
	var strHeader = "";
	if (strInteger.length > 0) {
		if ("-" == strInteger.charAt(0) || "+" == strInteger.charAt(0)) {
			strHeader = strInteger.charAt(0);
			strInteger = strInteger.substr(1);
		}
	}

	// insert Comma
	if (strInteger.length > 2) {
		var strResult = strInteger.substr(strInteger.length - 3, 3);
		strInteger = strInteger.substr(0, strInteger.length - 3);

		while (strInteger.length > 2) {
			strResult = strInteger.substr(strInteger.length - 3, 3) + "," + strResult;
			strInteger = strInteger.substr(0, strInteger.length - 3);
		}

		if (strInteger.length > 0)
			strResult = strInteger + "," + strResult;

		return (strHeader + strResult + strFraction);
	}
	else
		return (strHeader + strInteger + strFraction);
}

// dSource1 * dSource2
function CFL_Multiply(dSource1, dSource2) {
    if (null == dSource1 || "" == dSource1.toString())
        return 0;

    if (null == dSource2 || "" == dSource2.toString())
        return 0;

    dSource1 = CR(dSource1.toString());
    dSource2 = CR(dSource2.toString());

    var arSource1 = dSource1.split(".");
    var arSource2 = dSource2.split(".");

    var iDigit1 = 0;
    var iDigit2 = 0;

    if (null == arSource1[1] || "" == arSource1[1].toString() || arSource1[1].length < 1) {
        iDigit1 = 0;
        arSource1[1] = "";
    }
    else
        iDigit1 = arSource1[1].length;

    if (null == arSource2[1] || "" == arSource2[1].toString() || arSource2[1].length < 1) {
        iDigit2 = 0;
        arSource2[1] = "";
    }
    else
        iDigit2 = arSource2[1].length;

    var dResult = (Number(arSource1[0] + arSource1[1])) * (Number(arSource2[0] + arSource2[1])) / Power(10, (iDigit1 + iDigit2));

    return dResult;
}