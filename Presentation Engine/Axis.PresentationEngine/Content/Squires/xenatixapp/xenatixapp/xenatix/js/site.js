Array.prototype.remove = function(v) {
    return $.grep(this, function(e) {
        return e !== v;
    });
};

var owlSetup = {

	pagination : ".slide-slideshow .owl-controls",
	slideshow : ".slide-slideshow .slideshow",
	next : ".slide-slideshow .slideshowouter .next",
	prev : ".slide-slideshow .slideshowouter .prev",
	item : ".slide-slideshow .item",
	owlControls : ".slide-slideshow .owl-controls",

	sectionMenu : ".section-menu",

	init : function(){
		if($().owlCarousel){
			var owl = $(owlSetup.slideshow).owlCarousel({
					"singleItem" : true,
					"autoPlay" : true,
					"transitionStyle" : "fade",
					"lazyLoad" : true,
					"stopOnHover" : true,
					"dragBeforeAnimFinish" : true,
					"mouseDrag" : true,
					"touchDrag" : true,
					"autoHeight" : true
			});
			// Custom Navigation Events
			$(owlSetup.next).click(function(){
				owl.trigger('owl.next');
			});
			$(owlSetup.prev).click(function(){
				owl.trigger('owl.prev');
			});
			owlSetup.controls('first');
		}
	},
	controls : function(e){
		var slides = $(owlSetup.item);

		//Pagination positioning
		var sectionMenuHeight = $(owlSetup.sectionMenu).height();
		$(owlSetup.pagination).css({ "bottom" : sectionMenuHeight + 10 });

		if($().owlCarousel){
			if( e == 'first'){
				$(owlSetup.prev + ',' + owlSetup.next).hide();
				var slideshowHeight = $(owlSetup.slideshow).height();
				var navHeight = $(owlSetup.next).outerHeight();
				var navTop = (slideshowHeight - navHeight) / 2;

				if(navTop >= 2){
					$(owlSetup.prev + ',' + owlSetup.next).css({'top' : navTop});
				}
				$(owlSetup.prev + ',' + owlSetup.next).show();
			}else{
				$('.slideshow img').bind('resize',function(){
					$(window).trigger('resize');
				});
				$(window).bind('resize', function(){
					$(owlSetup.prev + ',' + owlSetup.next).hide();
					var slideshowHeight = $(owlSetup.slideshow).height();
					var navHeight = $(owlSetup.next).outerHeight();
					var navTop = (slideshowHeight / 2) - navHeight;

					if(navTop >= 2){
						$(owlSetup.prev + ',' + owlSetup.next).css({'top' : navTop});
					}
					$(owlSetup.prev + ',' + owlSetup.next).show();
				});
			}
		}
	}
};

var offCanvasNav = {
	init : function(){
		offCanvasNav.calcOffCanvasHeight();
		$(window).resize(function(){
			offCanvasNav.calcOffCanvasHeight();
			//enquireBreakPoints.docHeight();
		});

		$('[data-toggle=offcanvas]').click(function () {
			sq.log('offCanvasNav click');
			$('.row-offcanvas').toggleClass('active');
		});
	},
	calcOffCanvasHeight : function(){
		var mainContainer = $(document).height();
		$( '.sidebar-offcanvas' ).css({ 'height' : mainContainer });
	}
};

var lazyLoading = {
	init : function(){
		var ltIE9 = $('html').hasClass('lt-ie9');
		$(".lazy").lazyload({
			skip_invisible:false,
			effect:"fadeIn",
			event : "sporty",
			threshold: 10000,
			failure_limit:1000
		});
		$(window).bind("load", function() {
			var timeout = setTimeout(function() {
				$(".lazy").trigger("sporty")
			}, 100);
		});
		/**
		 * As images load re-calc the height of the columns
		 */

		$('img.lazy').load(function(){
			$(this).removeClass('lazy-logo');
			enquireBreakPoints.init();
		});

		//alert(navigator.appVersion);
		if ( ltIE9 ){
			setInterval(function(){

				enquireBreakPoints.init();

			}, 3000); //every 3 seconds
		}
	}
};

var enquireBreakPoints = {
	debug: false,
	init : function(){
		enquire.register("screen and (max-width: 767px)", {
			match : function(){
				$('body').addClass('mobile');
				enquireBreakPoints.reinit(true);
				sq.jsCenterContent.reset();
			},
			unmatch : function(){
				$('body').removeClass('mobile');
			}
		});

		enquire.register("screen and (min-width: 768px) and (max-width: 991px)", {
			match : function(){
				$('body').addClass('mobile');
				enquireBreakPoints.reinit();
				sq.jsCenterContent.reset();
			},
			unmatch : function(){
				$('body').removeClass('mobile');
			}
		});

		enquire.register("screen and (min-width: 992px) and (max-width: 1199px)", {
			match : function(){
				enquireBreakPoints.reinit();
			}
		});
		enquire.register("screen and (min-width: 1199px)", {
			match : function(){
				enquireBreakPoints.reinit();
			}
		});
		enquire.register("screen and (min-width: 500000px)", {
			match : function(){
				enquireBreakPoints.reinit();
			}
		}, true);
	},
	reset : function(eq){
		$( '.col-1,.col-2,.col-3',eq ).css({'height' : 'auto'});
	},
	reinit : function(smallest) {
		/**
		 * Equal height row
		 */
		var eq = $('.row.equal-height');
		enquireBreakPoints.reset(eq);//Reset heights to get correct values

		/**
		 * Owl Navigation positioning
		 */
		owlSetup.controls();
		sq.jsCenterContent.init();
	},

	log: function(message) {
		if (this.debug) {
			if (typeof console == "undefined" || typeof console.log == "undefined") {
				console = {log: function() {}};
			}
			console.log(message);
		}
	},

	// ===== End of Object =====

	endOfObject: 1

};

var sq = {
	debug: false,
	panelGroup : '.panel-group',
	panelTitle : '.panel-title',
	jsCenter : '.js-center',

	init: function() {
		sq.jsCenterContent.init();

		$('[rel="popover"]').popover({
			placement: "auto left",
			html : true,
			trigger : "click",
		});
		$('[rel="popover"].open').popover('show');

	},
	jsCenterContent: {
		init: function(){
			if( !$("body.short-screen").length ){
				$(sq.jsCenter).each(function(){
					var objectHeight = $(this).outerHeight();
					var parentHeight = $(this).parent().parent().outerHeight();
					$(this).css({ "top" : ( ( parentHeight - objectHeight ) ) / 2 });
				});
			}else{
				$(sq.jsCenter).each(function(){
					$(this).parent().css({ "height" : "auto" }).parent().css({ "height" : "auto" });
				});
			}

		},
		reset: function(){
			$(sq.jsCenter).each(function(){
				$(this).css({ "top" : "inherit" });
			});
		},
	},

	equalHeadings: {
		debug: false,

		init: function() {
			var highestCol = Math.max( $( '#content .row .col-1 h1:first' ).height(), $( '#content .row .col-1 h3:first' ).height(), $( '#content .row .col-2 h3:first' ).height(), $( '#content .row .sidebar h3:first' ).height() );
			$( '#content .row .col-1 h1:first,#content .row .col-1 h3:first,#content .row .col-2 h3:first,#content .row .sidebar h3:first' ).height(highestCol);
			$( '.page-content h3' ).css({'height' : 'auto'});
		},

		log: function(message) {
			if (this.debug) {
				sq.log(message);
			}
		},

		// ===== End of Object =====

		endOfObject: 1
	},

	log: function(message) {
		if (this.debug) {
			if (typeof console == "undefined" || typeof console.log == "undefined") {
				console = {log: function() {}};
			}
			console.log(message);
		}
	},

	// ===== End of Object =====

	endOfObject: 1
}

var bootstrapClassHelpers = {
	init : function(){
		$('.pull-down').each(function() {
			$(this).css('margin-top', $(this).parent().height()-$(this).height());
			$(this).fadeIn('slow');
		});
	}
};

var generalForms = {
	init : function(){
		$('#cat').change(function() {
			this.form.submit();
		});
	}
};

var analytics = {
	event : function(type, data) {
		sq.log("Analytics: type of event:" + type + ", data: " + data);
	}
};

var lbox = {
	instance : '',
	init : function(){
		lbox.instance = $('a[rel*="lightbox"]').swipebox({
				useCSS: true,
				hideBarsDelay: 6000,
				hideBarsOnMobile: false,
				initialIndexOnArray: 1
		});
	}
};

var countCSS = {
	debug : true,
	init : function() {
		var results = '',
			log = '';
		if (!document.styleSheets) {
			return;
		}
		for (var i = 0; i < document.styleSheets.length; i++) {
			countSheet(document.styleSheets[i]);
		}
		function countSheet(sheet) {
			var count = 0;
			try {
				if (sheet && sheet.cssRules) {
					for (var j = 0, l = sheet.cssRules.length; j < l; j++) {
						var rule = sheet.cssRules[j];
						if (rule instanceof CSSImportRule) {
							countSheet(rule.styleSheet);
						}
						if( !rule.selectorText ) {
							continue;
						}
						count += rule.selectorText.split(',').length;
					}

					log += '\nFile: ' + (sheet.href ? sheet.href : 'inline <style> tag');
					log += '\nRules: ' + sheet.cssRules.length;
					log += '\nSelectors: ' + count;
					log += '\n--------------------------';
					if (count >= 4096) {
						results += '\n********************************\nWARNING:\n There are ' + count + ' CSS rules in the stylesheet ' + sheet.href + ' - IE will ignore the last ' + (count - 4096) + ' rules!\n';
					}
				}
			} catch(e) {
				if (e.name == "SecurityError") {
					log += "\nCannot count CSS rules because of a security error.";
				} else {
					if (e.name == "InvalidAccessError") {
						log += "\nUnsupported operation.";
					} else {
						throw e;
					}
				}

				return;
			}
		}
		countCSS.log(log);
		countCSS.log(results);
	},

	log: function(message) {
		if (this.debug) {
			if (typeof console == "undefined" || typeof console.log == "undefined") {
				console = {log: function() {}};
			}
			console.log(message);
		}
	},

	// ===== End of Object =====

	endOfObject: 1
};

var wayPoints = {
	debug: false,
	sections: '.waypoint-section',

	init: function(){
		jQuery.each( jQuery(wayPoints.sections) ,function(){
			var sectionID = '#' + jQuery(this).prop('id');
			jQuery(sectionID).waypoint({
				handler: function(direction) {
					wayPoints.log(this.element.id + ' hit');
					if( direction == 'down' ){
						jQuery(sectionID).removeClass('up').addClass('down');
						jQuery('.animateMe', sectionID).removeClass('animateMe').addClass('animated');
					}
					if( direction == 'up' ){
						jQuery(sectionID).removeClass('down').addClass('up');
					}
					jQuery(sectionID).one('webkitAnimationEnd MSAnimationEnd oanimationend animationend', function(){
						jQuery('.animated', sectionID).addClass('finishedAnimation');
					});
				},
				offset: '70%'
			});
		});
        var navani = jQuery('#mission').waypoint({
            handler: function(direction) {
                jQuery('.navbar-fixed-top').toggleClass('border-bottom-solid');
            },
            offset: 100
        });
	},

	log: function(message) {
		if (this.debug) {
			if (typeof console == "undefined" || typeof console.log == "undefined") {
				console = {log: function() {}};
			}
			console.log(message);
		}
	},

	// ===== End of Object =====

	endOfObject: 1
};

var scrollToAnchor = {
    init: function(){
        jQuery('a[href^="/#"]').on('click', function(event) {
            if (location.pathname.replace(/^\//,'') == this.pathname.replace(/^\//,'') && location.hostname == this.hostname) {
                var target = jQuery(this.hash)
                if( target.length ) {
                    event.preventDefault();
                    jQuery('html, body').animate({
                        scrollTop: target.offset().top - 100
                    }, 800, function() {
                        window.location.hash = target.selector;
                    });
                }
            }
        });
    },

    // ===== End of Object =====

    endOfObject: 1
};

$(document).ready(function($) {
	sq.log("Document Ready");
	sq.init();
	owlSetup.init();
	lazyLoading.init();
	enquireBreakPoints.init();
	bootstrapClassHelpers.init();
	generalForms.init();
	wayPoints.init();
	scrollToAnchor.init();
	offCanvasNav.init();
	$('.panel-scroll').perfectScrollbar();

	/**
	 * Temporary Chrome font fix
	 */
	$(function() { $('body').hide().show(); });
});
