using System;
using System.Linq;
using System.Web.Mvc;

namespace Notes.Binders
{
    public class FlagEnumModelBinder<T> : DefaultModelBinder where T : struct
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value != null)
            {
                var rawValues = value.RawValue as string[];
                if (rawValues != null)
                {
                    T result;
                    if (Enum.TryParse(string.Join(",", rawValues.Where(v => !string.IsNullOrWhiteSpace(v))), out result))
                    {
                        return result;
                    }
                }
            }
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}