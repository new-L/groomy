using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBTablesName : MonoBehaviour
{
    #region TaskTables
    private static string _tasksTable = "tasks";
    private static string _tasksDifficultyTable = "tasksdifficulty";
    private static string _tasksTypeTable = "taskstype";
    private static string _dailytasks = "dailytasks";
    #endregion

    #region UsersTables
    private static string _users = "user";
    #endregion

    [SerializeField] private List<string> taskTables;
    [SerializeField] private List<string> userTables;

    #region Get/Set
    public static string TasksTable { get => _tasksTable; }
    public static string TasksDifficultyTable { get => _tasksDifficultyTable; }
    public static string TasksTypeTable { get => _tasksTypeTable; }
    public static string Dailytasks { get => _dailytasks; }
    public static string Users { get => _users; set => _users = value; }
    public List<string> TaskTables { get => taskTables; set => taskTables = value; }
    #endregion


}
