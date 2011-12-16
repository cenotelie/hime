function toggle(btn, content)
{
    if (content.style.display == "none")
    {
        content.style.display = "";
        btn.src = 'hime_data/button_minus.gif';
    }
    else
    {
        content.style.display = "none";
        btn.src = 'hime_data/button_plus.gif';
    }
}

function display(uri) {
    document.getElementById('myContent').innerHTML = "<iframe frameborder='no' src='" + uri + "'  class='maximize'/>";
}


var columnNames = ["ColumnAction", "ColumnHead", "ColumnBody", "ColumnLookaheads", "ColumnConflicts"];
var columnIsOn = [true, true, true, true, true];
function showColumn(index) {
	columnIsOn[index] = true;
	resetColumns();
}
function hideColumn(index) {
	columnIsOn[index] = false;
	resetColumns();
}

var style;
function resetColumns() {
	if(typeof style == 'undefined') {
        var append = true;
        style = document.createElement('style');
    } else {
        while (style.hasChildNodes()) {
            style.removeChild(style.firstChild);
        }
    }
    var head = document.getElementsByTagName('head')[0];
    var rules = new Array();
	for (var i=0; i!=columnNames.length; i++) {
		if (columnIsOn[i])
			rules[i] = document.createTextNode('.' + columnNames[i] + '{}');
		else
			rules[i] = document.createTextNode('.' + columnNames[i] + '{display:none;}');
	}

    style.type = 'text/css';
    if(style.styleSheet) {
        style.styleSheet.cssText = "";
		for (i=0; i!=columnNames.length; i++) {
			style.styleSheet.cssText += ' ' + rules[i].nodeValue;
		}
    } else {
		for (i=0; i!=columnNames.length; i++) {
			style.appendChild(rules[i]);
		}
    }
    if(append === true) head.appendChild(style);
}