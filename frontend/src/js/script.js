// // init Isotope
// var $grid = $('.collection-list').isotope({
//     itemSelector: '.collection-item',
//     layoutMode: 'fitRows'
//   });
//   // filter items on button click
//   $('.filter-button-group').on( 'click', 'button', function() {
//     var filterValue = $(this).attr('data-filter');
//     resetFilterBtns();
//     $(this).addClass('active-filter-btn');
//     $grid.isotope({ filter: filterValue });
//   });
  
//   var filterBtns = $('.filter-button-group').find('button');
//   function resetFilterBtns() {
//     $('.filter-button-group').find('button').removeClass('active-filter-btn');
//   }