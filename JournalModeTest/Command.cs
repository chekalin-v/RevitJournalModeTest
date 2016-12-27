#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

#endregion

namespace JournalModeTest
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class JournalingModeNoCommandDataCommand : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            commandData.JournalData.Add("Журнал 1", "Начинаем выбирать элемент");

            Reference r = null;
            try
            {
                r = uidoc.Selection.PickObject(ObjectType.Element, "Выберите элемент");
            }
            catch (OperationCanceledException)
            {
                commandData.JournalData.Add("Журнал 2", "Выбор отменен");
                return Result.Cancelled;                
            }

            TaskDialog.Show("JournalMode", "Вы выбрали объект " + r.ElementId.IntegerValue);

            commandData.JournalData.Add("Журнал 3", "Вы выбрали объект " + r.ElementId.IntegerValue);

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Change ProjectInfo");
                doc.ProjectInformation.Author = "Victor Chekalin";
                commandData.JournalData.Add("Журнал 4", "Изменили автора проекта");
                tx.Commit();
            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    public class JournalingModeUsingCommandData : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            commandData.JournalData.Add("Журнал 1", "Начинаем выбирать элемент");

            Reference r = null;
            try
            {
                r = uidoc.Selection.PickObject(ObjectType.Element, "Выберите элемент");
            }
            catch (OperationCanceledException)
            {
                commandData.JournalData.Add("Журнал 2", "Выбор отменен");
                return Result.Cancelled;
            }

            TaskDialog.Show("JournalMode", "Вы выбрали объект " + r.ElementId.IntegerValue);

            commandData.JournalData.Add("Журнал 3", "Вы выбрали объект " + r.ElementId.IntegerValue);

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Change ProjectInfo");
                doc.ProjectInformation.Author = "Victor Chekalin";
                commandData.JournalData.Add("Журнал 4", "Изменили автора проекта");
                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
