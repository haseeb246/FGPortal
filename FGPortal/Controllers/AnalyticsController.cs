using FGPortal.Models;
using FGPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FGPortal.Controllers
{
    public class AnalyticsController : BaseController
    {
        public AnalyticsController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public ActionResult Index()
		{
			return (ActionResult)(object)((Controller)this).RedirectToAction("Charts");
		}

		public ActionResult Charts()
		{
			return (ActionResult)(object)((Controller)this).View();
		}

		public ActionResult AdHoc()
		{
			return (ActionResult)(object)((Controller)this).View();
		}

		[HttpPost]
		public ActionResult GenerateAdHocData(AdHocModel model)
		{
			try
			{
				AppDbContext FGPortalEntities = new AppDbContext();
				try
				{
					((DbContext)FGPortalEntities).Database.SetCommandTimeout((int?)300);
					List<SqlParameter> parameters = new List<SqlParameter>();
					parameters.Add(new SqlParameter("@CustomerID", base.CustomerId));
					parameters.Add(new SqlParameter("@StartDate", model.FromDate ?? DateTime.Today.Date.AddDays(-60.0)));
					parameters.Add(new SqlParameter("@EndDate", model.ToDate ?? DateTime.Today.Date));

					model.Data = FGPortalEntities.SP_AdHoc_GetData_Result.FromSqlRaw("exec SP_AdHoc_GetData", parameters).ToList();
					//model.Data = ((IEnumerable<SP_AdHoc_GetData_Result>)FGPortalEntities.SP_AdHoc_GetData(base.CustomerId, model.FromDate ?? DateTime.Today.Date.AddDays(-60.0), model.ToDate ?? DateTime.Today.Date)).ToList();
				}
				finally
				{
					((IDisposable)FGPortalEntities)?.Dispose();
				}
			}
			catch (System.Exception ex)
			{
				//LogDataAccess.AddOrUpdateToDatabase(new Log
				//{
				//	Type = "Error",
				//	Details = $"{MethodBase.GetCurrentMethod().Name} - {ex.Message} --- {ex.StackTrace}",
				//	TimeStamp = DateTime.UtcNow
				//});
				base.ViewBag.Error = true;
			}
			return (ActionResult)(object)((Controller)this).View((object)model);
		}
	}
}
