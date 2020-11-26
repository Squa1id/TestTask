var dataTable = {
	dom: "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'f>>" +
		"<'row'<'col-sm-12'tr>>" +
		"<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
	language: {
		processing: "<span style='font-size: large;'>Загрузка...</span>",
		loadingRecords: "<span style='font-size: large;'>Загрузка...</span>",
		lengthMenu: 'Записей: _MENU_',
		zeroRecords: "Ничего не найдено",
		info: "Страница _PAGE_ из _PAGES_",
		infoEmpty: "Нет доступных записей",
		infoFiltered: '',
		paginate: {
			first: '<i class="mdi mdi-18px mdi-chevron-double-left"></i>',
			previous: '<i class="mdi mdi-18px mdi-chevron-left"></i>',
			next: '<i class="mdi mdi-18px mdi-chevron-right"></i>',
			last: '<i class="mdi mdi-18px mdi-chevron-double-right"></i>'
		}
	},
	lengthMenu: [10, 20, 30, 50, 100],
	createTable: function (selector, method, columns, data) {
		var ajax = {
			url: method, type: 'POST'
		};
		if (data) {
			ajax.data = data;
		}
		var table = $(selector).DataTable({
			dom: this.dom,
			ordering: false,
			processing: true,
			stateSave: true,
			autoWidth: false,
			searching: false,
			serverSide: true,
			pagingType: 'full',
			paging: true,
			//order: [],
			scrollX: true,
			scrollY: 600,
			scrollCollapse: true,
			lengthMenu: dataTable.lengthMenu,
			ajax: ajax,
			columns: columns,
			language: dataTable.language,
			drawCallback: function () {
				table.selectedRowsCount = 0;
			}
		});
		table.onRowSelect = null;
		$(`#${selector.id} tbody`).on('click', 'tr', function () {
			if ($(this).hasClass('selected')) {
				table.selectedRowsCount--;
			} else {
				table.selectedRowsCount++;
			}
			$(this).toggleClass('selected');
			if (table.onRowSelect) {
				table.onRowSelect();
			}
		});
		return table;
	}
};