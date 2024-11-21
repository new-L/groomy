using UnityEngine;
using System;
public class URLs
{
    #region UsersURLs
    private static string _authorization = "https://unnamedplace.ru/dbhelper/authorization.php";
    private static string _userCurrency = "https://unnamedplace.ru/dbhelper/usercurrency.php";

    public static string Authorization { get => _authorization; set => _authorization = value; }
    public static string UserCurrency { get => _userCurrency; set => _userCurrency = value; }

    #endregion

    #region AdminsURLs
    #region Tasks
    private static string _taskSettings = "https://unnamedplace.ru/dbhelper/tasksettings.php";
    private static string _newTask = "https://unnamedplace.ru/dbhelper/newtask.php";
    private static string _updateTask = "https://unnamedplace.ru/dbhelper/updatetask.php";
    private static string _deleteTask = "https://unnamedplace.ru/dbhelper/deletetask.php";
    private static string _scheduleTask = "https://unnamedplace.ru/dbhelper/scheduletask.php";
    private static string _dailyTask = "https://unnamedplace.ru/dbhelper/dailytasks.php";
    private static string _deScheduleTask = "https://unnamedplace.ru/dbhelper/descheduletask.php";
    private static string _taskOnReview = "https://unnamedplace.ru/dbhelper/taskonreview.php";
    #endregion

    #region Users
    private static string _users = "https://unnamedplace.ru/dbhelper/admin_userssettings.php";
    private static string _userCompletedTask = "https://unnamedplace.ru/dbhelper/usertask.php";
    #endregion

    #region Get/Set
    public static string TaskSettings { get => _taskSettings; set => _taskSettings = value; }
    public static string NewTask { get => _newTask; set => _newTask = value; }
    public static string UpdateTask { get => _updateTask; set => _updateTask = value; }
    public static string DeleteTask { get => _deleteTask; set => _deleteTask = value; }
    public static string ScheduleTask { get => _scheduleTask; set => _scheduleTask = value; }
    public static string DeScheduleTask { get => _deScheduleTask; set => _deScheduleTask = value; }
    public static string Users { get => _users; set => _users = value; }
    public static string DailyTask { get => _dailyTask; set => _dailyTask = value; }
    public static string UserCompletedTask { get => _userCompletedTask; set => _userCompletedTask = value; }
    public static string TaskOnReview { get => _taskOnReview; set => _taskOnReview = value; }
    #endregion

    #endregion
}
