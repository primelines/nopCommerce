using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nop.Api.Framework.Dtos;

namespace Nop.Api.Framework.Mvc.ModelBinding.Binders;

/// <summary>
/// Represents a model binder provider for specific properties
/// </summary>
public partial class NopDtoBinderProvider : IModelBinderProvider
{
    IModelBinder IModelBinderProvider.GetBinder(ModelBinderProviderContext context)
    {
        //if (context.Metadata.PropertyName == nameof(BaseNopDto.CustomProperties) && context.Metadata.ModelType == typeof(Dictionary<string, string>))
        //    return new CustomPropertiesDtoBinder();

        if (!context.Metadata.IsComplexType && context.Metadata.ModelType == typeof(string)) 
            return new StringDtoBinder();

        return null;
    }
}