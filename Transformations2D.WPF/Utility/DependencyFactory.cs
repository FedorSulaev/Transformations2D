using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Transformations2D.WPF.Helpers;

namespace Transformations2D.WPF.Utility
{
	public class DependencyFactory
	{
		private static IUnityContainer _container;

		public static IUnityContainer Container
		{
			get { return _container; }
			private set { _container = value; }
		}

		static DependencyFactory()
		{
			IUnityContainer container = new UnityContainer();
			UnityConfigurationSection section = (UnityConfigurationSection) ConfigurationManager.GetSection("unity");
			if (section != null)
			{
				section.Configure(container);
			}
			_container = container;
		}

		public static T Resolve<T>()
		{
			T ret = default(T);
			if (Container.IsRegistered(typeof (T)))
			{
				ret = Container.Resolve<T>();
			}
			return ret;
		}
	}
}
