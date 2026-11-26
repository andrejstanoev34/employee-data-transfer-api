namespace EmployeeDataImporterApi.Domain.Enums;

public enum IssueType
{
    MissingRequiredField, 
    EmailInvalid, 
    DuplicateEmail, 
    DuplicateEmployeeNumber,
    DepartmentUnknown, 
    RoleUnknown, 
    HireDateInvalid, 
    HireDateInFuture,
    DateOfBirthInvalid, 
    DateOfBirthTooYoung, 
    SalaryInvalid, 
    PhoneInvalid,
    OfficeUnknown, 
    ManagerUnknown, 
    ManagerCycle,
    InvalidSkillName,
    InvalidEmployeeNumber,
    MissingSkillNameField,
    SkillEmployeeAmbiguous,
    DuplicateSkillEmployeeNumber,
}
