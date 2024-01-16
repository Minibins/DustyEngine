using System;
using System.Reflection;

namespace DustyEngine
{
    public class Core
    {
        public Core()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
        
            Type[] types = assembly.GetTypes();
        
            foreach (Type type in types)
            {
                MethodInfo[] methods = type.GetMethods();
            
                foreach (MethodInfo method in methods)
                {
                    
                    if (method.Name == "Start")
                    {
                        var instance = Activator.CreateInstance(type);
                        method.Invoke(instance, null);
                    }
                    if (method.Name == "Update")
                    {
                        while (true)
                        {
                            var instance = Activator.CreateInstance(type);
                            method.Invoke(instance, null);
                        }
                    }
                }
            }
        }
    }
}