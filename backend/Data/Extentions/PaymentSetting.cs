using System;

namespace backend.Data.Extentions;

public class PaymentSetting
{
    public string PartnerCode { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string Endpoint { get; set; }
    public string ReturnUrl { get; set; }
    public string NotifyUrl { get; set; }
    public string RequestType { get; set; }
}