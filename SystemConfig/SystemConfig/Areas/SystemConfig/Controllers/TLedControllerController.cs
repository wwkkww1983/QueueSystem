﻿using System.Web.Mvc;
using BLL;
using Model;
using Newtonsoft.Json;
using SystemConfig.Controllers;

namespace SystemConfig.Areas.SystemConfig.Controllers
{
    public class TLedControllerController : BaseController
    {
        TLedControllerBLL bll;
        TLedWindowBLL winBll;

        public TLedControllerController()
        {
            this.bll = new TLedControllerBLL("MySQL", this.AreaNo);
            this.winBll = new TLedWindowBLL("MySQL", this.AreaNo);
        }

        //
        // GET: /SystemConfig/TLedController/ 

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetGridData(Pagination p)
        {
            var data = new
            {
                rows = bll.GetGridData()
            };
            return Content(JsonConvert.SerializeObject(data));
        }

        public ActionResult Form(int id)
        {
            var model = this.bll.GetModel(id);
            if (model == null)
                model = new TLedControllerModel() { ID = -1 };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(TLedControllerModel model)
        {
            if (model.ID == -1)
                this.bll.Insert(model);
            else
                this.bll.Update(model);
            return Content("操作成功！");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(int id)
        {
            foreach (var win in this.winBll.GetModelList(p => p.ControllerID == id))
                this.winBll.Delete(win);
            this.bll.Delete(this.bll.GetModel(id));
            return Content("操作成功！");
        }

    }
}
