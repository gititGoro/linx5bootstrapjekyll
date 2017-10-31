var _prevIndex = 0;
var _nextIndex = 0;
var _resultsPerPage = 10;
var _pageNumber = 1;
var mGoogleApiKey = 'AIzaSyBLCxNvv0IKcgpg-cil5prz4LFS75DZFsA';
var mGoogleCustomSearchKey = '011901386512917488629:ogdcfae0lsa';

$(document).ready(function () {
	Search(0);

	$('#prevResults').click(function () { Search(-1); });
	$('#nextResults').click(function () { Search(1); });
});

function Search(direction) {
	$("#searchSummary").html('Searching for "' + $("#searchTerm").val() + '"...');
	$("#searchBody").html("");

	var startIndex = 1;

	if (direction === -1) {
		startIndex = _prevIndex;
		_pageNumber--;
	}
	if (direction === 1) {
		startIndex = _nextIndex;
		_pageNumber++;
	}
	if (direction === 0) {
		startIndex = 1;
		_pageNumber = 1;
	}

	var url = "https://www.googleapis.com/customsearch/v1?key=" + mGoogleApiKey + "&num=10&cx=" + mGoogleCustomSearchKey + "&start=" + startIndex + "&q=" + escape($("#searchTerm").val()) + "&alt=json";

	$.getJSON(url, '', SearchCompleted);
}

function SearchCompleted(response) {
	UpdateSearchNav(response);
	
	if (response.searchInformation.totalResults == "0" || response.items == null || response.items.length === 0) {
		$("#searchSummary").html('No matches found for "' + $("#searchTerm").val() + '"');
		return;
	}

	$("#searchSummary").html(ExtractSummary(response));

	$("#searchBody").html(ExtractResults(response));
}

function ExtractSummary(response) {
	var summaryhtml = response.searchInformation.totalResults + ' matches found for "' + $("#searchTerm").val() +'"';
	return summaryhtml;
}

function ExtractResults(response) {
	var bodyHtml = "";
	for (var i = 0; i < response.items.length; i++) {
		bodyHtml += ExtractResult(response.items[i]);
	}
	return bodyHtml;
}

function ExtractResult(result) {
    var resultHtml = "<div class='resultItem'><a class='resultsTitle' href=\"" + result.link + "\">" + result.title + "</a>";
    resultHtml += "<a class='resultLink' href=\"" + result.link + "\">http://" + result.formattedUrl + "</a>";
    resultHtml += result.htmlSnippet + "</div>";
	return resultHtml;
}

function UpdateSearchNav(response) {
	if (response.queries.nextPage != null) {
		_nextIndex = response.queries.nextPage[0].startIndex;
		$("#nextResults").show();
	}
	else {
		$("#nextResults").hide();
	}

	if (response.queries.previousPage != null) {
		_prevIndex = response.queries.previousPage[0].startIndex;
		$("#prevResults").show();
	}
	else {
		$("#prevResults").hide();
	}

	if (response.queries.request[0].totalResults > _resultsPerPage) {
	    $("#paging").show().html("Page " + _pageNumber + " of " + Math.ceil(response.queries.request[0].totalResults / _resultsPerPage));
	}
	else {
		$("#paging").hide();
	}
}