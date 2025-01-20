using UnityEngine;

public class URLs : MonoBehaviour
{
    #region UsersURLs
    private static string _authorization = "https://unnamedplace.ru/dbhelper/authorization.php";
    private static string _userCurrency = "https://unnamedplace.ru/dbhelper/usercurrency.php";
    private static string _rating = "https://unnamedplace.ru/dbhelper/ratings.php";
    private static string _tutorial = "https://unnamedplace.ru/dbhelper/tutorials.php";
    private string _shopProduct = "https://unnamedplace.ru/dbhelper/shop.php";

    public static string Authorization { get => _authorization; }
    public static string UserCurrency { get => _userCurrency; }
    public static string UserRating { get => _rating; }
    public static string Tutorial { get => _tutorial; }
    public  string ShopProduct { get => _shopProduct; }

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
    public static string TaskSettings { get => _taskSettings; }
    public static string NewTask { get => _newTask; }
    public static string UpdateTask { get => _updateTask; }
    public static string DeleteTask { get => _deleteTask; }
    public static string ScheduleTask { get => _scheduleTask; }
    public static string DeScheduleTask { get => _deScheduleTask; }
    public static string Users { get => _users; }
    public static string DailyTask { get => _dailyTask; }
    public static string UserCompletedTask { get => _userCompletedTask; }
    public static string TaskOnReview { get => _taskOnReview; }
    
    #endregion

    #endregion

    #region RhytmMiniGame
    private static string _melodiesList = "https://unnamedplace.ru/dbhelper/getmelodies.php";
    public static string MelodiesList { get => _melodiesList; }
    #endregion
}
