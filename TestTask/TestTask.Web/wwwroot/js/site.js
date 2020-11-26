var config = {
	datepickerOption: {
		formatter: (input, date) => {
			input.value = moment(date).format("DD.MM.YYYY");
		},
		startDay: 1,
		customDays: ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
		customMonths: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь']
	}
}