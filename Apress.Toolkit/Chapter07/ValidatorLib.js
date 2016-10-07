<script language="javascript">
<!--
/*****************************************************
/*      .NET Char class equivalent.
/****************************************************/
function Char()
{
}
Char.IsDigit = function (c) { return /^\d$/.test(c); };
Char.IsLetter = function (c) { return /^[a-zA-Z_,\s]$/.test(c); };
Char.IsLetterOrDigit = function (c) { return this.IsDigit(c) || this.IsLetter(c); };
Char.IsLower = function (c) 
{ 
  return c == c.toLowerCase();
};
Char.IsNumber = function (c) 
{ 
    var reg = "^[0-9";
    reg += "\\" + CultureInfo.NumberFormat.NumberDecimalSeparator;
    reg += "\\" + CultureInfo.NumberFormat.NumberGroupSeparator;
    reg += "\\" + CultureInfo.NumberFormat.NegativeSign;
    reg += "\\" + CultureInfo.NumberFormat.PositiveSign;
    reg += "]$";    
    return new RegExp(reg).test(c); 
};
Char.IsUpper = function (c) 
{ 
  return c == c.toUpperCase();
};
Char.IsWhitespace = function (c) { return c = " "; };
/*****************************************************
/*      End of Char object
/****************************************************/


/*****************************************************
/*      .NET String class equivalent (renamed)
/****************************************************/
//Unimplemented: Compare, CompareOrdinal, Copy, Format, Intern, IsInterned
function NetString(value)
{
    this._value = value;
    this.Intern = value;
    this.Length = value.length;
    this.Empty = "";
    this.Whitespace = " ";
}
function Chars(index)
{
    return new NetString(this.Intern.charAt(index));
}
function EndsWith(value)
{
    return this.Intern.substring(
        this.Intern.length - value.length, 
        this.Intern.length) == value;
}
function Equals(value)
{
    return this.Intern == value;
}
function IndexOf(value)
{
    return this.Intern.indexOf(value);
}
function IndexOfAny(anyOf)
{
    var pos;

    for (i = 0; i < this.Intern.length; i++)
    {
        for (j = 0; j < anyOf.length; j++)
        {
            if (this.Intern.charAt(i) == anyOf[j])
                return i;
        }
    }
    return -1;
}
function Insert(startIndex, value)
{
    var start = this.Intern.substring(0, startIndex);
    var end = this.Intern.substring(startIndex, this.Intern.length);
    return new NetString(start + value + end);
}
function Intern()
{
    return this._value;
}
function Join(separator, values)
{
    var ret = "";
    for (i = 0; i < values.length; i++)
        ret += values[i] + separator;
    
    if (values.length != 0) 
        ret = ret.substring(0, ret.length - 1);

    return new NetString(ret);
}
function LastIndexOf(value)
{
    return this.Intern.lastIndexOf(value);
}
function LastIndexOfAny(anyOf)
{
    var pos = -1;
    var tmp;

    for (j = 0; j < anyOf.length; j++)
    {
        tmp = this.LastIndexOf(anyOf[j]);
        if (tmp > pos)
            pos = tmp;
    }

    return pos;
}
function PadLeft(totalWidth, paddingChar)
{
    var ch = (!paddingChar) ? this.Whitespace : paddingChar;
    var dif = totalWidth - this.Length;
    var res = new Array();
    
    for (i = 0; i < dif; i++)
        res.push(ch);
    
    return new NetString(res.join("") + this.Intern);
}
function PadRight(totalWidth, paddingChar)
{
    var ch = (!paddingChar) ? this.Whitespace : paddingChar;
    var dif = totalWidth - this.Intern.length;
    var res = new Array();
    
    for (i = 0; i < dif; i++) 
        res.push(ch);
        
    return new NetString(this.Intern + res.join(""));
}
function Remove(startIndex, count)
{
    var start = this.Intern.substring(0, startIndex);
    var end = this.Intern.substring(startIndex + count, this.Intern.length);
    return new NetString(start + end);
}
function Replace(regexp, newSubStr)
{
    return new NetString(this.Intern.replace(regexp, newSubStr));
}
function Split(separator, limit)
{
    if (!limit) limit = 2147483647; //.NET String limit to Split.
    var arr = this.Intern.split(separator, limit);
    var ret = new Array(arr.length);
    
    for (i = 0; i < arr.length; i++)
        ret[i] = new NetString(arr[i]);
    
    return ret;
}
function Substring(startIndex, length)
{
    if (!length) length = this.Intern.length;
        
    return new NetString(this.Intern.substr(startIndex, length));
}
function ToCharArray(startIndex, length)
{
    if (!startIndex) startIndex = 0;
    if (!length) length = this.Intern.length;
    if (startIndex >= this.Intern.length) return new Array(0);
    if (length > (this.Intern.length - startIndex)) 
        length = this.Intern.length - startIndex;

    var arr = new Array(length);
    
    for (i = 0; i < length; i++)
        arr[i] = new NetString(this.Intern.substring(startIndex + i, startIndex + i + 1));
                
    return arr;
}
function ToLower()
{
    return new NetString(this.Intern.toLowerCase());
}
function ToUpper()
{
    return new NetString(this.Intern.toUpperCase());
}
function ToString()
{
    return this.Intern.toString();
}
function Trim(trimChars)
{
    return this.TrimHelper(trimChars, 0);
}
function TrimStart(trimChars)
{
    return this.TrimHelper(trimChars, 1);
}
function TrimEnd(trimChars)
{
    return this.TrimHelper(trimChars, 2);
}
function TrimHelper(trimChars, type)
{
    if (!trimChars) trimChars = [this.Whitespace];
    
    var expr = "";
    
    for (i =  0; i < trimChars.length; i++)
        expr = expr.concat(trimChars[i]);        
        
    if (expr == this.Empty) return new NetString(this.Intern);
    
    if (type == 0)
    {
        expr = "[" + expr + "]*";
        return new NetString(this.Intern.replace(new RegExp(expr, "g"), this.Empty));
    }
    else
    {
        if (type == 1) expr = "^[" + expr + "]*";
        if (type == 2) expr = "[" + expr + "]*$";
        return new NetString(this.Intern.replace(new RegExp(expr), this.Empty));
    }    
}

/*****************************************************
/*      String passthrough functions.
/* This makes the intrinsic methods available too. 
/****************************************************/
function charAt(index) { return this.Intern.charAt(index); }
function charCodeAt(index) { return this.Intern.charCodeAt(index); }
function concat() 
{ 
    var func = "this.Intern.concat("
    
    for (i = 0; i < arguments.length; i++)
        func += "\"" + arguments[i] + "\",";
    
    func = func.substring(0, func.length - 1) + ");";
    return eval(func); 
}
function indexOf(searchValue, fromIndex) { return this.Intern.indexOf(searchValue, fromIndex); }
function lastIndexOf(searchValue, fromIndex) { return this.Intern.lastIndexOf(searchValue, fromIndex); }
function length() { return this.Intern.length; }
function match(regexp) { return this.Intern.match(regexp); }
function replace(regexp, newSubStr) { return this.Intern.replace(regexp, newSubStr); }
function search(regexp) { return this.Intern.search(regexp); }
function slice(beginslice, endSlice) { return this.Intern.slice(beginslice, endSlice); }
function split(separator, limit) { return this.Intern.split(separator, limit); }
function substr(start, length) { return this.Intern.substr(start, length); }
function substring(indexA, indexB) { return this.Intern.substring(indexA, indexB); }
function toLowerCase() { return this.Intern.toLowerCase(); }
function toSource() { return this.Intern.toSource(); }
function toUpperCase() { return this.Intern.toUpperCase(); }
function valueOf() { return this.Intern.valueOf(); }

/*****************************************************
/*      Prototyping our NetString object
/****************************************************/
//Just copying the String prototype doesn't work :(
//NetString.prototype = new String("");
NetString.prototype.Chars = Chars;
NetString.prototype.EndsWith = EndsWith;
NetString.prototype.Equals = Equals;
NetString.prototype.IndexOf = IndexOf;
NetString.prototype.IndexOfAny = IndexOfAny;
NetString.prototype.Insert = Insert;
NetString.prototype.Join = Join;
NetString.prototype.LastIndexOf = LastIndexOf;
NetString.prototype.LastIndexOfAny = LastIndexOfAny;
NetString.prototype.PadLeft = PadLeft;
NetString.prototype.PadRight = PadRight;
NetString.prototype.Remove = Remove;
NetString.prototype.Replace = Replace;
NetString.prototype.Split = Split;
NetString.prototype.Substring = Substring;
NetString.prototype.ToCharArray = ToCharArray;
NetString.prototype.ToLower = ToLower;
NetString.prototype.ToUpper = ToUpper;
NetString.prototype.Trim = Trim;
NetString.prototype.TrimStart = TrimStart;
NetString.prototype.TrimEnd = TrimEnd;
NetString.prototype.TrimHelper = TrimHelper;
NetString.prototype.ToString = ToString;

//Register String methods passthrough.
NetString.prototype.charAt = charAt;
NetString.prototype.charCodeAt = charCodeAt;
NetString.prototype.concat = concat;
NetString.prototype.indexOf = indexOf;
NetString.prototype.lastIndexOf = lastIndexOf;
NetString.prototype.length = length;
NetString.prototype.match = match;
NetString.prototype.replace = replace;
NetString.prototype.search = search;
NetString.prototype.slice = slice;
NetString.prototype.split = split;
NetString.prototype.substr = substr;
NetString.prototype.substring = substring;
NetString.prototype.toLowerCase = toLowerCase;
NetString.prototype.toSource = toSource;
NetString.prototype.toUpperCase = toUpperCase;
NetString.prototype.valueOf = valueOf;
NetString.prototype.toString = ToString; //Override default implementation.
NetString.fromCharCode = String.fromCharCode; //Add the static member.
/*****************************************************
/*      End of NetString object
/****************************************************/

/*****************************************************
/*   Extend intrinsic js String object to mach .NET
/****************************************************/
//Unimplemented: Compare, CompareOrdinal, Copy, Format, Intern, IsInterned
function Concat()
{
    var s = new String("");
    return s.concat.apply(s, arguments);
}
String.Concat = Concat;
String.Empty = new String("");
String.Equals = function(a, b) { return (a == b); };
String.Join = NetString.prototype.Join;


/*****************************************************
/*      Validator object.
/****************************************************/
function WroxToolkitValidator(controlToValidate, errorMessage, display, 
            isValid, validateKeyPress, validateChange, id, functionName)
{
    this.ControlToValidate = controlToValidate;
    this.ErrorMessage = errorMessage;
    this.Display = display;
    this.IsValid = isValid;
    this.ValidateChange = validateChange;
    this.ValidateKeyPress = validateKeyPress;
    this.Id = id;
    this.FunctionName = functionName;
    this.ValidatorControl = document.getElementById(id);
}
function RefreshState()
{
    //Hide/show label.
    if (this.Display == "None") return;
    if (this.Display == "Dynamic")
        this.ValidatorControl.style.display = this.IsValid ? "none" : "inline";
    if (this.Display == "Static")
        this.ValidatorControl.style.visibility = this.IsValid ? "hidden" : "visible";
}
WroxToolkitValidator.prototype.RefreshState = RefreshState;


/****************************************************/
/*      Global Event handling and initialization    */
/****************************************************/

//Global variable to hold validators
var WroxToolkitValidators = new Array();

//Register a new validator in the page
function AddValidator(controlToValidate, errorMessage,
            display, isValid, validateKeyPress, validateChange,
            id, functionName)
{    
    var arr = null;
    //First locate the array of validator for the target control.
    for (i = 0; i < WroxToolkitValidators.length; i++)
    {
        if (WroxToolkitValidators[i] == controlToValidate)
        {
            arr = WroxToolkitValidators[i].Elements;
            break;
        }
    }
   
    //If we don't find one, create a new array now.
    if (arr == null)
    {
        arr = new Array();
        var t = new String(controlToValidate);
        t.Elements = arr;
        //The key in the array is the control ID itself.
        WroxToolkitValidators.push(t);
    }
    
    var val = new WroxToolkitValidator(controlToValidate, 
            errorMessage, display, isValid, validateKeyPress, 
            validateChange, id, functionName);

    //Add the new validator to the array of validators for the 
    //target control to validate.    
    arr.push(val);
}

//If any one validator returns false, the whole event is canceled.
function ValidateOnChange(e)
{
    var sender = PrepareSender(e);
    var argument = PrepareChangeArgument(e);    
    var validators = new Array();
    var cancel = false;
    
    debugger;
    //Retrieve the list of validators for the current control.    
    for (i = 0; i < WroxToolkitValidators.length; i++)
    {
        if (WroxToolkitValidators[i] == sender.id)
        {
            validators = WroxToolkitValidators[i].Elements;
            break;
        }
    }
    
    //Call the validation method on each validator. 
    for (i = 0; i < validators.length; i++)
    {
        var validator = validators[i];
        //Execute the appropriate function.
        if (validator.ValidateChange)
        {
            //Reset Cancel flag.
            argument.Cancel = false;
            eval("validator.Change" + validator.FunctionName + "(sender, argument);");
            
            //If the event is canceled, the whole function will be canceled. 
            if (argument.Cancel) cancel = true;
            validator.RefreshState();
        }
    }    
    
    return !cancel;
}

//If any one validator returns false, the whole event is canceled.
function ValidateOnKeyPress(e)
{
    var sender = PrepareSender(e);
    var argument = PrepareKeyPressArgument(e);    
    var validators = new Array();
    var cancel = false;
    
    //Retrieve the list of validators for the current control.    
    for (i = 0; i < WroxToolkitValidators.length; i++)
    {
        if (WroxToolkitValidators[i] == sender.id)
        {
            validators = WroxToolkitValidators[i].Elements;
            break;
        }
    }
    
    //Call the validation method on each validator. 
    for (i = 0; i < validators.length; i++)
    {
        var validator = validators[i];
        //Execute the appropriate function.
        if (validator.ValidateKeyPress)
        {
            //Reset Handled flag.
            argument.Handled = false;
            //Call: validator.KeyPress[key](sender, argument);
            eval("validator.KeyPress" + validator.FunctionName + "(sender, argument);");
            
            //If the event is handled, it has to be canceled. 
            if (argument.Handled) cancel = true;
            validator.RefreshState();
        }
    }    
    
    return !cancel;
}

/*  The sender object will provide access to a Text           */
/*  and (if available in the source element) a Value property */
function PrepareSender(e)   
{
    var sender;
    
    if (!e) var e = window.event;

    if (e.srcElement != undefined)
        sender = e.srcElement;
    else
        sender = e.target;
        
    //This property may differ from the Text one, as in a Select element.
    if (sender.value != undefined)
        sender.Value = new NetString(sender.value);
    else
        sender.Value = "";
    
    if (sender.text != undefined)
        sender.Text = new NetString(sender.text);
    else
        sender.Text = sender.Value;

    return sender;
}

/*  The e event argument will provide the Cancel property. */
/*  Look the .NET documentation for CancelEventArgs        */
function PrepareChangeArgument(e)
{
    if (!e) var e = window.event;

    var arg = new Object();
    //By default, the event passes on.
    arg.Cancel = false;
    return arg;
}

/* The e event argument will provide the .NET equivalents of        */
/* Handled (to cancel the event) and KeyChar properties. Look the   */
/* .NET documentation of KeyPressEventArgs. Additionally we provide */
/* a KeyCode property. IsControl is a NS6+ workaround.              */
function PrepareKeyPressArgument(e)
{
    if (!e) var e = window.event;

    var arg = e;
    var key;
    
    //By default, the event should pass over.
    arg.Handled = false;

    //IE doesn't define the which event property.
    if (e.which == undefined)
        key = e.keyCode;
    else
	    //In Nav6/Mozilla, charCode is used if its an
	    //alphanumeric key and keyCode is used if it is not.
	    key = (e.charCode != 0) ? e.charCode : e.keyCode;
	    
	arg.KeyChar = String.fromCharCode(key);
	arg.KeyCode = key;
	if (e.charCode == 0 || e.altKey || e.ctrlKey)
	    arg.IsControl = true;
	else
	    arg.IsControl = false;
	
    return arg;
}
-->
</script>
