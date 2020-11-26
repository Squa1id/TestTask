using System;
using System.Collections.Generic;
using TestTask.Data.Enums;

namespace TestTask.Data.ViewModels {
	public class DataTableResponseModel<TViewModel> {
		public int draw { get; set; } = 0;
		public int recordsTotal { get; set; } = 0;
		public int recordsFiltered { get; set; } = 0;
		public IEnumerable<TViewModel> data { get; set; } = new List<TViewModel>();
		public string error { get; set; } = string.Empty;
		public ResultState State { get; set; } = ResultState.Success;

		public static DataTableResponseModel<TViewModel> ErrorResponse(Exception exc) {
			return new DataTableResponseModel<TViewModel> {
				State = ResultState.Success,
				error = exc.Message
			};
		}
	}
}
