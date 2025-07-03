using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WerkstattlagerViewLogic;
using Xunit.Abstractions;

namespace WerkstattlagerIntegration.Tests;

public abstract class TestBase
{
    protected IServiceProvider RootServiceProvider { get; init; }

    public TestBase(ITestOutputHelper testOutputHelper)
    {
        var services = new ServiceCollection();
        services.AddWerkstattlager();
        services.AddSingleton<DatabaseFixture>();
        services.AddLogging(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
            builder.AddTestOutputLogger(c => c.TestOutputHelper = testOutputHelper);
        });

        RootServiceProvider = services.BuildServiceProvider();
    }
}
