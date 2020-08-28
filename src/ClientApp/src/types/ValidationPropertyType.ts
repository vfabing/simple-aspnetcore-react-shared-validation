export enum ValidationPropertyType {
    NotNull = 'NotNullValidator',
	NotEmpty = 'NotEmptyValidator',
    MaximumLength = 'MaximumLengthValidator',
    LessThanOrEqual = 'LessThanOrEqualValidator',
    GreaterThanOrEqual = 'GreaterThanOrEqualValidator',
	AspNetCoreCompatibleEmail = 'AspNetCoreCompatibleEmailValidator',
}