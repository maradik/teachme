﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeachMe.Models.Jobs
{
    public class Job : IEntity<Guid>
    {
        private const double MinCost = 100;
        private const double MaxCost = double.MaxValue;

        private List<JobAttachment> attachments;

        public Guid Id { get; set; }

        [Display(Name = "Предмет")]
        [Range(1, int.MaxValue, ErrorMessage = "Выберите значение из списка")]
        public int SubjectId { get; set; }

        [Display(Name = "Заголовок")]
        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        [StringLength(1000)]
        public string Description { get; set; }

        [Display(Name = "Статус")]
        public JobStatus Status { get; set; }

        public JobPaymentState PaymentState { get; set; }

        [Display(Name = "Стоимость")]
        [Range(MinCost, MaxCost, ErrorMessage = "Значение поля {0} должно быть не менее {1}")]
        public double StudentCost { get; set; }

        [Display(Name = "Предоплата")]
        public double StudentPrepaymentAmount { get; set; }

        [Display(Name = "Стоимость")]
        public double TeacherCost => StudentCost - Commission;

        [Display(Name = "Предоплата")]
        public double TeacherPrepaymentAmount => StudentPrepaymentAmount - PrepaymentCommission;

        [Display(Name = "Ставка комиссии")]
        public double CommissionRate { get; set; }

        [Display(Name = "Комиссия")]
        public double Commission => Math.Round(StudentCost * CommissionRate);

        [Display(Name = "Комиссия по предоплате")]
        public double PrepaymentCommission => Math.Round(StudentPrepaymentAmount * CommissionRate);

        [DisplayName("Фото, документы")]
        public List<JobAttachment> Attachments { get { return attachments ?? (attachments = new List<JobAttachment>()); } set { attachments = value; } }

        [DisplayName("Исполнитель")]
        public string TeacherUserId { get; set; }

        [DisplayName("Заказчик")]
        public string StudentUserId { get; set; }

        [DisplayName("Срок исполнения")]
        public long DeadlineTicks { get; set; }

        [DisplayName("Дата создания")]
        public long CreationTicks { get; set; }
    }
}