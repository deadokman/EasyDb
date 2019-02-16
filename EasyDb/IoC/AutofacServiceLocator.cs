namespace EasyDb.IoC
{
    using System;
    using System.Collections.Generic;
    using Autofac;
    using CommonServiceLocator;

    /// <summary>
    /// Defines the <see cref="AutofacServiceLocator" />
    /// </summary>
    public class AutofacServiceLocator : IServiceLocator
    {
        /// <summary>
        /// Locator instance
        /// </summary>
        private static AutofacServiceLocator instance;

        /// <summary>
        /// Autofac container builder
        /// </summary>
        private readonly ContainerBuilder _containerBuilder;

        /// <summary>
        /// Container instance
        /// </summary>
        private IContainer _container;

        /// <summary>
        /// Prevents a default instance of the <see cref="AutofacServiceLocator"/> class from being created.
        /// </summary>
        private AutofacServiceLocator()
        {
            this._containerBuilder = new ContainerBuilder();
        }

        /// <summary>
        /// Gets the Instance
        /// Returns instance of the Autofac service locator
        /// </summary>
        public static AutofacServiceLocator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AutofacServiceLocator();
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets Autofac container builder
        /// </summary>
        /// <returns>Returns container builder instance</returns>
        public ContainerBuilder GetBuilder()
        {
            return this._containerBuilder;
        }

        /// <summary>
        /// Build IoC container
        /// </summary>
        public void ActivateIoc()
        {
            this._container = this._containerBuilder.Build();
        }

        /// <summary>
        /// The GetAllInstances
        /// </summary>
        /// <param name="serviceType">The serviceType<see cref="Type"/></param>
        /// <returns>The </returns>
        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The GetAllInstances
        /// </summary>
        /// <typeparam name="TService">Service type</typeparam>
        /// <returns>The <see cref="IEnumerable{TService}"/></returns>
        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return this._container.Resolve<IEnumerable<TService>>();
        }

        /// <summary>
        /// The GetInstance
        /// </summary>
        /// <param name="serviceType">The serviceType<see cref="Type"/></param>
        /// <returns>The <see cref="object"/></returns>
        public object GetInstance(Type serviceType)
        {
            return this._container.ResolveOptional(serviceType);
        }

        /// <summary>
        /// The GetInstance
        /// </summary>
        /// <param name="serviceType">The serviceType<see cref="Type"/></param>
        /// <param name="key">The key<see cref="string"/></param>
        /// <returns>The <see cref="object"/></returns>
        public object GetInstance(Type serviceType, string key)
        {
            return this._container.ResolveKeyed(key, serviceType);
        }

        /// <summary>
        /// The GetInstance
        /// </summary>
        /// <typeparam name="TService">Service type</typeparam>
        /// <returns>Service type2</returns>
        public TService GetInstance<TService>()
        {
            return this._container.Resolve<TService>();
        }

        /// <summary>
        /// The GetInstance
        /// </summary>
        /// <typeparam name="TService">Service type</typeparam>
        /// <param name="key">The key<see cref="string"/>Service key</param>
        /// <returns>Service type3</returns>
        public TService GetInstance<TService>(string key)
        {
            return this._container.ResolveKeyed<TService>(key);
        }

        /// <summary>
        /// The GetService
        /// </summary>
        /// <param name="serviceType">The serviceType<see cref="Type"/></param>
        /// <returns>The <see cref="object"/></returns>
        public object GetService(Type serviceType)
        {
            return this._container.Resolve(serviceType);
        }
    }
}
