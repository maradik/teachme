using System;
using System.Web.Mvc;
using log4net;
using TeachMe.DataAccess;
using TeachMe.DataAccess.Payouts;
using TeachMe.Models.Payouts;
using TeachMe.Models.Users;
using TeachMe.ProjectsSupport;
using TeachMe.Services.General;
using TeachMe.Services.Payouts;
using TeachMe.Services.UserCasheSupport;
using TeachMe.ViewModels.Payouts;

namespace TeachMe.Controllers
{
    [Authorize(Roles = UserRole.Names.Teacher + "," + UserRole.Names.Admin)]
    public class PayoutController : ControllerBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PayoutController));

        private readonly IPayoutRepository payoutRepository;
        private readonly IPayoutActionService payoutActionService;

        public PayoutController(IProjectTypeProvider projectTypeProvider,
                                IProjectInfoProvider projectInfoProvider,
                                IPayoutRepository payoutRepository,
                                IPayoutActionService payoutActionService)
            : base(projectTypeProvider, projectInfoProvider)
        {
            this.payoutRepository = payoutRepository;
            this.payoutActionService = payoutActionService;
        }

        public ActionResult Index(bool justCreated = false)
        {
            var viewModel = new PayoutIndexViewModel
            {
                JustCreated = justCreated,
                Payouts = payoutRepository.Get(x => x.UserId == ApplicationUser.Id, x => x.CreationTicks, SortOrder.Descending)
            };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            var payout = new Payout();
            payout.Amount = ApplicationUser.Cash.AvailableAmount;
            payout.Recipient.QiwiPhoneNumber = ApplicationUser.PhoneNumber;
            return View(payout);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Payout payout)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    payoutActionService.CreatePayout(payout.Amount, payout.Recipient.QiwiPhoneNumber, ApplicationUser);
                    return RedirectToAction("Index", new {justCreated = true});
                }
            }
            catch (InvalidPayoutAmountException e)
            {
                ModelState.AddModelError(nameof(Payout.Amount), e.Message);
            }
            catch (Exception e)
            {
                Logger.Error("Не удалось создать выплату", e);
            }

            return View(payout);
        }

        [Authorize(Roles = UserRole.Names.Admin)]
        public ActionResult Perform(Guid id)
        {
            var payout = payoutRepository.Get(id);
            return View(new PayoutPerformViewModel(payout));
        }

        [Authorize(Roles = UserRole.Names.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Perform(Guid id, PayoutPerformViewModel viewModel)
        {
            try
            {
                payoutActionService.PerformPayout(id);
                return RedirectToAction("IndexAll");
            }
            catch (PayoutActionException e)
            {
                ModelState.AddModelError("", e.Message);
            }
            catch (Exception e)
            {
                Logger.Error("Не удалось изменить выплату", e);
                throw;
            }

            return View(viewModel);
        }

        [Authorize(Roles = UserRole.Names.Admin)]
        public ActionResult Discard(Guid id)
        {
            var payout = payoutRepository.Get(id);
            return View(new PayoutDiscardViewModel(payout));
        }

        [Authorize(Roles = UserRole.Names.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Discard(Guid id, PayoutDiscardViewModel viewModel)
        {
            try
            {
                payoutActionService.DiscardPayout(id, viewModel.AdminComment);
                return RedirectToAction("IndexAll");
            }
            catch (PayoutActionException e)
            {
                ModelState.AddModelError("", e.Message);
            }
            catch (Exception e)
            {
                Logger.Error("Не удалось изменить выплату", e);
                throw;
            }

            return View(viewModel);
        }

        [Authorize(Roles = UserRole.Names.Admin)]
        public ActionResult IndexAll(int limit = 100)
        {
            var payouts = payoutRepository.Get(orderByExpression: x => x.CreationTicks, sortOrder: SortOrder.Descending, limit: limit);
            return View(payouts);
        }
    }
}