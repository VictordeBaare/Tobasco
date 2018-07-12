using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;

namespace TobascoTest.GeneratedEntity
{
	public class EntityBase : INotifyPropertyChanged
	{
		public long Id { get; set; }

		public string ModifiedBy { get; set; }
		public DateTime ModifiedOn { get; set; }

		public DateTime MutatieDatumTijd { get; set; }

		public byte[] RowVersion { get; set; }

		public bool IsDirty { get; private set; }

		public bool IsNew { get { return Id == 0; } }

		public virtual bool IsDeleted { get; protected set; } = false;

		public event PropertyChangedEventHandler PropertyChanged;

		protected void SetField<T>(ref T field, T value, string propertyName)
		{
			if (!EqualityComparer<T>.Default.Equals(field, value))
			{
				field = value;
				IsDirty = true;
				OnPropertyChanged(propertyName);
			}
		}

		public virtual ExpandoObject ToAnonymous() { return new ExpandoObject(); }

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void MarkOld()
		{
			IsDirty = false;
		}

		public void MarkDeleted()
		{
			IsDeleted = true;
		}

		public virtual IEnumerable<EntityBase> GetChildren()
		{
			return Enumerable.Empty<EntityBase>();
		}
	}
}
