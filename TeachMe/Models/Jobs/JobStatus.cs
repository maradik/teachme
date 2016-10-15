namespace TeachMe.Models.Jobs
{
    public enum JobStatus
    {
        [HumanAnnotation("Черновик")]
        Draft = 0,

        [HumanAnnotation("Опубликовано")]
        Opened = 1,

        [HumanAnnotation("В работе")]
        InWorking = 2,

        [HumanAnnotation("Работа выполнена")]
        Finished = 3,

        [HumanAnnotation("Оплачено")]
        Accepted = 4,

        [HumanAnnotation("Отменено")]
        Cancelled = 5,

        [HumanAnnotation("В работе (повторно)")]
        InReWorking = 6,

        [HumanAnnotation("Предложена отмена")]
        AbortOffered = 7,

        [HumanAnnotation("Отменено")]
        Aborted = 8,

        [HumanAnnotation("В арбитраже")]
        InArbitrage = 9
    }
}