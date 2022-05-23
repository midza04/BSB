 $(document).ready(function () {
	 
 	$(".ts-sidebar-menu li a").each(function () {
 		if ($(this).next().length > 0) {
 			$(this).addClass("parent");
 		};
 	})
 	var menux = $('.ts-sidebar-menu li a.parent');
 	$('<div class="more"><i class="fa fa-angle-down"></i></div>').insertBefore(menux);
 	$('.more').click(function () {
 		$(this).parent('li').toggleClass('open');
 	});
	$('.parent').click(function (e) {
		e.preventDefault();
 		$(this).parent('li').toggleClass('open');
	});

 	$('.menu-btn').click(function () {
 		$('nav.ts-sidebar').toggleClass('menu-open');
 	});

 	$('.settings-date-picker').datetimepicker({
 	    format: 'DD MMM YYYY',
 	    default: $(this).html()
 	});
	 
 	var loggerTable = $('#loggerTable').DataTable({ "searching": true, "ordering": false, "pageLength": 25, "columns": [null, { "width": "180px" }, null, { "width": "150px" }] });

 	var uploadsTable = $('#uploadsTable').DataTable({ "searching": true, "ordering": false, "pageLength": 25, "columns": [null, { "width": "180px" }, null, { "width": "150px" }] });

 	//$('#loggerTable thead th').each(function () {
 	//    var title = $(this).text();
 	//    $(this).html('<input type="text" placeholder="Search ' + title + '" />');
 	//}); 	

 	//loggerTable.columns().each(function () {
 	//    var that = this;
 	//    $('input', this.header()).on('keyup change', function () {

 	//        if (that.search() !== this.value) {
 	//            that
    //                .search(this.value)
    //                .draw(); 	            
 	//        }
 	//    });
 	//});

 	
	 
	 $("#input-43").fileinput({
		showPreview: false,
		allowedFileExtensions: ["zip", "rar", "gz", "tgz"],
		elErrorContainer: "#errorBlock43"
			// you can configure `msgErrorClass` and `msgInvalidFileExtension` as well
	});

 });
