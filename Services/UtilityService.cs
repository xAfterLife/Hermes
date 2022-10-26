using Hermes.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Hermes.Services;

public class UtilityService
{
    public static List<ISlashCommand> GetCommands()
    {
        var outList = new List<ISlashCommand>();

        try
        {
            foreach ( var type in typeof(ISlashCommand).Assembly.GetTypes()
                                                       .Where(myType =>
                                                       {
                                                           var fullName = typeof(ISlashCommand).FullName;
                                                           return fullName != null && myType.GetInterface(fullName) != null;
                                                       }) )
            {
                if ( Activator.CreateInstance(type) is not ISlashCommand obj )
                    continue;
                outList.Add(obj);
            }
        }
        catch ( Exception ex )
        {
            Console.WriteLine(ex);
            throw;
        }

        return outList;
    }

    public static List<IService> GetInitializableServices(IServiceProvider services)
    {
        var outList = new List<IService>();

        try
        {
            outList.AddRange(typeof(IService).Assembly.GetTypes()
                                             .Where(myType =>
                                             {
                                                 var fullName = typeof(IService).FullName;
                                                 return fullName != null && myType.GetInterface(fullName) != null;
                                             })
                                             .Select(services.GetRequiredService)
                                             .Cast<IService>());
        }
        catch ( Exception ex )
        {
            Console.WriteLine(ex);
            throw;
        }

        return outList;
    }
}
