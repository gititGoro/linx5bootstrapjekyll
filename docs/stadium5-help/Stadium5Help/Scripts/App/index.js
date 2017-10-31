$(function () {
	$(".printPage").click(function() {
		printPage();
	});
	$("#contact-form input, #contact-form textarea").focus(function() {
		if ($(this).val() == $(this).attr("initial-data")) {
			$(this).val("");
		}
	});
	$("#contact-form input, #contact-form textarea").blur(function() {
		if ($(this).val() == "") {
			$(this).val($(this).attr("initial-data"));
		}
	});
	$("#contactUs").click(function() {
		if (!validateNameAndMessage()) {
			ShowContactUsMessageResult("Please supply a name and message.", false);
			return;
		}
		if (!validateEmail($("#contactus-email").val())) {
			ShowContactUsMessageResult("Please supply a valid email address.", false);
			return;
		}
		ShowContactUsSendingMessage();
		var message = new Object();
		message.name = $("#contactus-name").val();
		message.email = $("#contactus-email").val();
		message.contents = $("#contactus-message").val();
		$.post($(this).data("request-url"), message)
			.done(function() {
				ShowContactUsMessageResult("Thank you!", true);
			})
			.error(function(data) {
				ShowContactUsMessageResult("Sorry, there was a problem on the server. The mail was not sent.", false);
			});
	});
	var isTouch = (('ontouchstart' in window) || (navigator.msMaxTouchPoints > 0));
	if (!isTouch) {
	    $(".menu-tab").mouseenter(function () {
	        $(this).next(".sub-menu").show(200);
	    });
	    $("img:not(.icon-64, .printPage)").wrap(function () {
	        return "<a rel='lightbox' href='" + $(this).attr("src") + "'></a>";
	    });
	    $(".printPage").show();
	}
	$(".menu-tab").parent().mouseleave(function () {
	    $(".sub-menu").hide(50);
	});
	checkViewport();
	$(window).resize(function () {
	    checkViewport();
	});
	showMobileMenu();
	$("#contactUs").click(function () {
	    ga('send', 'event', 'button', 'click', this.id);
	});
});

function showMobileMenu() {
    var mainmenuitems = $("#menu").clone().removeAttr("id").attr("id", "MobileMenuContainer");
    mainmenuitems.find(".sub-menu").remove();
    $("#MobileMenuItems").append(mainmenuitems);
    $("#mobilemenu").click(function() {
        if ($(".showMobileMenu").length == 0) {
            $("#MobileMenuItems").css("left", "0").addClass("showMobileMenu");
            $("#content").css("padding-left", "220px");
        }
        else {
            $("#MobileMenuItems").css("left", "-200px").removeClass("showMobileMenu");
            $("#content").css("padding-left", "20px");
        }
    });
}

function checkViewport() {
    var viewportWidth = $(window).width();
    var isTouch = (('ontouchstart' in window) || (navigator.msMaxTouchPoints > 0));
    if (viewportWidth <= 1009) {
        if (($("a[rel='lightbox']").length == 0) && (!isTouch)) {
            $("img:not(.icon-64)").wrap(function () {
                return "<a rel='lightbox' href='" + $(this).attr("src") + "'></a>";
            });
        }
    }

    if (viewportWidth <= 1256) {
        $("a[rel='lightbox']").each(function () {
            $(this).replaceWith($(this).find("img"));
        });
    }
}

function ShowContactUsSendingMessage()
{
	$("#contactus-status-message").text("Sending email...");
	$("#contactus-status").show();
}
function ShowContactUsMessageResult(message, clearFields)
{
	$("#contactus-status-message").text(message);
	$("#contactus-status").show();
	if (clearFields) {
		$("#contactus-name").val($("#contactus-name").attr("initial-data"));
		$("#contactus-email").val($("#contactus-email").attr("initial-data"));
		$("#contactus-message").val($("#contactus-message").attr("initial-data"));
	}
	$("#contactus-status").fadeOut(2000);
}

function validateNameAndMessage()
{
	return !(
		!$("#contactus-name").val() ||
		!$("#contactus-message").val() ||
		$("#contactus-name").val() == $("#contactus-name").attr("initial-data") ||
		$("#contactus-message").val() == $("#contactus-message").attr("initial-data"));
}

function validateEmail(email)
{
	var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
	return re.test(email);
}

function printPage()
{
	window.print();
}
$(function () {
	$("video").click(function(){
		vid_play_pause(this);
	});
});

function vid_play_pause(obj) {
  if (obj.paused) {
    obj.play();
  } else {
    obj.pause();
  }
}
