using System;

namespace TestTask.Data.ViewModels {
	public class PairResponseModel<TKey>
		where TKey : struct {
		/// <summary>
		/// Ключ
		/// </summary>
		public TKey Key { get; set; }
		/// <summary>
		/// Значение
		/// </summary>
		public string Text { get; set; }

		public PairResponseModel() { }
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="key">Ключ</param>
		/// <param name="value">Значение</param>
		public PairResponseModel(TKey key, string value) {
			Key = key;
			Text = value;
		}
	}
}
