using NUnit.Framework;

using static WeShare.Application.IntegrationTests.Testing;

namespace WeShare.Application.IntegrationTests;
public class TestBase
{
    [SetUp]
    public async Task TestSetUp() => await ResetState();
}
