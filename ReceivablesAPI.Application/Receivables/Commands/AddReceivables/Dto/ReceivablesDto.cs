using System.Runtime.Serialization;

namespace ReceivablesAPI.Application.Receivables.Commands.AddReceivables.Dto;

[DataContract]
[Serializable]
public class ReceivablesDto
{
    public List<ReceivableDto> ReceivableList { get; set; } = new List<ReceivableDto>();
}

