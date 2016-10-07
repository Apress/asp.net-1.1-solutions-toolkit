<script language="JavaScript">
<!--
  function IsChar(code)
  {
    return /\w/.test(String.fromCharCode(code));
  }
  function KeyPressed(e, src)
  {
    var code;

    if (e.which == undefined)
    {
      //IE
      code = e.keyCode;
    }
    else
    {
      //Mozilla/NS6+
      code = e.which;
    }

    // It's not a character value, leave the function.
    if (!IsChar(code)) return true;
    
    return !isNaN(String.fromCharCode(code));
  }
  function KeyUp(src)
  {
    // If we have a valid value, we save it for later,
    // else we restore the previously saved value.
    if (src.value <= src.max && src.value >= src.min)
    {
      src.original = src.value;
      return true;
    }
    else
    {
      // Exceptional case: the user is entering a negative number.
      if (src.value == "-") return true;

      src.value = src.original;
      return false;
    }
  }
  function SpinnerChanged(src)
  {
    // If we have a valid value, we save it for later,
    // else we restore the previously saved value.
    if (src.value <= src.max && src.value >= src.min)
    {
      src.original = src.value;
      return true;
    }
    else
    {
      src.value = src.original;
      return false;
    }
  }

  function Decrement(target)
  {
    src = document.getElementById(target);
    src.value = parseInt(src.value) - parseInt(src.step);
    SpinnerChanged(src);
    src.focus();
  }

  function Increment(target)
  {
    src = document.getElementById(target);
    src.value = parseInt(src.value) + parseInt(src.step);
    SpinnerChanged(src);
    src.focus();
  }
//-->
</script>

