function SetEnable(strCtlName, bEnable)
{
	if (bEnable==true)
	{
		document.all[strCtlName].disabled=false;
		//document.all[strCtlName+'_out'].disabled=false;
		//document.all[strCtlName+'_div'].disabled=false;
	}
	else
	{
		document.all[strCtlName].disabled=true;
		//document.all[strCtlName+'_out'].disabled=true;
		//document.all[strCtlName+'_div'].disabled=true;
	}
}

function SetBtnEnable(strCtlName, bEnable)
{
    if (document.getElementById(strCtlName+'_Auth').value == 'F')
            return;
	if (bEnable==true)
	{
		document.all[strCtlName].disabled=false;
		//document.all[strCtlName+'_out'].disabled=false;
		//document.all[strCtlName+'_div'].disabled=false;
	}
	else
	{
		document.all[strCtlName].disabled=true;
		//document.all[strCtlName+'_out'].disabled=true;
		//document.all[strCtlName+'_div'].disabled=true;
	}
}    

function SetText(strCtlName, strContext)
{
	document.all[strCtlName+'_div'].innerText=strContext;
}

function SetVisible(strCtlName, bEnable)
{
	if (bEnable==true)
	{
		document.all[strCtlName+'_out'].style.visibility="visible";
	}
	else
	{
		document.all[strCtlName+'_out'].style.visibility="hidden";
	}
}
