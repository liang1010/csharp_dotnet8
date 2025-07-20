namespace ht_csharp_dotnet8.Attributes
{
    public enum ServiceDependenciesLifeCycle
    {
        /// <summary>
        /// singleton instance throughout app's life
        /// </summary>
        Singleton = 1,
        /// <summary>
        /// new instance per ctor injection
        /// </summary>
        Transient = 2,
        /// <summary>
        /// new instance per context (API context, per request)
        /// </summary>
        Scoped = 3
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ServiceDependenciesAttribute : Attribute
    {

        public ServiceDependenciesAttribute(ServiceDependenciesLifeCycle lifeCycle = ServiceDependenciesLifeCycle.Transient, bool skip = false)
        {
            LifeCycle = lifeCycle;
            this.Skip = skip;
        }

        public ServiceDependenciesLifeCycle LifeCycle { get; }

        public bool Skip { get; protected set; }
    }
}
