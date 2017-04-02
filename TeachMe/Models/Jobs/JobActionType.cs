namespace TeachMe.Models.Jobs
{
    public enum JobActionType
    {
        [HumanAnnotation("Сделать черновиком")]
        Hide = 0,

        [HumanAnnotation("Опубликовать")]
        Open = 1,

        [HumanAnnotation("Взять в работу")]
        Take = 2,

        [HumanAnnotation("Работы выполнены")]
        Finish = 3,

        [HumanAnnotation("Принять работу")]
        Accept = 4,

        [HumanAnnotation("Отменить задачу")]
        Cancel = 5,

        [HumanAnnotation("На доработку")]
        Reject = 6,

        [HumanAnnotation("Отменить задачу")]
        OfferAbort = 7,

        [HumanAnnotation("Подтвердить отмену")]
        ConfirmAbort = 8,

        [HumanAnnotation("Не отменять")]
        RejectAbort = 9,

        [HumanAnnotation("Проверить результат")]
        ReserveRemainAmount = 10,

        [HumanAnnotation("Принять работу (без оплаты остатка)")]
        AcceptWithoutRemainAmount = 11,

        [HumanAnnotation("Изменить")]
        Edit = 100,

        [HumanAnnotation("Удалить")]
        Delete = 101
    }
}