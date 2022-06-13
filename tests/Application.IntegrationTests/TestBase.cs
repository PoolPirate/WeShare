using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WeShare.Application.Services;
using WeShare.Domain.Entities;
using static WeShare.Application.IntegrationTests.Testing;

namespace WeShare.Application.IntegrationTests;
public class TestBase
{
    [SetUp]
    public async Task TestSetUp() => await ResetState();

    public static async Task<UserId> RunAsUserAsync()
    {
        var secretService = ScopeFactory.CreateScope().ServiceProvider.GetService<ISecretService>()!;
        var testUser = User.Create(Username.From("playwo"), "playwo@playwo.de",
            secretService.HashPassword(PlainTextPassword.From("1234ABCDabcd!")), Nickname.From(String.Empty));
        await AddAsync(testUser);
        SetCurrentUser(testUser.Id);
        return testUser.Id;
    }
}
