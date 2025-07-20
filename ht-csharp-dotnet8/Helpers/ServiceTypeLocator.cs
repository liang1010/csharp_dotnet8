using System.Reflection;

namespace ht_csharp_dotnet8.Helpers
{
    public static class ServiceTypeLocator
    {
        private static bool isInitialized = false;

        private static List<Type> AllTypes;

        private static List<Type> AllContracts;

        private static List<Type> AllTransactions;

        private static List<Type> AllNHibernate;

        private static Dictionary<Type, Type> ServiceDependencies;

        private static void LocateAllTypes()
        {
            if (isInitialized) return;
            try
            {
                AllTypes = new List<Type>();
                AllContracts = new List<Type>();
                AllTransactions = new List<Type>();
                AllNHibernate = new List<Type>();
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var serviceDll = Directory.GetFiles(path, "*.dll").ToList();
                foreach (string file in serviceDll)
                {
                    string fileNameOnly = Path.GetFileName(file);
                    if (fileNameOnly.ToUpper().Contains("Entity".ToUpper()))
                    {
                        continue;
                    }
                    try
                    {
                        string asmFile = Path.Combine(path, fileNameOnly);

                        Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic).FirstOrDefault(n => n.Location == asmFile);
                        if (assembly == null)
                        {
                            assembly = Assembly.Load(AssemblyName.GetAssemblyName(asmFile));
                        }
                        string strNamespace = assembly.ManifestModule.Name.Substring(0, assembly.ManifestModule.Name.Length - 4);
                        AllTypes.AddRange(assembly.GetTypes().ToList());
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                AllContracts.AddRange(AllTypes.Where(x => x.IsInterface));
                //AllTransactions.AddRange(AllTypes.Where(x => x.GetInterfaces().Contains(typeof(IAbstractBase))));
                //AllNHibernate.AddRange(AllTypes.Where(t => t.GetCustomAttributes(typeof(NHibernateServiceAttribute), true).FirstOrDefault() != null));
                isInitialized = true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public static List<Type> GetAllTypes()
        {
            if (!isInitialized)
            {
                LocateAllTypes();
            }
            return AllTypes;
        }

        public static List<Type> GetAllNHibernate()
        {
            if (!isInitialized)
            {
                LocateAllTypes();
            }
            return AllNHibernate;
        }


        public static List<Type> GetAllContracts()
        {
            if (!isInitialized)
            {
                LocateAllTypes();
            }
            return AllContracts;
        }

        public static List<Type> GetAllTransactions()
        {
            if (!isInitialized)
            {
                LocateAllTypes();
            }
            return AllTransactions;
        }

        public static Type GetTypeByName(string ContractOrClassName)
        {
            try
            {
                if (isInitialized == false)
                    LocateAllTypes();
                var type = AllNHibernate.FirstOrDefault(c => c.Name == ContractOrClassName || c.FullName == ContractOrClassName || c.Name == "I" + ContractOrClassName.Split('.').LastOrDefault());
                if (type == null)
                {
                    type = AllTransactions.FirstOrDefault(c => c.Name == ContractOrClassName || c.FullName == ContractOrClassName || c.Name == "I" + ContractOrClassName.Split('.').LastOrDefault());
                }
                return type;

            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
