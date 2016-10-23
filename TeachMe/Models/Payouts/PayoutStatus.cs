namespace TeachMe.Models.Payouts
{
    public enum PayoutStatus
    {
        [HumanAnnotation("Ожидает обработки")]
        Pending = 0,

        [HumanAnnotation("Выполнено")]
        Done = 1,

        [HumanAnnotation("Отменено")]
        Discarded = 2
    }
}