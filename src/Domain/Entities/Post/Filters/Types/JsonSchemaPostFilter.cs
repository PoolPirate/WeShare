namespace WeShare.Domain.Entities;
public class JsonSchemaPostFilter :  PostFilter
{
    public string Schema { get; set; }

    public JsonSchemaPostFilter(ShareId shareId, PostFilterName name, string schema)
        :base(shareId, PostFilterType.JsonSchema, name)
    {
        Schema = schema;
    }
}
