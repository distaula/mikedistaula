﻿$(document).ready(function () {

	var consoleTimeout;

	$('.minicolors').each(function () {
		//
		// Dear reader, it's actually much easier than this to initialize 
		// miniColors. For example:
		//
		//  $(selector).minicolors();
		//
		// The way I've done it below is just to make it easier for me 
		// when developing the plugin. It keeps me sane, but it may not 
		// have the same effect on you!
		//
		$(this).minicolors({
			control: $(this).attr('data-control') || 'hue',
			defaultValue: $(this).attr('data-default-value') || '',
			inline: $(this).hasClass('inline'),
			letterCase: $(this).hasClass('uppercase') ? 'uppercase' : 'lowercase',
			opacity: $(this).hasClass('opacity'),
			position: $(this).attr('data-position') || 'default',
			styles: $(this).attr('data-style') || '',
			swatchPosition: $(this).attr('data-swatch-position') || 'left',
			textfield: !$(this).hasClass('no-textfield'),
			theme: $(this).attr('data-theme') || 'default',
			change: function (hex, opacity) {

				// Generate text to show in console
				text = hex ? hex : 'transparent';
				if (opacity) text += ', ' + opacity;
				text += ' / ' + $(this).minicolors('rgbaString');

				// Show text in console; disappear after a few seconds
				$('#console').text(text).addClass('busy');
				clearTimeout(consoleTimeout);
				consoleTimeout = setTimeout(function () {
					$('#console').removeClass('busy');
				}, 3000);
			}
		});

	});
});