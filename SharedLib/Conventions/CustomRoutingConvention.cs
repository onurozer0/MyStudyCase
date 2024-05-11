using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace SharedLib.Conventions
{
	public class CustomRoutingConvention : IControllerModelConvention
	{
		public void Apply(ControllerModel controller)
		{
			controller.ControllerName = controller.ControllerName.ToLower();
			foreach (var selector in controller.Selectors)
			{
				selector.AttributeRouteModel!.Template = selector.AttributeRouteModel.Template!.ToLower();
			}
			foreach (var action in controller.Actions)
			{
				action.ActionName = action.ActionName.ToLower();
				if (action.ActionName.Contains('ı') || action.ActionName.Contains('ü') || action.ActionName.Contains('ş') || action.ActionName.Contains('ç') || action.ActionName.Contains('ö'))
				{
					action.ActionName = action.ActionName.Replace('ı', 'i');
					action.ActionName = action.ActionName.Replace('ü', 'u');
					action.ActionName = action.ActionName.Replace('ş', 's');
					action.ActionName = action.ActionName.Replace('ç', 'c');
					action.ActionName = action.ActionName.Replace('ö', 'o');
				}
			}
		}
	}
}
