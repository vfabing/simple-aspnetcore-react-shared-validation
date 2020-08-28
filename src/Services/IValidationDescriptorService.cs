using simple_aspnetcore_react_shared_validation.Dtos;
using System.Collections.Generic;

namespace simple_aspnetcore_react_shared_validation.Services
{

    public interface IValidationDescriptorService
    {
        /// <summary>
        /// Gets the validation descriptors for each validator type.
        /// Each validation dexcriptor is a dictionary mapping property name to its property validators' info.
        /// For example:
        /// {
        ///     "ValidatorName1": {
        ///         "PropertyName1": [
        ///             {
        ///                 "Name": "RuleName1",
        ///                 "ErrorMessage": "ErrorMessage1",
        ///             },
        ///             {
        ///                 "Name": "RuleName2",
        ///                 "ErrorMessage": "ErrorMessage2",
        ///             }
        ///         ]
        ///     }
        /// }
        /// </summary>
        /// <returns>A dictionary matching each validator type to its validation descriptors.</returns>
        IDictionary<string, Dictionary<string, List<PropertyValidatorInfo>>> GetValidationDescriptors();
    }
}
