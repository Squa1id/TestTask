var auditModal = {
	elements: {
		modal: document.getElementById('editModal'),
		modalTitle: document.getElementById('modalTitle'),
		confirmAuditButton: document.getElementById('confirmAuditButton'),
		smallBusinessSubjectSelect: document.getElementById('smallBusinessSubjectSelect'),
		supervisoryAuthoritySelect: document.getElementById('supervisoryAuthoritySelect'),
		auditPeriodFromModalInput: document.getElementById('auditPeriodFromModalInput'),
		auditPeriodToModalInput: document.getElementById('auditPeriodToModalInput'),
		auditDurationModalInput: document.getElementById('auditDurationModalInput')
	},
	data: {
		auditId: null
	},
	init: function () {
		auditModal.elements.confirmAuditButton.addEventListener("click", auditModal.eventHandlers.onClickConfirmButton, false);
		auditModal.elements.auditDurationModalInput.addEventListener("keypress", auditModal.eventHandlers.onAuditDurationModalInputKeyPress, false);
		$(auditModal.elements.modal).on('hidden.bs.modal', auditModal.eventHandlers.onModalHidden);
		auditModal.showAddModal = auditModal.methods.showAddModal;
		auditModal.showEditModal = auditModal.methods.showEditModal;
		var fromDatepicker = datepicker(auditModal.elements.auditPeriodFromModalInput, config.datepickerOption);
		var toDatepicker = datepicker(auditModal.elements.auditPeriodToModalInput, config.datepickerOption);
		fromDatepicker.onSelect = (instance, date) => {
			toDatepicker.setMin(date);
			auditModal.elements.auditPeriodToModalInput.value = "";
		}
		auditModal.methods.completeSelects();
	},
	eventHandlers: {
		onAuditDurationModalInputKeyPress: function (e) {
			if (e.which === 45) {
				e.preventDefault();
			}
		},
		onClickConfirmButton: function () {
			auditModal.methods.saveData();
		},
		onModalHidden: function () {
			auditModal.methods.clearForm();
		}
	},
	methods: {
		checkData: function () {
			if (auditModal.elements.smallBusinessSubjectSelect.value !== "" &&
				auditModal.elements.supervisoryAuthoritySelect.value !== "" &&
				auditModal.elements.auditPeriodFromModalInput.value !== "" &&
				auditModal.elements.auditPeriodToModalInput.value !== "" &&
				auditModal.elements.auditDurationModalInput.value !== "") return true;
			return false;
		},
		saveData: function () {
			if (auditModal.methods.checkData()) {
				var model = {
					id: auditModal.data.auditId,
					smallBusinessSubjectId: auditModal.elements.smallBusinessSubjectSelect.value,
					supervisoryAuthorityId: auditModal.elements.supervisoryAuthoritySelect.value,
					beginAuditDate: auditModal.elements.auditPeriodFromModalInput.value,
					endAuditDate: auditModal.elements.auditPeriodToModalInput.value,
					auditDuration: auditModal.elements.auditDurationModalInput.value
				}
				$.post("/Register/SaveSmallBusinessSubjectAudit", { model: model }).done(function () {
					page.methods.reloadTable();
					auditModal.methods.hideEditModal();
				});
			} else {
				alert("Проверьте данные!");
			}
		},
		loadData: function (id) {
			$.post("/Register/GetSmallBusinessSubjectAudit", { id : id}).done(function (data) {
				auditModal.elements.smallBusinessSubjectSelect.value = data.smallBusinessSubjectId;
				auditModal.elements.supervisoryAuthoritySelect.value = data.supervisoryAuthorityId;
				auditModal.elements.auditPeriodFromModalInput.value = data.beginAuditDate;
				auditModal.elements.auditPeriodToModalInput.value = data.endAuditDate;
				auditModal.elements.auditDurationModalInput.value = data.auditDuration;
			});
		},
		showAddModal: function () {
			auditModal.elements.modalTitle.innerText = "Добавление проверки";
			$(auditModal.elements.modal).modal('show');
		},
		showEditModal: function (id) {
			auditModal.data.auditId = id;
			auditModal.methods.loadData(id);
			auditModal.elements.modalTitle.innerText = "Редактирование проверки";
			$(auditModal.elements.modal).modal('show');
		},
		hideEditModal: function () {
			$(auditModal.elements.modal).modal('hide');
		},
		completeSelects: function () {
			$.post("/Register/GetSmallBusinessSubjects").done(function (data) {
					auditModal.methods.completeSelect(smallBusinessSubjectSelect, data);
				});
			$.post("/Register/GetSupervisoryAuthorities").done(function (data) {
				auditModal.methods.completeSelect(supervisoryAuthoritySelect, data);
				});
		},
		completeSelect: function (select, options) {
			for (var i = 0; i < options.length; i++) {
				var opt = document.createElement('option');
				opt.value = options[i].key;
				opt.innerHTML = options[i].text;
				select.appendChild(opt);
			}
		},
		clearForm: function () {
			auditModal.elements.smallBusinessSubjectSelect.value = null;
			auditModal.elements.supervisoryAuthoritySelect.value = null;
			auditModal.elements.auditPeriodFromModalInput.value = null;
			auditModal.elements.auditPeriodToModalInput.value = null;
			auditModal.elements.auditDurationModalInput.value = 1;
			auditModal.data.auditId = null;
		}
	}
}
auditModal.init();