using UnityEngine;
using System;
public class URLs
{
    #region UsersURLs
    private static string _authorization = "https://tracker.local/DBHandler/authorization.php";
    private static string _userCurrency = "https://tracker.local/DBHandler/usercurrency.php";

    public static string Authorization { get => _authorization; set => _authorization = value; }
    public static string UserCurrency { get => _userCurrency; set => _userCurrency = value; }

    #endregion

    #region AdminsURLs
    #region Tasks
    private static string _taskSettings = "https://tracker.local/DBHandler/tasksettings.php";
    private static string _newTask = "https://tracker.local/DBHandler/newtask.php";
    private static string _updateTask = "https://tracker.local/DBHandler/updatetask.php";
    private static string _deleteTask = "https://tracker.local/DBHandler/deletetask.php";
    private static string _scheduleTask = "https://tracker.local/DBHandler/scheduletask.php";
    private static string _deScheduleTask = "https://tracker.local/DBHandler/descheduletask.php";
    #endregion

    #region Users
    private static string _users = "https://tracker.local/DBHandler/admin_userssettings.php";
    #endregion

    #region Get/Set
    public static string TaskSettings { get => _taskSettings; set => _taskSettings = value; }
    public static string NewTask { get => _newTask; set => _newTask = value; }
    public static string UpdateTask { get => _updateTask; set => _updateTask = value; }
    public static string DeleteTask { get => _deleteTask; set => _deleteTask = value; }
    public static string ScheduleTask { get => _scheduleTask; set => _scheduleTask = value; }
    public static string DeScheduleTask { get => _deScheduleTask; set => _deScheduleTask = value; }
    public static string Users { get => _users; set => _users = value; }
    #endregion

    #endregion
}
