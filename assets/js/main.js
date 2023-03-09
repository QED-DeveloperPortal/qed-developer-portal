$(document).ready(function() {
  console.log("Doc READY");

  // initialize smooth scroll
  $("a").smoothScroll({ offset: -20 });

  // add lightbox class to all image links
  $("a[href$='.jpg'], a[href$='.png'], a[href$='.gif']").attr("data-lity", "");
});

// $(document).ready(function($) {
//   $('.js-nav-menu-toggle').on('click', function() {
//     console.log('** Nav clicked!!!');
//     $(this).parents('.navigation-menu').toggleClass('navigation-menu--open');
//   });
  
//   $('html').on('click', function(e) {
//     if(!$(e.target).closest('.js-nav-menu').length &&
//       ($('.js-nav-menu').hasClass('navigation-menu--open'))) {
//         $('.js-nav-menu').removeClass('navigation-menu--open');
//     }
//   });
// })(jQuery);
