namespace TeachMe.Models.Jobs
{
    public enum JobActionType
    {
        [HumanAnnotation("Скрыть")]
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

        [HumanAnnotation("Изменить")]
        Edit = 9,

        [HumanAnnotation("Удалить")]
        Delete = 10,

        [HumanAnnotation("Не отменять")]
        RejectAbort = 11
    }
}