var page = {
	elements: {
		addButton: document.getElementById('addButton'),
		editButton: document.getElementById('editButton'),
		deleteButton: document.getElementById('deleteButton'),
		importButton: document.getElementById('importButton'),
		exportButton: document.getElementById('exportButton'),
		findButton: document.getElementById('findButton'),
		table: document.getElementById('registerTable'),
		nameInput: document.getElementById('nameInput'),
		auditPeriodFromInput: document.getElementById('auditPeriodFromInput'),
		auditPeriodToInput: document.getElementById('auditPeriodToInput'),
		uploader: document.getElementById('uploader')
	},
	data: {
		searchData: {
			name: null,
			startDate: null,
			endDate: null
		},
		dataTable: null,
		columns: [
			{ data: 'smallBusinessSubjectName', title: "Проверяемый СМП", width: "25%" },
			{ data: 'supervisoryAuthorityName', title: "Контролирующий орган", width: "25%" },
			{ title: "Плановый период проверки", render: function (data, type, row) { return `${row.beginAuditDate} - ${row.endAuditDate}`; }, width: "25%" },
			{ data: 'auditDuration', title: "Плановая длительность", width: "25%" }
		]
	},
	init: function () {
		page.elements.addButton.addEventListener("click", page.eventHandlers.onAddButtonClick, false);
		page.elements.editButton.addEventListener("click", page.eventHandlers.onEditButtonClick, false);
		page.elements.deleteButton.addEventListener("click", page.eventHandlers.onDeleteButtonClick, false);
		page.elements.importButton.addEventListener("click", page.eventHandlers.onImportButtonClick, false);
		page.elements.exportButton.addEventListener("click", page.eventHandlers.onExportButtonClick, false);
		page.elements.findButton.addEventListener("click", page.eventHandlers.onFindButtonClick, false);
		var fromDatepicker = datepicker(page.elements.auditPeriodFromInput, config.datepickerOption);
		var toDatepicker = datepicker(page.elements.auditPeriodToInput, config.datepickerOption);
		fromDatepicker.onSelect = (instance, date) => {
			toDatepicker.setMin(date);
			page.elements.auditPeriodToInput.value = "";
		}
		page.data.dataTable = dataTable.createTable(page.elements.table, "/Register/GetSmallBusinessSubjectAuditDataTable", page.data.columns, function (data) {
			return Object.assign({}, data, page.data.searchData);
		});
		page.data.dataTable.onRowSelect = page.eventHandlers.onRowSelect;
		FilePond.registerPlugin(FilePondPluginFileValidateType);
		$(page.elements.uploader).filepond({
			allowMultiple: false,
			instantUpload: true,
			maxFiles: 1,
			maxParallelUploads: 1,
			acceptedFileTypes: ['application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'],
			name: "file",
			server: "/Register/ImportSmallBusinessSubjectAudit",
			onprocessfiles: function () {
				$(page.elements.uploader).filepond("removeFiles");
				page.methods.reloadTable();
			} 
		});
	},
	eventHandlers: {
		onRowSelect: function () {
			if (page.data.dataTable.selectedRowsCount > 0) {
				if (page.data.dataTable.selectedRowsCount === 1) {
					page.elements.editButton.classList.remove("d-none");
				} else {
					page.elements.editButton.classList.add("d-none");
				}
				page.elements.deleteButton.classList.remove("d-none");
			} else {
				page.elements.editButton.classList.add("d-none");
				page.elements.deleteButton.classList.add("d-none");
			}
		},
		onAddButtonClick: function () {
			auditModal.showAddModal();
		},
		onEditButtonClick: function () {
			var editId = page.data.dataTable.rows('.selected').data()[0].id;
			auditModal.showEditModal(editId);
		},
		onDeleteButtonClick: function () {
			var deleteIds = page.data.dataTable.rows('.selected').data().map(x => x.id).toArray();
			page.methods.deleteSmallBusinessSubjectAudit(deleteIds);
		},
		onImportButtonClick: function () {
			$(page.elements.uploader).filepond('browse');
		},
		onExportButtonClick: function () {
			page.methods.exportFile(0);
		},
		onFindButtonClick: function () {
			page.data.searchData.name = page.elements.nameInput.value;
			page.data.searchData.startDate = page.elements.auditPeriodFromInput.value;
			page.data.searchData.endDate = page.elements.auditPeriodToInput.value;
			page.methods.reloadTable();
		}
	},
	methods: {
		deleteSmallBusinessSubjectAudit: function(ids) {
			$.post("/Register/DeleteSmallBusinessSubjectAudits", { ids: ids })
				.done(function (data) {
					page.data.dataTable.ajax.reload();
				});
		},
		reloadTable: function() {
			page.elements.editButton.classList.add("d-none");
			page.elements.deleteButton.classList.add("d-none");
			page.data.dataTable.ajax.reload();
		},
		exportFile: function (type) {
			var params = $.param(page.data.searchData);
			window.location.href = `/Register/ExportSmallBusinessSubjectAudit?type=${type}&${params}`;
		}
	}
}
page.init();