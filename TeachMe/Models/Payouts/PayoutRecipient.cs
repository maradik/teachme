using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeachMe.Models.Payouts
{
    public class PayoutRecipient
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\+7[\d]{10}$", ErrorMessage = "Введите номер телефона в формате +7xxxxxxxxxx")]
        [DisplayName("QIWI-кошелек (номер телефона)")]
        public string QiwiPhoneNumber { get; set; }
    }
}