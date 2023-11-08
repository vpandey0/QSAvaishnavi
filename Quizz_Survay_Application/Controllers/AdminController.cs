﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quizz_Survay_Application.Models;
using Quizz_Survay_Application.Repository;

namespace Quizz_Survay_Application.Controllers
{
    public class AdminController : Controller
    {
        IQSARepository repoObj;
        string CurrUser;

        public AdminController()
        {
            repoObj = new QSARepository();
            CurrUser = "vpandey2606@gmail.com";
        }

        // GET: Admin
        public ActionResult Index()
        {
            repoObj.UpdateLog(CurrUser);
            return View();
        }

        public ActionResult GetAdminProfile()
        {
            return View(repoObj.GetUser(CurrUser));
        }

        public ActionResult GetAllUser()
        {
            return View(repoObj.GetAllUser());
        }

        [HttpPost]
        public void makeadmin(string UserToMakeAdmin)
        {
            repoObj.MakeAdmin(UserToMakeAdmin);
        }

        [HttpGet]
        public ActionResult Notifyadmin()
        {
            var assignmentsnotification = repoObj.notifyAssignments();
            return View(assignmentsnotification);
        }

        [HttpPost]
        public ActionResult editnotifiedassignment(NotifyAdminModel selected_assignment_by_user)
        {
            var QuestionsOfAssignmentByAdmin = repoObj.GetQuestionsOfAssignment(selected_assignment_by_user.As_Id);

            foreach (var item in QuestionsOfAssignmentByAdmin)
            {
                item.Options = repoObj.GetOptionsOfQuestion(item.Q_Id).ToList();
            }

            TempData["CurrAssignment"] = selected_assignment_by_user.As_Id;
            return View(QuestionsOfAssignmentByAdmin);
        }

        [HttpPost]
        public void UpdateAndSubmitAssignmentAdmin(IEnumerable<QuestionModel> res)
        {
            repoObj.UpdateAssignment(res);
            repoObj.PublishAssignment((int)TempData["CurrAssignment"]);
            return;
        }

        public int CountNotification()
        {
            return repoObj.CountNotifications();
        }

        [HttpPost]
        public void DeleteAssignmentAdmin(int id)
        {
            repoObj.DeleteAssignment(id);
            return;
        }
    }
}