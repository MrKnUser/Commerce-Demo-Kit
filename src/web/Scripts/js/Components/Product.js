/* jshint -W099 */
/* global jQuery:false */

(function($, Oxx, commercestarterkit){

	"use strict";

//********************************************************************************
//*NAMESPACES ********************************************************************
//********************************************************************************
	commercestarterkit = window.commercestarterkit = (!commercestarterkit) ? {} : commercestarterkit;

//********************************************************************************
//*CLASS VARIABLES****************************************************************
//********************************************************************************
	var defaultOptions = {

	};


//********************************************************************************
//*CONSTRUCTOR********************************************************************
//********************************************************************************
	/**
	 *
	 * @param containerId
	 * @param options
	 * @constructor
	 */
	commercestarterkit.Product = function(containerId, options) {

		/** @type {jQuery} */
		this._$el = $(containerId);

		/** @type {Object} */
		this._settings = {};

		/** @type {Object} */
		this._settings.options = Oxx.ObjectUtils.createOptionsObject(defaultOptions, options);

		/** @type {jQuery} */
		this._$sizes = this._$el.find('.sizes select');


		/** @type {jQuery} */
		this._$mainProductView = this._$el.find('.main-product-view');

		/** @type {jQuery} */
		this._$mainProductFullScreen = this._$el.find('.full-screen');

		/** @type {jQuery} */
		this._$wearItWithProducts = this._$el.find('.wear-it-with-products');

		/** @type {jQuery} */
		this._$lightbox = $('#lightbox');

		/** @type {jQuery} */
		this._$sizeguide = this._$el.find('.sizeguide');

		/** @type {string} */
		this.wearItWithProducts = 'vertical';

		/** @type {int} */
		this.wearItBreakPoint = 992;

		/** @type {boolean} */
		this.showZoomFlyout = false;

		this._init();
	};
//********************************************************************************
//*PROTOTYPE/PUBLIC FUNCTIONS*****************************************************
//********************************************************************************
	commercestarterkit.Product.prototype = {
		constructor: commercestarterkit.Product,

		/**
		 * Init the product view
		 *
		 * @private
		 */
		_init: function() {



			// attach events
			this._$sizes.on('change', $.proxy(this._onSizeChanged, this));
			this._$mainProductFullScreen.on('click', $.proxy(this._onMainProductFullScreenClick, this));
			this._$lightbox.find('a.close').on('click', $.proxy(this._onLightBoxCloseClick, this));
			$(window).on('resize', $.proxy(this._onWindowResized, this));


			this._$sizeguide.on('click', $.proxy(this._onSizeGuideClick, this));
		},


		/**
		 * Get the viewport width and height
		 * (better than $(window).width() because of the scrollbar
		 *
		 * @returns {{width: *, height: *}}
		 */
		viewport: function() {
			var e = window, a = 'inner';
			if (!('innerWidth' in window )) {
				a = 'client';
				e = document.documentElement || document.body;
			}
			return { width : e[ a+'Width' ] , height : e[ a+'Height' ] };
		},

//********************************************************************************
//*PRIVATE OBJECT METHODS ********************************************************
//********************************************************************************

		
//********************************************************************************


	
//********************************************************************************

		/**
		 * Set the height of the slider to be the same as the product images
		 *
		 * @private
		 */
		_setWearItWithHeight: function() {
			var mainProductImageContainer = this._$mainProductImageSlider.parent(),
				wearItWithProductsBox = this._$wearItWithProducts.find('.box');

				setTimeout(function() {
					var mainProductImageContainerHeight = mainProductImageContainer.height() - 20;
					if(wearItWithProductsBox.height() < mainProductImageContainerHeight) {
						wearItWithProductsBox.css('height', mainProductImageContainerHeight + 'px');
					}
				}, 200);
		},

//********************************************************************************

		/**
		 * Open the size guide dialog
		 *
		 * @param {int} reference
		 * @private
		 */
		_openSizeGuide: function(reference) {
			var $sizeGuideDialog = new commercestarterkit.SizeGuideDialog(reference);
		},

//********************************************************************************
//*EVENT METHODS******************************************************************
//********************************************************************************

		/**
		 * When user has changed product size
		 *
		 * @param event
		 * @private
		 */
		_onSizeChanged: function(event) {
			this._$sizes.parents('form').trigger('submit');
		},


//********************************************************************************

		/**
		 * When size guide button is clicked
		 *
		 * @param event
		 * @private
		 */
		_onSizeGuideClick: function(event) {
			event.preventDefault();
			event.stopPropagation();

			var $btn = $(event.currentTarget),
				reference = $btn.data('reference');

			if(reference > 0) {
				this._openSizeGuide(reference);
			}
		},

//********************************************************************************

		/**
		 *
		 * @param event
		 * @private
		 */
		_onSliderSwipeLeft: function(event) {
			var $slider = $(event.target).parents('.slider');
			$slider.flexslider('prev');

		},

//********************************************************************************
		/**
		 * Event for closing the fullscreen image lightbox
		 *
		 * @private
		 */
		_onLightBoxCloseClick: function(event) {
			event.preventDefault();
			event.stopPropagation();
			this._$lightbox.lightbox('hide');
		}

//********************************************************************************

	};


})(jQuery, window.Oxx, window.commercestarterkit);

