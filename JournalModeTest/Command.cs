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

            commandData.JournalData.Add("������ 1", "�������� �������� �������");

            Reference r = null;
            try
            {
                r = uidoc.Selection.PickObject(ObjectType.Element, "�������� �������");
            }
            catch (OperationCanceledException)
            {
                commandData.JournalData.Add("������ 2", "����� �������");
                return Result.Cancelled;                
            }

            TaskDialog.Show("JournalMode", "�� ������� ������ " + r.ElementId.IntegerValue);

            commandData.JournalData.Add("������ 3", "�� ������� ������ " + r.ElementId.IntegerValue);

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Change ProjectInfo");
                doc.ProjectInformation.Author = "Victor Chekalin";
                commandData.JournalData.Add("������ 4", "�������� ������ �������");
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

            commandData.JournalData.Add("������ 1", "�������� �������� �������");

            Reference r = null;
            try
            {
                r = uidoc.Selection.PickObject(ObjectType.Element, "�������� �������");
            }
            catch (OperationCanceledException)
            {
                commandData.JournalData.Add("������ 2", "����� �������");
                return Result.Cancelled;
            }

            TaskDialog.Show("JournalMode", "�� ������� ������ " + r.ElementId.IntegerValue);

            commandData.JournalData.Add("������ 3", "�� ������� ������ " + r.ElementId.IntegerValue);

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Change ProjectInfo");
                doc.ProjectInformation.Author = "Victor Chekalin";
                commandData.JournalData.Add("������ 4", "�������� ������ �������");
                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
