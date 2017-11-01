$(document).ready(function() {
	$('#searchField').keyup(function (e) {
		if (e.keyCode == 13) {
			var searchTerm = $('#searchField').val();
			window.location = $(this).data("search-url") + "?term=" + escape(searchTerm);
		}
	});
});