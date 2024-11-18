using System;
using System.Security.Cryptography;
using System.Text;
using backend.Data.Extentions;
using backend.Dtos.Payment;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
namespace backend.Services;

public class PaymentService : IPaymentService
{
    IOptions<PaymentSetting> _settings;

    public PaymentService(IOptions<PaymentSetting> settings)
    {
        _settings = settings;
    }

    public async Task<PaymentResponse> CreatePayment(PaymentCreateRequest request)
    {
        var requestId = Guid.NewGuid();
        request.OrderInfo = "Khách hàng: " + request.FullName + " đã đặt hàng với mã đơn hàng " + request.OrderId + " với số tiền "
            + request.Amount + ". Thông tin đơn hàng: " + request.OrderInfo;
        var returnUrl = _settings.Value.ReturnUrl;
        var rawData =
            $"partnerCode={_settings.Value.PartnerCode}" +
            $"&accessKey={_settings.Value.AccessKey}" +
            $"&requestId={requestId}" +
            $"&amount={request.Amount}" +
            $"&orderId={request.OrderId}" +
            $"&orderInfo={request.OrderInfo}" +
            $"&returnUrl={returnUrl}" +
            $"&notifyUrl={_settings.Value.NotifyUrl}" +
            $"&extraData=";
        var signature = ComputeHmacSha256(rawData, _settings.Value.SecretKey);
        var restClient = new RestClient(_settings.Value.Endpoint);
        var restRequest = new RestRequest()
        {
            Method = Method.Post
        };
        restRequest.AddHeader("Content-Type", "application/json; charset=UTF-8");
        var requestData = new
        {
            accessKey = _settings.Value.AccessKey,
            partnerCode = _settings.Value.PartnerCode,
            requestType = _settings.Value.RequestType,
            notifyUrl = _settings.Value.NotifyUrl,
            returnUrl = returnUrl,
            orderId = request.OrderId.ToString(),
            amount = request.Amount.ToString(),
            orderInfo = request.OrderInfo,
            requestId = requestId.ToString(),
            extraData = "",
            signature = signature
        };
        restRequest.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);
        var response = await restClient.ExecuteAsync(restRequest);
        return JsonConvert.DeserializeObject<PaymentResponse>(response.Content);
    }


    private string ComputeHmacSha256(string rawData, string secretKey)
    {
        using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
        {
            byte[] hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}



