import { useEffect, useState, useCallback } from "react";
import { Dictionary } from '../types/Dictionary';
import { ValidationRules } from 'react-hook-form';
import { PropertyValidatorInfo } from "../types/PropertyValidatorInfo";
import { ValidationPropertyType } from "../types/ValidationPropertyType";

const useValidation = (validatorName: string) => {

    const [validationData, setValidationData] = useState<Dictionary<Dictionary<PropertyValidatorInfo[]>> | null>(null);
    const [validationRules, setValidationRules] = useState<Dictionary<ValidationRules>>();
    const [validationRulesProcessed, setValidationRulesProcessed] = useState<boolean>(false);

    useEffect(() => {
        if (!validationData) {
            (async function () {
                let response = await fetch("api/validation");
                setValidationData(await response.json());
            })();
        }
    }, [validationData])

    const _transformValidationRules = useCallback((propertyName: string, validationRulesInfo: PropertyValidatorInfo[]): ValidationRules => {
        var validationRules: ValidationRules = {};

        validationRulesInfo.forEach(rulesInfo => {
            switch (rulesInfo.name) {
                case ValidationPropertyType.NotNull:
                    validationRules.required = { value: true, message: rulesInfo.errorMessage?.replace("{PropertyName}", propertyName) ?? ValidationPropertyType.NotNull };
                    break;
                case ValidationPropertyType.NotEmpty:
                    validationRules.required = { value: true, message: rulesInfo.errorMessage?.replace("{PropertyName}", propertyName) ?? ValidationPropertyType.NotEmpty };
                    validationRules.minLength = { value: 1, message: rulesInfo.errorMessage?.replace("{PropertyName}", propertyName) ?? ValidationPropertyType.NotEmpty };
                    break;
                case ValidationPropertyType.MaximumLength:
                    validationRules.maxLength = rulesInfo.Max && { value: rulesInfo.Max, message: rulesInfo.errorMessage?.replace("{PropertyName}", propertyName) ?? ValidationPropertyType.MaximumLength }
                    break;
                case ValidationPropertyType.AspNetCoreCompatibleEmail:
                    validationRules.validate = { value: value => customEmailSimpleValidation(value, rulesInfo.errorMessage?.replace("{PropertyName}", propertyName) ?? ValidationPropertyType.AspNetCoreCompatibleEmail) };
                    break;
                case ValidationPropertyType.GreaterThanOrEqual:
                    validationRules.min = { value: rulesInfo.ValueToCompare, message: rulesInfo.errorMessage?.replace("{PropertyName}", propertyName).replace("{ComparisonValue}", rulesInfo.ValueToCompare as string) ?? ValidationPropertyType.GreaterThanOrEqual }
                    break;
                case ValidationPropertyType.LessThanOrEqual:
                    validationRules.max = { value: rulesInfo.ValueToCompare, message: rulesInfo.errorMessage?.replace("{PropertyName}", propertyName).replace("{ComparisonValue}", rulesInfo.ValueToCompare as string) ?? ValidationPropertyType.LessThanOrEqual }
                    break;
                default:
                    console.warn(`Not Implemented Rule name ${rulesInfo.name}`);
            }
        });

        return validationRules;
    }, []);

    const customEmailSimpleValidation = (value: string, message: string) => {
        const index = value.indexOf('@');
        if (index > 0 && index !== value.length - 1 && index === value.lastIndexOf('@')) {
            return;
        }
        return message;
    }

    useEffect(() => {
        if (validationData && !validationRulesProcessed) {
            let validatorInfo = validationData[validatorName];

            if (validatorInfo) {
                var result: Dictionary<ValidationRules> = {};
                let propertiesInfo = validatorInfo as Dictionary<PropertyValidatorInfo[]>;
                Object.keys(propertiesInfo).forEach(propertyName => {
                    result[propertyName] = _transformValidationRules(propertyName, propertiesInfo[propertyName]);
                });
                setValidationRules(result);
                setValidationRulesProcessed(true);
            }
        }
    }, [validationData, _transformValidationRules, validationRulesProcessed, validatorName]);

    return { validationRules }
}

export default useValidation;