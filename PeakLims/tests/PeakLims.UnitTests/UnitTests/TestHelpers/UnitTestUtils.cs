namespace PeakLims.UnitTests.UnitTests.TestHelpers;

using System.Reflection;
using Mapster;
using MapsterMapper;

public class UnitTestUtils
{
    public static Mapper GetApiMapper()
    {
        var apiAssembly = GetAssemblyByName("PeakLims");
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings.Clone();
        typeAdapterConfig.Scan(apiAssembly);
        var mapper = new Mapper(typeAdapterConfig);
        return mapper;
    }

    private static Assembly GetAssemblyByName(string name)
    {
        return AppDomain.CurrentDomain.GetAssemblies().
            SingleOrDefault(assembly => assembly.GetName().Name == name);
    }
}
