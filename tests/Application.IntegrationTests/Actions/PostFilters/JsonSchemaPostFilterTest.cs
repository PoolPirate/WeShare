using FluentAssertions;
using NUnit.Framework;
using System.Text;
using WeShare.Application.Actions.Commands;
using WeShare.Domain.Entities;
using static WeShare.Application.IntegrationTests.Testing;

namespace WeShare.Application.IntegrationTests;
public class JsonSchemaPostFilterTest : TestBase
{
    private readonly string TestJsonSchema = File.ReadAllText("Actions\\PostFilters\\nameschema.json");

    private string TestJson = "{\"firstName\": \"John\",\"lastName\": \"Doe\",\"age\": 21}";
    private string InvalidTestJson = "{\"firstName\": \"John\",\"lastName\": \"Doe\",\"age\": -1}";

    [Test]
    public async Task TestValidSchema()
    {
        var userId = await RunAsUserAsync();

        var createShareResult = await SendAsync(new ShareCreateAction.Command(
            userId, ShareName.From("MyShare01"), false, "1234", "1234"));

        var shareId = createShareResult.ShareId!.Value;
        var share = (await FindAsync<Share>(shareId))!;

        await SendAsync(new JsonSchemaPostFilterCreateAction.Command(
            shareId, PostFilterName.From("SchemaFilter"), TestJsonSchema));

        var headers = new Dictionary<string, string[]>
        {
            { "Content-Type", new[] { "application/json" } }
        };

        var payload = new MemoryStream(Encoding.UTF8.GetBytes(TestJson));

        var submitResult = (await SendAsync(new PostSubmitAction.Command(
            share.Secret, headers, payload)))!;

        submitResult.Status.Should().Be(PostSubmitAction.Status.Success);

        payload = new MemoryStream(Encoding.UTF8.GetBytes(InvalidTestJson));

        submitResult = (await SendAsync(new PostSubmitAction.Command(
            share.Secret, headers, payload)))!;

        submitResult.Status.Should().Be(PostSubmitAction.Status.FilterValidationFailed);
    }
}
