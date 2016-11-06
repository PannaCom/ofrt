$(document).ready(function(){
	"use strict";
	
	/*
	  ==============================================================
		   Banner Bx-Slider Script Start
	  ============================================================== */
	if($('.banner_bxslider').length){
		$('.banner_bxslider').bxSlider({
			auto:true,
		});
	}
	
	/* ---------------------------------------------------------------------- */
	/*	DL Responsive Menu
	/* ---------------------------------------------------------------------- */
	if(typeof($.fn.dlmenu) == 'function'){
		$('#kode-responsive-navigation').each(function(){
			$(this).find('.dl-submenu').each(function(){
				if( $(this).siblings('a').attr('href') && $(this).siblings('a').attr('href') != '#' ){
					var parent_nav = $('<li class="menu-item kode-parent-menu"></li>');
					parent_nav.append($(this).siblings('a').clone());
					
					$(this).prepend(parent_nav);
				}
			});
			$(this).dlmenu();
		});
	}
	
	/*
	  ==============================================================
		   Tab View Script Start
	  ============================================================== */
	
	if($('#tabs').length){
		$('#tabs').tab();
	}
		
		
	/* ==================================================================
					Number Count Up(WayPoints) Script
	  =================================================================	*/		
	if($('.counter').length){
		$('.counter').counterUp({
			delay: 10,
			time: 1000
		});
	}
	 
	/*
	  ==============================================================
		   Post Bx-Slider Script Start
	  ============================================================== */
	if($('.countdown').length){
		$('.countdown').downCount({ date: '08/08/2016 12:00:00', offset: +1 });
	}
	  
	  
	/*
	  ==============================================================
		   Recent Property and Our Agent Owl Carousel Script Start
	  ============================================================== */
	if($('.kf_rent_property,.kf_agent_slider').length){
		var owl = $(".kf_rent_property,.kf_agent_slider");
		owl.owlCarousel({	
		 autoPlay: 3000, //Set AutoPlay to 3 seconds
		  itemsCustom : [
			[0, 1],
			[450, 1],
			[600, 1],
			[700, 2],
			[1000, 3],
			[1200, 4],
			[1400, 4],
			[1600, 4]
		  ],
		  navigation : true

		});
	}
	
	/*
	  ==============================================================
		   Latest Blog Post Owl Carousel Script Start
	  ============================================================== */
	if($('.kf_blog_slider').length){
		var owl = $(".kf_blog_slider");
		owl.owlCarousel({	
		 autoPlay: 3000, //Set AutoPlay to 3 seconds
		  itemsCustom : [
			[0, 1],
			[450, 1],
			[600, 1],
			[700, 2],
			[1000, 3],
			[1200, 3],
			[1400, 3],
			[1600, 3]
		  ],
		  navigation : true

		});
	}
	
	/*
	  ==============================================================
		   Company List Owl Carousel Script Start
	  ============================================================== */
	if($('.kf_company_slider').length){
		var owl = $(".kf_company_slider");
		owl.owlCarousel({	
		 autoPlay: 3000, //Set AutoPlay to 3 seconds
		  itemsCustom : [
			[0, 1],
			[450, 1],
			[600, 2],
			[700, 2],
			[1000, 3],
			[1200, 5],
			[1400, 5],
			[1600, 5]
		  ],
		  navigation : true

		});
	}
	
	/*
	  ==============================================================
		   Accordion Script Start
	  ============================================================== */
	if($('.accordion').length){
		//custom animation for open/close
		$.fn.slideFadeToggle = function(speed, easing, callback) {
		  return this.animate({opacity: 'toggle', height: 'toggle'}, speed, easing, callback);
		};

		$('.accordion').accordion({
		  defaultOpen: 'section1',
		  cookieName: 'nav',
		  speed: 'slow',
		  animateOpen: function (elem, opts) { //replace the standard slideUp with custom function
			elem.next().stop(true, true).slideFadeToggle(opts.speed);
		  },
		  animateClose: function (elem, opts) { //replace the standard slideDown with custom function
			elem.next().stop(true, true).slideFadeToggle(opts.speed);
		  }
		});
	}
	
	/*
	  ==============================================================
		   Client Review Owl Carousel Script Start
	  ============================================================== */
	if($('.kf_client_review_slider').length){
		var owl = $(".kf_client_review_slider");
		owl.owlCarousel({	
		 autoPlay: 3000, //Set AutoPlay to 3 seconds
		  itemsCustom : [
			[0, 1],
			[450, 1],
			[600, 2],
			[700, 2],
			[1000, 2],
			[1200, 2],
			[1400, 2],
			[1600, 2]
		  ],
		  navigation : true

		});
	}
	
	/*
	  ==============================================================
		   Twitter Bx-Slider Script Start
	  ============================================================== */
	if($('.kf_twitter_slider,.kf_testimonial_slider').length){
		$('.kf_twitter_slider,.kf_testimonial_slider').bxSlider({
			auto:true,
		});
	} 		
		  
	/*
	  =======================================================================
		  		 Pretty Photo Script
	  =======================================================================
	*/
	if($("a[data-rel^='prettyPhoto']").length){
		$("a[data-rel^='prettyPhoto']").prettyPhoto();
	}
	
	/*
	  =======================================================================
		  		 Chosen Script Script
	  =======================================================================
	*/	
	if($(".chosen-select").length){
		$(".chosen-select").chosen()
	}
	
	/*
	  =======================================================================
		  	Listing Bx-Slider Pager Script Script
	  =======================================================================
	*/	
	if($('.kf_listing_slider').length){
	$('.kf_listing_slider').bxSlider({
	  pagerCustom: '.kf_blog_listing_pager',
	  auto:true,
	});
	}
		
	/*
	  =======================================================================
		  	Listing Bx-Slider Pager Script Script
	  =======================================================================
	*/	
	if($('.property_pager_item').length){
		$('.property_pager_item').bxSlider({
		  pagerCustom: '#property_detail_pager',
		  auto:true,
		});
	}

	/*
	  =======================================================================
		  		Filter Able Script Script
	  =======================================================================
	*/
	
	jQuery(window).load(function($) {
		if(jQuery('#filterable-item-holder-1').length){
		var filter_container = jQuery('#filterable-item-holder-1');

		filter_container.children().css('position','relative');	
		filter_container.masonry({
			singleMode: true,
			itemSelector: '.filterable-item:not(.hide)',
			animate: true,
			animationOptions:{ duration: 800, queue: false }
		});	
		jQuery(window).resize(function(){
			var temp_width =  filter_container.children().filter(':first')();
			filter_container.masonry({
				columnWidth: temp_width,
				singleMode: true,
				itemSelector: '.filterable-item:not(.hide)',
				animate: true,
				animationOptions:{ duration: 800, queue: false }
			});		
		});	
		jQuery('ul#filterable-item-filter-1 a').on('click',function(e){	

			jQuery(this).addClass("active");
			jQuery(this).parents("li").siblings().children("a").removeClass("active");
			e.preventDefault();
			
			var select_filter = jQuery(this).attr('data-value');
			
			if( select_filter == "All" || jQuery(this).parent().index() == 0 ){		
				filter_container.children().each(function(){
					if( jQuery(this).hasClass('hide') ){
						jQuery(this).removeClass('hide');
						jQuery(this).fadeIn();
					}
				});
			}else{
				filter_container.children().not('.' + select_filter).each(function(){
					if( !jQuery(this).hasClass('hide') ){
						jQuery(this).addClass('hide');
						jQuery(this).fadeOut();
					}
				});
				filter_container.children('.' + select_filter).each(function(){
					if( jQuery(this).hasClass('hide') ){
						jQuery(this).removeClass('hide');
						jQuery(this).fadeIn();
					}
				});
			}
			
			filter_container.masonry();	
			
		});
		}
	});
	
	/*
	  =======================================================================
		  		Map Script Script
	  =======================================================================
	*/
	if($('#map-canvas').length){
		google.maps.event.addDomListener(window, 'load', initialize);
	}
	
	/*
	  =======================================================================
		  		Range Slider Script Script
	  =======================================================================
	*/
	if($('.slider-range').length){
		$( ".slider-range" ).slider({
		  range: true,
		  min: 0,
		  max: 500,
		  values: [ 50, 450 ],
		  slide: function( event, ui ) {
			$( ".amount" ).val( "$" + ui.values[ 0 ] + " - $" + ui.values[ 1 ] );
		  }
		});
		$( ".amount" ).val( "$" + $( ".slider-range" ).slider( "values", 0 ) +
		  " - $" + $( ".slider-range" ).slider( "values", 1 ) );
	}
	
});

	/* ---------------------------------------------------------------------- */
	/*	Google Map Function for Custom Style
	/* ---------------------------------------------------------------------- */
	function initialize() {
		var MY_MAPTYPE_ID = 'custom_style';
		var map;
		var brooklyn = new google.maps.LatLng(40.6743890, -73.9455);
		var featureOpts = [
			{
			  stylers: [
				{ hue: '#f9f9f9' },			
				{ visibility: 'simplified' },
				{ gamma: 0.7 },
				{ saturation: -200 },
				{ lightness: 45 },
				{ weight: 0.6 }
			  ]
			},
			{
			featureType: "road",
			  elementType: "geometry",
			  stylers: [
				{ lightness: 200 },
				{ visibility: "simplified" }
			  ]
			},
			{
			  elementType: 'labels',
			  stylers: [		  
				{ visibility: 'on' }
			  ]
			},
			{
			  featureType: 'water',
			  stylers: [
				{ color: '#ffffff' }
			  ]
			}
		];	

		var mapOptions = {
			zoom: 15,
			scrollwheel: false,
			center: brooklyn,
			mapTypeControlOptions: {
			  mapTypeIds: [google.maps.MapTypeId.ROADMAP, MY_MAPTYPE_ID]
			},
			mapTypeId: MY_MAPTYPE_ID
		};

		map = new google.maps.Map(document.getElementById('map-canvas'),
			  mapOptions);

		var styledMapOptions = {
			name: 'Custom Style'
		};

		var customMapType = new google.maps.StyledMapType(featureOpts, styledMapOptions);

		map.mapTypes.set(MY_MAPTYPE_ID, customMapType);
	}