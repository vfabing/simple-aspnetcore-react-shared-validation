using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Resources;
using FluentValidation.Validators;
using Microsoft.Extensions.DependencyInjection;
using simple_aspnetcore_react_shared_validation.Dtos;
using simple_aspnetcore_react_shared_validation.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace simple_aspnetcore_react_shared_validation.Services
{
    public class ValidationDescriptorService : IValidationDescriptorService
    {
        private readonly IList<IValidator> _validators;

        public ValidationDescriptorService(IEnumerable<Assembly> assembliesToScan, IServiceProvider serviceProvider)
        {
            _validators = assembliesToScan
                .Distinct()
                .SelectMany(a => a.ExportedTypes)
                .Where(t => IsValidValidatorType(t))
                .Select(t => (IValidator)serviceProvider.GetRequiredService(t))
                .ToList();
        }

        /// <summary>
        /// Retrieve all FluentValidation validators, associated with their validated properties and their associated validation rules.
        /// Example :
        /// "CreateUserDtoValidator" : {
        ///   "FirstName" : [validationRules],
        ///   "LastName" : [validationRules] }
        /// etc.
        /// </summary>
        /// <returns>All FluentValidation validators, associated with their validated properties and their associated validation rules</returns>
        public IDictionary<string, Dictionary<string, List<PropertyValidatorInfo>>> GetValidationDescriptors()
        {
            return _validators.Select(
                validator => new
                {
                    validator.GetType().Name,
                    PropertyValidationDescriptors = GetPropertyValidationDescriptors(validator)
                                                    .Where(pvd => pvd.PropertyValidators.Any())
                                                    .ToList()
                })
                .Where(x => x.PropertyValidationDescriptors.Any())
                .ToDictionary(
                    x => x.Name,
                    x => x.PropertyValidationDescriptors
                        .GroupBy(pvd => pvd.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.SelectMany(pvd => pvd.PropertyValidators).ToList()));
        }

        /// <summary>
        /// Retrieve from FluentValidation validator all properties validated, associated with their rules.
        /// Example : 
        /// "FirstName" : [{ name : "NotEmptyValidator" }, { name : "MaximumLengthValidator"}],
        /// "LastName" : [{ name : "NotEmptyValidator" }, { name : "MaximumLengthValidator"}],
        /// etc...
        /// </summary>
        /// <param name="validator">The FluentValidation validator to describe</param>
        /// <returns>A list of PropertyValidationDescriptor (intermediate type) corresponding to a property associated with its rules</returns>
        private static List<PropertyValidationDescriptor> GetPropertyValidationDescriptors(IValidator validator)
        {
            var propertyRules = ((IEnumerable<IValidationRule>)validator).Cast<PropertyRule>();
            var result = propertyRules.Select(
                    rule => new PropertyValidationDescriptor
                    {
                        PropertyName = rule.PropertyName,
                        PropertyValidators = rule.Validators
                                                 .Select(GetPropertyValidatorInfo)
                                                 .ToList()
                    })
                    .Where(d => d.PropertyValidators.Any())
                    .ToList();
            return result;
        }

        /// <summary>
        /// Convert FluentValidation IPropertyValidator into our Dto PropertyValidatorInfo.
        /// Produced JSON Example : 
        /// {
        ///   "name":"MaximumLengthValidator",
        ///   "errorMessage":"The length of '{PropertyName}' must be {MaxLength} characters or fewer. You entered {TotalLength} characters.",
        ///   "Min":0,
        ///   "Max":100
        /// }
        /// </summary>
        /// <param name="propertyValidator">The FluentValidation property to convert</param>
        /// <returns>a PropertyValidatorInfo Dto</returns>
        private static PropertyValidatorInfo GetPropertyValidatorInfo(IPropertyValidator propertyValidator)
        {
            var type = propertyValidator.GetType();
            var details = new PropertyValidatorInfo
            {
                Name = type.Name
            };

            var excludedProperties = typeof(IPropertyValidator).GetProperties();
            var props = type.GetProperties()
                .Where(p => p.GetMethod != null
                        && p.GetMethod.IsPublic
                        && ShouldSerialize(p.GetValue(propertyValidator)?.GetType() ?? p.PropertyType)
                        && excludedProperties.All(ex => ex.Name != p.Name && ex.PropertyType != p.PropertyType))
                .ToList();

            details.AdditionalData = props.ToDictionary(p => p.Name, p => p.GetValue(propertyValidator));

            try
            {
                details.ErrorMessage = propertyValidator.Options.ErrorMessageSource?.GetString(null);
            }
            catch (FluentValidationMessageFormatException)
            {
            }

            return details;
        }

        /// <summary>
        /// Indicates whether a type is exploitable by the client and should be serialized.
        /// </summary>
        private static bool ShouldSerialize(Type type)
        {
            return type.IsValueType
                || type == typeof(string)
                || type.IsArray
                || typeof(IEnumerable).IsAssignableFrom(type);
        }

        private static bool IsValidValidatorType(Type type)
        {
            return typeof(IValidator).IsAssignableFrom(type);
        }

        private class PropertyValidationDescriptor
        {
            public string PropertyName { get; set; }

            public IList<PropertyValidatorInfo> PropertyValidators { get; set; } = new List<PropertyValidatorInfo>();
        }
    }
}
