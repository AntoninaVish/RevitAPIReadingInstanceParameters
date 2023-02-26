using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIReadingInstanceParameters
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand

    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            //выбираем объект и прочтем один из его параметров
            //прописываем логику для выбранного объекта
            var selectedRef = uidoc.Selection.PickObject(ObjectType.Element, "Выберите элемент");

            //нужно выбрать переменную типа Reference как элемент
            var selectedElement = doc.GetElement(selectedRef);

            //проверяем тип элемента
            if(selectedElement is Wall)
            {
                ////Первый способ выбора параметра
                ////параметер с имеем Length является стандартным и его название будет зависить от выбранного языка,
                ////если в русской версии REvit то "Длина"
                //Parameter lengthParameter1 = selectedElement.LookupParameter("Длина");

                ////проверяем тип данного параметра напр., дробное число
                //if(lengthParameter1.StorageType == StorageType.Double)
                //{
                //    TaskDialog.Show("Длина1", lengthParameter1.AsDouble().ToString()); //выводим значение данного параметра
                //}

                //Второй способ выбора параметра
                //Создаем еще одну переменную
                Parameter lengthParameter = selectedElement.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                                                            //CURVE_ELEM_LENGTH это внутренее имя параметра длины

                //проверяем тип данного параметра
                if(lengthParameter.StorageType == StorageType.Double)
                {
                    //значение из фут преобразовать в метры (создаем переменную)
                    double lengthValue = UnitUtils.ConvertFromInternalUnits(lengthParameter.AsDouble(), UnitTypeId.Meters);
                    TaskDialog.Show("Length2", lengthValue.ToString());
                }

            }

            return Result.Succeeded;
        }
    }
}
