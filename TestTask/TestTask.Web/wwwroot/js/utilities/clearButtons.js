var clearButtons = document.getElementsByClassName('clear-button');
for (var i = 0; i < clearButtons.length; i++) {
	clearButtons[i].addEventListener("click", function (e) {
		var inputId = e.currentTarget.attributes['for'].value;
		if (inputId) {
			var input = document.getElementById(inputId);
			input.value = null;
		}
	}, false);
}
