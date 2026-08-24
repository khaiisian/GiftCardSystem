using GiftCardSystem.Repository.PaymentMethods;
using GiftCardSystem.Service.PaymentMethods.Dtos;

namespace GiftCardSystem.Service.PaymentMethods;

public class PaymentMethodService
{
    private readonly IPaymentMethodRepository _repo;

    public PaymentMethodService(IPaymentMethodRepository repo) => _repo = repo;

    public async Task<List<PaymentMethodDto>> GetAllAsync()
    {
        var methods = await _repo.GetAllAsync();
        return methods.Select(x => new PaymentMethodDto
        {
            PaymentMethodId = x.PaymentMethodId,
            Name = x.Name,
            Description = x.Description
        }).ToList();
    }
}
