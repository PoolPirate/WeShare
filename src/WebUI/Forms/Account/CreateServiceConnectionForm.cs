using WeShare.Domain.Entities;

namespace WeShare.WebAPI.Forms;

public class CreateServiceConnectionForm
{
    public ServiceConnectionType Type { get; set; }
    public string Code { get; set; }
}
