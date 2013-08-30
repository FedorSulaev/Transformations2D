using System;
using System.ComponentModel;
using System.Linq.Expressions;
using NUnit.Framework;

namespace Transformations2D.WPF.UnitTests
{
	public static class NotifyPropertyChanged
	{
		public static NotifyExpectation<T> ShouldNotifyOn<T, TProperty>(this T owner,
			Expression<Func<T, TProperty>> propertyPicker) where T : INotifyPropertyChanged
		{
			return CreateExpectation(owner, propertyPicker, true);
		}

		public static NotifyExpectation<T> ShouldNotNotifyOn<T, TProperty>(this T owner,
			Expression<Func<T, TProperty>> propertyPicker) where T : INotifyPropertyChanged
		{
			return CreateExpectation(owner, propertyPicker, false);
		}

		private static NotifyExpectation<T> CreateExpectation<T, TProperty>(T owner,
			Expression<Func<T, TProperty>> pickProperty, bool eventExpected) where T : INotifyPropertyChanged
		{
			string propertyName = ((MemberExpression) pickProperty.Body).Member.Name;
			return new NotifyExpectation<T>(owner, propertyName, eventExpected);
		}
	}

	public class NotifyExpectation<T> where T : INotifyPropertyChanged
	{
		private readonly T _owner;
		private readonly string _propertyName;
		private readonly bool _eventExpected;

		public NotifyExpectation(T owner, string propertyName, bool eventExpected)
		{
			_owner = owner;
			_propertyName = propertyName;
			_eventExpected = eventExpected;
		}

		public void When(Action<T> action)
		{
			bool eventWasRaised = false;
			_owner.PropertyChanged += (sender, e) =>
			{
				if (e.PropertyName == _propertyName)
				{
					eventWasRaised = true;
				}
			};
			action(_owner);

			Assert.AreEqual(_eventExpected, eventWasRaised, "PropertyChanged on {0}", _propertyName);
		}
	}
}
